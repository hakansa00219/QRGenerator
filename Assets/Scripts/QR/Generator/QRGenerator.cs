using QR.Algorithms;
using QR.Analysis;
using QR.Encoding;
using QR.Enums;
using QR.Masking;
using QR.Scriptable;
using mLogger = QR.Logger.Logger;
using ILogger = QR.Logger.ILogger;
using QR.Structs;
using QR.Utilities;
using Sirenix.OdinInspector;
using UI.Interactables;
using UI.Visualizer;
using UnityEngine;
using Version = QR.Enums.Version;

namespace QR
{
    public class QRGenerator : MonoBehaviour
    {
        [SerializeField] private Version version;
        [SerializeField, Range(0,7)] private int mask;
        [SerializeField] private ErrorCorrectionLevel errorCorrectionLevel;
        [SerializeField] private string data;
        [SerializeField] private bool debugMode;
        [SerializeField] private bool useMask;
        [SerializeField] private UIHandler uiHandler;
        
        private VersionData _versionOne;
        private QRResolution _qrResolution;
        private IBitProvider _bitProvider;
        private ITextureRenderer _textureRenderer;
        private MaskPatternData _maskPatternData;
        
        private ILogger _logger;
        [SerializeField] private MonoBehaviour logVisualizer;
        private ILogVisualizer _logVisualizer => logVisualizer as ILogVisualizer;

        [ShowInInspector, ReadOnly]
        private SpriteRenderer _spriteRenderer;
        [ShowInInspector, ReadOnly]
        private GameObject _qrTexture;
        [ShowInInspector, ReadOnly]
        private EncodingType _encodingType;
        [ShowInInspector, ReadOnly]
        private int _textureSize;
        [ShowInInspector, ReadOnly]
        private byte _byteSize;
        
        [ShowInInspector, ReadOnly]
        private int _capacity;
        [ShowInInspector, ReadOnly]
        private int _totalDataBitSize;
        
        public Version Version => version;
        public int Mask => mask;
        public ErrorCorrectionLevel ErrorCorrectionLevel => errorCorrectionLevel;
        public Texture2D LastTexture => _textureRenderer?.Texture;
        
        // !! White = 0 Black = 1

        private void Awake()
        {
            _logger = new mLogger(_logVisualizer);
            if (uiHandler == null)
            {
                _logger.LogError("UI handler is empty. Please assign it in the inspector.");
            }
            else
            {
                uiHandler.Init(this, _logger);
            }
            
            
            _versionOne = Resources.Load<VersionData>("Data/Version1");
            _qrResolution = Resources.Load<QRResolution>("Data/QRResolutionData");
            _maskPatternData = Resources.Load<MaskPatternData>("Data/MaskPatternData");
            
            CheckVersionResolution();
        }
        
        [Button("Generate QR Code")]
        public void Generation(string qrData)
        {
            if (string.IsNullOrEmpty(qrData))
                qrData = string.Empty;
            
            if (debugMode)
            {
                int size = qrData.Length;
                string fullData = qrData;
                for (int i = 0; i < size; i++)
                {
                    Generate(fullData[i..]);
                }
            }
            else
            {
                Generate(qrData);
            }
            
        }

        
        private void Generate(string qrData)
        {
            if(!string.IsNullOrEmpty(qrData)) 
                data = qrData;

            if (_qrTexture == null)
            {
                _qrTexture = new GameObject();
            }
            
            _qrTexture.name = $"QR-{data}";

            if (_spriteRenderer == null)
                _spriteRenderer = _qrTexture.AddComponent<SpriteRenderer>();
            
            _spriteRenderer.sprite = Sprite.Create(Generation(), new Rect(0, 0, _textureSize, _textureSize), new Vector2(0.5f, 0.5f));
            
            Camera mainCamera = Camera.main;

            if (mainCamera != null) mainCamera.orthographicSize = _spriteRenderer.sprite.bounds.size.x * 0.7f;
        }

        private Texture2D Generation()
        {
            Texture2D texture = TextureGeneration();
            
            // Fill everything with 0. Do not need it but yea
            for (int i = 0; i < _textureSize; i++)
            {
                for (int j = 0; j < _textureSize; j++)
                {
                    texture.SetPixel(i, j, Color.white);
                }
            }

            OrganizedData organizedData = new OrganizedData();
            
            _byteSize = (byte)System.Text.Encoding.UTF8.GetByteCount(data);
            
            // Data Analyzing / Bit Provider Service
            AnalyzeData();
            
            // Texture Rendering Service
            ImplementTextureRendererService(ref texture);
            
            // Essential Shapes
            SetFinderPatterns();
            SetTimingStrips();
            SetDarkModule();
            
            // Rendering Actual Data
            SetEncodingMode(ref organizedData);
            SetDataLength(ref organizedData);
            SetData(data, ref organizedData);     
            SetErrorCorrectionData(in organizedData); 
            
            // Mask and Format Info
            CheckBestMask(out MaskPattern maskPattern);
            SetFormatInfo();
            
            if (useMask)
                SetMask(maskPattern);
            
            texture.Apply();

            return texture;
        }

