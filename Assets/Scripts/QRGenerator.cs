using System;
using System.Linq;
using QR.Enums;
using QR.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;
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
        
        private EncodingType _encodingType;
        private int _size;
        private byte _dataSize;
        private byte _capacity;
        
        // !! White = 0 Black = 1

        private void Awake()
        {
            _versionOne = Resources.Load<VersionData>("Data/Version1");
            _qrResolution = Resources.Load<QRResolution>("Data/QRResolutionData");
        }

        private void Start()
        {
            CheckVersionResolution();
            
            GameObject QR = new GameObject();

            SpriteRenderer rawImage = QR.AddComponent<SpriteRenderer>();
            rawImage.sprite = Sprite.Create(Generation(), new Rect(0, 0, _size, _size), new Vector2(0.5f, 0.5f));
            QR.name = "QR";
            
            Camera mainCamera = Camera.main;

            if (mainCamera != null) mainCamera.orthographicSize = rawImage.sprite.bounds.size.x * 0.7f;
        }

        private Texture2D Generation()
        {
            Texture2D texture = new Texture2D(_size, _size, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    // Color randomColor = Random.value > 0.5f ? Color.white : Color.black;
            
                    texture.SetPixel(i, j, Color.white);
                }
            }

            _dataSize = (byte)data.Length;

            SetOrientationShapes(ref texture, 0, 0);
            SetOrientationShapes(ref texture, 0, 13);
            SetOrientationShapes(ref texture, 13, 13);
            SetTimingStrips(ref texture);
            SetWeirdPixelBlack(ref texture);
            SetEncodingType(ref texture);
            SetLength(ref texture, 1);
            SetFormatInfo(ref texture, out MaskPattern maskPattern);
            SetMask(ref texture, maskPattern);
            texture.Apply();

            return texture;
        }

        private void SetFormatInfo(ref Texture2D texture, out MaskPattern maskPattern)
        {
            maskPattern = new MaskPattern(out byte pattern, _versionOne, (byte)mask);
            BCH bch = new BCH(pattern, (byte)errorCorrectionLevel);
            
            int maskedFilterBits = bch.Calculation();
            //TODO: masked filter bits going to be set to texture
            FilterInfo filterInfo = new FilterInfo(ref texture, maskedFilterBits);
            // Debug.Log(maskedFilterBits);
            filterInfo.SetMaskedFilterBits();
            //001001110111110
        }

        private void SetMask(ref Texture2D texture, MaskPattern maskPattern)
        {
            maskPattern.SetMask(ref texture);
        }

        private void SetLength(ref Texture2D texture, byte dataOrder)
        {
            Length lengthModule = new Length(ref texture, _versionOne, dataOrder, _dataSize);
            lengthModule.SetLength();
        }

        private void SetOrientationShapes(ref Texture2D texture, int x, int y)
        {
            Color[] blackArray = new Color[49];
            Color[] whiteArray = new Color[64];

            Array.Fill(blackArray, Color.black);
            Array.Fill(whiteArray, Color.white);

            //7x7 black white shapes
            texture.SetPixels(x, y, 8, 8, whiteArray);
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
            texture.SetPixels(x, y, 7, 7, blackArray);
            texture.SetPixels(x + 1, y + 1, 5, 5, whiteArray);
            texture.SetPixels(x + 2, y + 2, 3, 3, blackArray);
        }

        private void SetTimingStrips(ref Texture2D texture)
        {
            //TODO: make it viable for every versions of QR
            texture.SetPixels(6, 8, 1, 5,
                Enumerable.Range(0, 5).Select(i => (i % 2 == 0) ? Color.black : Color.white).ToArray());
            texture.SetPixels(8, 14, 5, 1,
                Enumerable.Range(0, 5).Select(i => (i % 2 == 0) ? Color.black : Color.white).ToArray());
        }

        private void SetWeirdPixelBlack(ref Texture2D texture)
        {
            texture.SetPixel(8, 7, Color.black);
        }

        private void SetEncodingType(ref Texture2D texture)
        {
            //TODO: make it viable for every versions of QR
            Encoder encoder = new Encoder(ref texture, _versionOne, errorCorrectionLevel, data, _dataSize);
            _encodingType = encoder.SetEncoding(out errorCorrectionLevel);
            _capacity = _versionOne.CharacterSizeTable[(_encodingType, errorCorrectionLevel)];
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

