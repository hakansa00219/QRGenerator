using System.Linq;
using NUnit.Framework;
using Logger = QR.Logger.Logger;
using ILogger = QR.Logger.ILogger;
using QR.Masking;
using UnityEngine;

namespace Tests.PlayMode.QR.Masking
{
    public class ThirdEvaluationTests
    {
        [Test]
        public void LeftPattern_ShouldReturn40()
        {
            Texture2D texture = new Texture2D(11, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 40);

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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 0);

            Object.Destroy(texture);
        }
        [Test]
        public void RightPattern_ShouldReturn40()
        {
            Texture2D texture = new Texture2D(11, 1, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white, Color.white, Color.white});
            texture.Apply();

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 40);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void UpPattern_ShouldReturn40()
        {
            Texture2D texture = new Texture2D(1, 11, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black , Color.white, Color.white, Color.white, Color.white});
            texture.Apply();

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 40);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }
        [Test]
        public void DownPattern_ShouldReturn40()
        {
            Texture2D texture = new Texture2D(1, 11, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            
            texture.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.white, Color.black, Color.white, Color.black, Color.black, Color.black, Color.white, Color.black });
            texture.Apply();

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 40);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
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

            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 0);
            
            Object.Destroy(texture);
        }

        [Test]
        public void Ver1DataMask2_ThirdEvaluation_ShouldReturn80()
        {
            //21x21, "Ver1" Data, Version 1, mask 2 
            bool[] testData = new bool[] { true, true, true, true, true, true, true, false, false, false, false, false, false, true, false, false, true, false, true, true, false, true, false, false, false, false, false, true, false, false, false, true, false, false, false, true, true, true, false, true, false, false, true, false, true, true, true, false, true, false, true, true, true, true, false, false, false, false, true, true, true, false, false, true, false, true, true, true, false, true, false, true, true, false, true, false, true, true, true, false, false, true, false, false, true, false, true, true, true, false, true, false, true, false, false, true, false, false, true, true, false, true, false, true, false, true, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, false, false, true, false, false, true, true, true, true, true, true, true, false, false, true, false, false, true, false, true, true, false, false, false, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, true, false, false, false, false, true, true, false, true, false, false, false, true, true, true, false, true, false, false, false, true, false, true, false, true, true, false, true, false, true, true, true, false, false, false, false, false, true, true, false, true, true, true, false, true, true, true, true, false, true, true, false, true, true, false, true, true, false, true, false, false, true, true, true, false, false, true, true, true, true, false, true, false, false, false, true, true, false, true, true, false, true, true, false, false, true, true, false, true, true, false, true, false, false, true, true, true, false, true, false, true, true, false, false, false, true, true, true, false, false, true, true, true, false, false, false, false, false, false, false, false, true, true, false, false, true, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, false, true, false, true, false, true, false, true, true, true, true, true, true, true, true, false, false, false, false, false, true, false, true, true, true, false, true, false, true, false, false, false, false, false, true, true, false, true, true, true, false, true, false, false, false, false, false, true, false, true, false, true, true, true, false, true, true, false, true, true, true, false, true, false, false, true, false, false, false, false, true, false, true, true, true, false, true, true, false, true, true, true, false, true, false, true, false, true, true, true, false, true, false, true, true, true, false, true, true, false, false, false, false, false, true, false, true, true, false, false, false, false, true, false, false, false, false, false, true, true, true, true, true, true, true, true, false, true, false, false, false, false, false, true, true, true, true, true, true, true };

            Texture2D texture = new Texture2D(21, 21, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };

            Color[] pixels = testData.Select(x => x ? Color.black : Color.white).ToArray();
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            Evaluation thirdEvaluation = new ThirdEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += thirdEvaluation.Calculation(texture.ConvertTo2DArray(), texture.width, texture.height, logger);
            
            Assert.IsTrue(penalty == 120);
            
            Object.Destroy(texture);
        }
    }
}