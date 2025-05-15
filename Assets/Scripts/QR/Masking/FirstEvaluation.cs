using UnityEngine;

namespace QR.Masking
{
    public class FirstEvaluation : Evaluation
    {
        public override int Calculation(byte mask, ref Texture2D texture)
        {
            int horizontalTotal = 0, verticalTotal = 0;
            
            int width = texture.width;
            int height = texture.height;

            var test = new GameObject(mask.ToString(), typeof(SpriteRenderer));
            test.SetActive(false);
            SpriteRenderer rawImage = test.GetComponent<SpriteRenderer>();
            
            Texture2D newtx = new Texture2D(21, 21, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            newtx.SetPixels(texture.GetPixels());
            newtx.Apply();
            
            rawImage.sprite = Sprite.Create(newtx, new Rect(0, 0, 21, 21), new Vector2(0.5f, 0.5f));

            int stackCount;
            Color lastBitColor;
            
            for (int y = 0; y < height; y++)
            {
                lastBitColor = Color.clear;
                stackCount = 0;
                for (int x = 0; x < width; x++)
                {
                    var nextBitColor = texture.GetPixel2D(x, y);
                    if (lastBitColor == nextBitColor) stackCount++;
                    else
                    {
                        lastBitColor = nextBitColor;
                        stackCount = 1;
                        continue;
                    }

                    switch (stackCount)
                    {
                        case 5:
                            horizontalTotal += 3;
                            break;
                        case > 5:
                            horizontalTotal++;
                            break;
                    }
                }
            }


            for (int x = 0; x < width; x++)
            {
                lastBitColor = Color.clear;
                stackCount = 0;
                for (int y = 0; y < height; y++)
                {
                    var nextBitColor = texture.GetPixel2D(x, y);
                    if (lastBitColor == nextBitColor) stackCount++;
                    else
                    {
                        lastBitColor = nextBitColor;
                        stackCount = 1;
                        continue;
                    }

                    switch (stackCount)
                    {
                        case 5:
                            verticalTotal += 3;
                            break;
                        case > 5:
                            verticalTotal++;
                            break;
                    }
                }
            }

            return horizontalTotal + verticalTotal;
        }
    }
}