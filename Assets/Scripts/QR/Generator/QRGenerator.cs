using System;
using System.Collections.Generic;
using System.Linq;
using QR.Algorithms;
using QR.Analysis;
using QR.Enums;
using QR.Masking;
using QR.Scriptable;
using QR.Utilities;
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
        private DataAnalyzer _analyzer;
        
        private EncodingType _encodingType;
        private int _size;
        private byte _dataSize;
        private int _capacity;
        private int _totalDataBitSize;
        
        // !! White = 0 Black = 1

        private void Awake()
        {
            _versionOne = Resources.Load<VersionData>("Data/Version1");
            _qrResolution = Resources.Load<QRResolution>("Data/QRResolutionData");
            
            _totalDataBitSize = VersionUtility.GetTotalBitCount(version);
                
            CheckVersionResolution();
        }

        private void Start()
        {
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
            
            // Essential Shapes
            SetOrientationShapes(ref texture, 0, 0);
            SetOrientationShapes(ref texture, 0, 13);
            SetOrientationShapes(ref texture, 13, 13);
            SetTimingStrips(ref texture);
            SetDarkModule(ref texture);
            // Format Info
            SetFormatInfo(ref texture, out MaskPattern maskPattern);
            // Data
            AnalyzeData();
            SetEncodingMode(ref texture); //204
            SetDataLength(ref texture);//196
            SetData(ref texture, data, out byte[] combinedData); //196 - (EC * 8) - Data - Padding = 0          
            SetErrorCorrectionData(ref texture, combinedData); // EC * 8
            SetMask(ref texture, maskPattern);
            texture.Apply();

            return texture;
        }

        private void AnalyzeData()
        {
            _analyzer = new DataAnalyzer(_versionOne, _qrResolution.VersionResolutions[version]);
            Debug.Log("Data Size: " + _analyzer.BitQueue.Count);
        }

        private void SetErrorCorrectionData(ref Texture2D texture, byte[] combinedData)
        {
            ErrorCorrection ec =
                new ErrorCorrection(ref texture, ref _analyzer, ref _versionOne, _encodingType, errorCorrectionLevel,
                    combinedData, _dataSize);
            ec.SetErrorCorrectionData();
        }

        private void SetData(ref Texture2D texture, string actualData, out byte[] combinedData)
        {
            QRData qrData = new QRData(ref texture, ref _analyzer, _versionOne, _encodingType, errorCorrectionLevel, actualData);
            qrData.SetData(out combinedData);
        }

        private void SetFormatInfo(ref Texture2D texture, out MaskPattern maskPattern)
        {
            maskPattern = new MaskPattern(out byte pattern, ref _versionOne, (byte)mask);
            BCH bch = new BCH(pattern, (byte)errorCorrectionLevel);
            
            int maskedFilterBits = bch.Calculation();
            //TODO: masked filter bits going to be set to texture
            FormatInfo formatInfo = new FormatInfo(ref texture, maskedFilterBits);
            // Debug.Log(maskedFilterBits);
            formatInfo.SetMaskedFormatBits();
            //001001110111110
        }

        private void SetMask(ref Texture2D texture, MaskPattern maskPattern)
        {
            maskPattern.SetMask(ref texture);
        }

        private void SetDataLength(ref Texture2D texture)
        {
            Length lengthModule = new Length(ref texture, ref _analyzer, _encodingType, _versionOne, _dataSize);
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

        private void SetDarkModule(ref Texture2D texture)
        {
            texture.SetPixel(8, (int)version * 4 + 3, Color.black);
        }

        private void SetEncodingMode(ref Texture2D texture)
        {
            //TODO: make it viable for every versions of QR
            Encoder encoder = new Encoder(ref texture, ref _analyzer,  _versionOne, errorCorrectionLevel, data, _dataSize);
            _encodingType = encoder.SetEncoding(out errorCorrectionLevel);
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

