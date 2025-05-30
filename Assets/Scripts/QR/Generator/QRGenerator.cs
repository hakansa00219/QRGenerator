using System;
using QR.Algorithms;
using QR.Analysis;
using QR.Encoding;
using QR.Enums;
using QR.Masking;
using QR.Scriptable;
using QR.Structs;
using QR.Utilities;
using Sirenix.OdinInspector;
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
        
        private VersionData _versionOne;
        private QRResolution _qrResolution;
        private IBitProvider _bitProvider;
        private ITextureRenderer _textureRenderer;
        private MaskPatternData _maskPatternData;
        
        private EncodingType _encodingType;
        private int _size;
        private byte _dataSize;
        
        [ShowInInspector, ReadOnly]
        private int _capacity;
        [ShowInInspector, ReadOnly]
        private int _totalDataBitSize;
        
        // !! White = 0 Black = 1

        private void Awake()
        {
            _versionOne = Resources.Load<VersionData>("Data/Version1");
            _qrResolution = Resources.Load<QRResolution>("Data/QRResolutionData");
            _maskPatternData = Resources.Load<MaskPatternData>("Data/MaskPatternData");
            
            _totalDataBitSize = VersionUtility.GetTotalBitCount(version);
                
            CheckVersionResolution();
        }

        private void Start()
        {
            Generate();
        }

        public void Generate(string qrData = "")
        {
            if(!string.IsNullOrEmpty(qrData)) 
                data = qrData;
            
            GameObject QR = new GameObject();

            SpriteRenderer rawImage = QR.AddComponent<SpriteRenderer>();
            rawImage.sprite = Sprite.Create(Generation(), new Rect(0, 0, _size, _size), new Vector2(0.5f, 0.5f));
            QR.name = "QR";
            
            Camera mainCamera = Camera.main;

            if (mainCamera != null) mainCamera.orthographicSize = rawImage.sprite.bounds.size.x * 0.7f;
        }

        private Texture2D Generation()
        {
            Texture2D texture = TextureGeneration();
            
            // Fill everything with 0. Do not need it but yea
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    texture.SetPixel(i, j, Color.white);
                }
            }

            _dataSize = (byte)data.Length;
            
            // Data Analyzing / Bit Provider Service
            AnalyzeData();
            
            // Texture Rendering Service
            ImplementTextureRendererService(ref texture);
            
            // Essential Shapes
            SetFinderPatterns();
            SetTimingStrips();
            SetDarkModule();
            
            // Rendering Actual Data
            SetEncodingMode();
            SetDataLength();
            SetData(data, out byte[] combinedData);     
            SetErrorCorrectionData(combinedData); 
            
            // Mask and Format Info
            CheckBestMask(out MaskPattern maskPattern);
            SetFormatInfo();
            SetMask(maskPattern);
            texture.Apply();

            return texture;
        }

        private Texture2D TextureGeneration()
        {
            Texture2D texture = new Texture2D(_size, _size, TextureFormat.RGB565, false)
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
            Debug.Log("Data Size: " + _bitProvider.BitQueue.Count);
        }

        private void SetErrorCorrectionData(byte[] combinedData)
        {
            ErrorCorrection ec =
                new ErrorCorrection(_textureRenderer, _versionOne, _encodingType, errorCorrectionLevel,
                    combinedData, _dataSize);
            ec.SetErrorCorrectionData();
        }

        private void SetData(string actualData, out byte[] combinedData)
        {
            QRData qrData = new QRData(_textureRenderer, _versionOne, _encodingType, errorCorrectionLevel, actualData);
            qrData.SetData(out combinedData);
        }

        private void CheckBestMask(out MaskPattern maskPattern)
        {
            
            
            maskPattern = new MaskPattern(_textureRenderer, _versionOne, _maskPatternData);
            
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
            Debug.Log($"Applied lowest penalty mask: {mask} - Score: {maskPattern.LowestScore}");
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

        private void SetDataLength()
        {
            Length lengthModule = new Length(_textureRenderer, _encodingType, _versionOne, _dataSize);
            lengthModule.SetLength();
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

        private void SetEncodingMode()
        {
            //TODO: make it viable for every versions of QR
            IEncodingSelection encoder = new EncodingSelector(_textureRenderer, _versionOne, data, _dataSize);
            encoder.SetEncoding();
            
            _encodingType = encoder.SelectedEncodingType;
            errorCorrectionLevel = encoder.SelectedErrorCorrectionLevel;
            _capacity = _versionOne.CharacterSizeTable[new QRType(_encodingType, errorCorrectionLevel)].MaxMainData;
        }

        private void CheckVersionResolution()
        {
            if (version != Version.Auto)
                _size = _qrResolution.VersionResolutions[Version.One];
            else //Start with one then check compatibility. If it can be done with Version One return that if not continue with higher ones.
                throw new NotImplementedException();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = -30; i < 30; i++)
            {
                Gizmos.DrawLine(new Vector3(-1, 0.005f + i * (0.01f), 0) , Vector3.right * _size + new Vector3(0f,0.005f + i * (0.01f),0));
            }
            
            for (int i = -30; i < 30; i++)
            {
                Gizmos.DrawLine(new Vector3(0.005f + i * (0.01f),-1 , 0) , Vector3.up * _size + new Vector3(0.005f + i * (0.01f),0f,0));
            }
            
        }
    }
}

