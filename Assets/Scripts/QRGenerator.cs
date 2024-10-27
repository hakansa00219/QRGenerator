using System;
using System.Linq;
using UnityEngine;
using QR.Encoder;
using Random = UnityEngine.Random;

namespace QR
{
    public class QRGenerator : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private string data;

        private byte charSize;
        private void Start()
        {
            GameObject QR = new GameObject();

            SpriteRenderer rawImage = QR.AddComponent<SpriteRenderer>();
            rawImage.sprite = Sprite.Create(Generation(), new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            // Generation();

            Camera.main.orthographicSize = rawImage.sprite.bounds.size.x * 0.7f;
        }

        private Texture2D Generation()
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB565, false);
            texture.filterMode = FilterMode.Point;
            texture.anisoLevel = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color randomColor = Random.value > 0.5f ? Color.white : Color.black;

                    texture.SetPixel(i, j, randomColor);
                }
            }

            this.charSize = (byte)data.Length;

            SetOrientationShapes(ref texture, 0, 0);
            SetOrientationShapes(ref texture, 0, 13);
            SetOrientationShapes(ref texture, 13, 13);
            SetTimingStrips(ref texture);
            SetWeirdPixelBlack(ref texture);
            SetEncodingType(ref texture);
            SetLength(ref texture);
            texture.Apply();

            return texture;
        }

        private void SetLength(ref Texture2D texture)
        {
            string binaryVersionLength = Convert.ToString(charSize, 2).PadLeft(8, '0');

            Debug.Log(binaryVersionLength);
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
            QREncoder encoder = new QREncoder(ref texture, data, charSize);
            encoder.SetEncoding();
        }
    }
}