        private Texture2D TextureGeneration()
        {
            Texture2D texture = new Texture2D(_textureSize, _textureSize, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            return texture;
        }

        private void SetFinderPatterns()
        {
            int offset = _textureRenderer.TextureSize - 8;
            SetOrientationShapes(0, 0);
            SetOrientationShapes(0, offset);
            SetOrientationShapes(offset, offset);
        }

        private void ImplementTextureRendererService(ref Texture2D texture)
        {
            _textureRenderer = new Encoder(ref texture, _bitProvider);
        }

        private void AnalyzeData()
        {
            _bitProvider = new DataAnalyzer(_versionOne, _qrResolution.VersionResolutions[version]);
            _logger.Log("Data Size: " + _bitProvider.BitQueue.Count, false);
        }

        private void SetErrorCorrectionData(in OrganizedData organizedData)
        {
            ErrorCorrection ec = new ErrorCorrection(_textureRenderer, _logger, _versionOne, errorCorrectionLevel);
            ec.SetErrorCorrectionData(in organizedData);
        }

        private void SetData(string actualData, ref OrganizedData organizedData)
        {
            QRData qrData = new QRData(_textureRenderer, _logger, _versionOne, _encodingType, errorCorrectionLevel, actualData);
            qrData.SetData(ref organizedData);
        }

        private void CheckBestMask(out MaskPattern maskPattern)
        {
            maskPattern = new MaskPattern(_textureRenderer, _logger, _versionOne, _maskPatternData);
            
            for (byte i = 0; i < _maskPatternData.MaskPatterns.Count; i++)
            {
                BCH bch = new BCH(i, (byte)errorCorrectionLevel);
                int maskedFilterBits = bch.Calculation();
                FormatInfo formatInfo = new FormatInfo(_textureRenderer, maskedFilterBits);
                formatInfo.SetMaskedFormatBits();
                Texture2D copiedTexture = _textureRenderer.GetCopyTexture();
                maskPattern.CheckPenalty(copiedTexture, i);
            }
            
            mask = maskPattern.BestMask;
            _logger.Log($"Applied lowest penalty mask: {mask} - Score: {maskPattern.LowestScore}", false);
        }

        private void SetFormatInfo()
        {
            BCH bch = new BCH((byte)mask, (byte)errorCorrectionLevel);
            int maskedFilterBits = bch.Calculation();
            FormatInfo formatInfo = new FormatInfo(_textureRenderer, maskedFilterBits);
            formatInfo.SetMaskedFormatBits();
        }

        private void SetMask(MaskPattern maskPattern)
        {
            maskPattern.SetMask((byte)mask);
        }

        private void SetDataLength(ref OrganizedData organizedData)
        {
            Length lengthModule = new Length(_textureRenderer, _encodingType, _versionOne, data.Length);
            lengthModule.SetLength(ref organizedData);
        }

        private void SetOrientationShapes(int x, int y)
        {
            //7x7 black white shapes
            _textureRenderer.RenderingDataBlockToTexture(x, y, 8, 8, false);
            switch (x)
            {
                case 0 when y > 0:
                    y++;
                    break;
                case > 0 when y > 0:
                    x++;
                    y++;
                    break;
            }
            _textureRenderer.RenderingDataBlockToTexture(x, y, 7, 7, true);
            _textureRenderer.RenderingDataBlockToTexture(x + 1, y + 1, 5, 5, false);
            _textureRenderer.RenderingDataBlockToTexture(x + 2, y + 2, 3, 3, true);
        }

        private void SetTimingStrips()
        {
            //TODO: make it viable for every versions of QR
            for (int i = 0; i < 5; i++)
            {
                bool value = i % 2 == 0;
                _textureRenderer.RenderingBitToTexture(6, 8 + i, value);
                _textureRenderer.RenderingBitToTexture(8 + i, 6, value);
            }
        }

        private void SetDarkModule()
        {
            _textureRenderer.RenderingBitToTexture(8, _textureRenderer.TextureSize - 1 - ((int)version * 4 + 3), true);
        }

        private void SetEncodingMode(ref OrganizedData organizedData)
        {
            //TODO: make it viable for every versions of QR
            IEncodingSelection encoder = new EncodingSelector(_textureRenderer, _versionOne, data, _logger);
            encoder.SetEncoding(ref organizedData);
            
            _encodingType = encoder.SelectedEncodingType;
            errorCorrectionLevel = encoder.SelectedErrorCorrectionLevel;
            _capacity = _versionOne.MaxMainDataSizeTable[new QRType(_encodingType, errorCorrectionLevel)];
        }

        private void CheckVersionResolution()
        {
            if (version == Version.Auto)
            {
                _logger.LogError($"{version} is not implemented yet. Please select a version manually.");
            }

            if (version != Version.One)
            {
                _logger.LogError("Only Version one is implemented. Continues with Version one.");
            }

            version = Version.One;
            _textureSize = _qrResolution.VersionResolutions[version];
            _totalDataBitSize = VersionUtility.GetTotalBitCount(version, _logger);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = -30; i < 30; i++)
            {
                Gizmos.DrawLine(new Vector3(-1, 0.005f + i * (0.01f), 0) , Vector3.right * _textureSize + new Vector3(0f,0.005f + i * (0.01f),0));
            }
            
            for (int i = -30; i < 30; i++)
            {
                Gizmos.DrawLine(new Vector3(0.005f + i * (0.01f),-1 , 0) , Vector3.up * _textureSize + new Vector3(0.005f + i * (0.01f),0f,0));
            }
            
        }
    }
}

