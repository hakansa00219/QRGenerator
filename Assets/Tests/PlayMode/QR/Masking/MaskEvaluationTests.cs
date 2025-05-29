using System.Linq;
using NUnit.Framework;
using QR.Masking;
using UnityEngine;

namespace Tests.PlayMode.QR.Masking
{
    public class MaskEvaluationTests
    {
        private const int ExpectedPenalty = 40;

        [Test]
        public void LeftPattern_ShouldReturnExpectedPenalty()
        {
            Texture2D texture = new Texture2D(11, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == ExpectedPenalty);

            Object.Destroy(texture);
        }
        [Test]
        public void LeftWrongPattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(11, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.black, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);

            Object.Destroy(texture);
        }
        [Test]
        public void LeftWrong1WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(8, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);

            Object.Destroy(texture);
        }
        [Test]
        public void LeftWrong2WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(9, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);

            Object.Destroy(texture);
        }
        [Test]
        public void LeftWrong3WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(10, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);

            Object.Destroy(texture);
        }
        [Test]
        public void RightPattern_ShouldReturnExpectedPenalty()
        {
            Texture2D texture = new Texture2D(11, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white, Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == ExpectedPenalty);
            
            Object.Destroy(texture);
        }
        [Test]
        public void RightWrong1WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(8, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void RightWrong2WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(9, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void RightWrong3WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(10, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void RightWrongPattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(11, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.black, Color.white, Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void UpPattern_ShouldReturnExpectedPenalty()
        {
            Texture2D texture = new Texture2D(1, 11, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white, Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == ExpectedPenalty);
            
            Object.Destroy(texture);
        }
        [Test]
        public void UpWrongPattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 11, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.black, Color.white, Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void UpWrong1WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 8, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void UpWrong2WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 9, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void UpWrong3WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 10, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white, Color.white});
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void DownPattern_ShouldReturnExpectedPenalty()
        {
            Texture2D texture = new Texture2D(1, 11, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == ExpectedPenalty);
            
            Object.Destroy(texture);
        }
        [Test]
        public void DownWrongPattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 11, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.black, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void DownWrong1WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 8, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void DownWrong2WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 9, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void DownWrong3WhitePattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 10, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void HorizontalNotPenaltyPattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(7, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void VerticalNotPenaltyPattern_ShouldReturn0()
        {
            Texture2D texture = new Texture2D(1, 7, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            ThirdEvaluation thirdEvaluation = new ThirdEvaluation();
            var pixels = texture.GetPixels();
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(pixels.Select(x => x == Color.black).ToArray(), texture.width, texture.height);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
    }
}