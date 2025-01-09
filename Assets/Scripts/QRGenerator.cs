using System;
using System.Linq;
using QR.Enums;
using QR.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QR
{
    public class QRGenerator : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private ErrorCorrectionLevel errorCorrectionLevel;
        [SerializeField] private string data;

        private byte _charSize;
        private DataConversion _versionOne;
        
        // !! White = 0 Black = 1

        private void Awake()
        {
            _versionOne = Resources.Load<DataConversion>("Data/Version1");
        }

        private void Start()
        {
            GameObject QR = new GameObject();

            SpriteRenderer rawImage = QR.AddComponent<SpriteRenderer>();
            rawImage.sprite = Sprite.Create(Generation(), new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            QR.name = "QR";
            
            Camera mainCamera = Camera.main;

            if (mainCamera != null) mainCamera.orthographicSize = rawImage.sprite.bounds.size.x * 0.7f;
        }

        private Texture2D Generation()
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Color randomColor = Random.value > 0.5f ? Color.white : Color.black;
            
                    texture.SetPixel(i, j, Color.white);
                }
            }

            _charSize = (byte)data.Length;

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
            SetErrorCorrection(ref texture);
            maskPattern = default;
        }

        private void SetMask(ref Texture2D texture, MaskPattern maskPattern)
        {
            // throw new NotImplementedException();
        }

        private void SetErrorCorrection(ref Texture2D texture)
        {
            ErrorCorrection errorCorrectionModule = new ErrorCorrection(ref texture, errorCorrectionLevel);
            errorCorrectionModule.SetErrorCorrection();
        }

        private void SetLength(ref Texture2D texture, byte dataOrder)
        {
            Length lengthModule = new Length(ref texture, _versionOne, dataOrder, _charSize);
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
            Encoder encoder = new Encoder(ref texture, data, _charSize);
            encoder.SetEncoding();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = -30; i < 30; i++)
            {
                Gizmos.DrawLine(new Vector3(-1, 0.005f + i * (0.01f), 0) , Vector3.right * width + new Vector3(0f,0.005f + i * (0.01f),0));
            }
            
            for (int i = -30; i < 30; i++)
            {
                Gizmos.DrawLine(new Vector3(0.005f + i * (0.01f),-1 , 0) , Vector3.up * height + new Vector3(0.005f + i * (0.01f),0f,0));
            }
            
        }
    }
}

