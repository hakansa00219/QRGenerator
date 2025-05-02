using System.Collections.Generic;
using System.IO;
using QR.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QR.Masking
{
    public class MaskPattern
    {
        private readonly MaskPatternData _maskPatternData;
        private readonly VersionData _versionData;

        private readonly List<Evaluation> Evaluations = new List<Evaluation>()
        {
            new FirstEvaluation(),
            new SecondEvaluation(),
            new ThirdEvaluation(),
            new FourthEvaluation()
        };

        public Texture2D CachedBestTexture { get; private set; }
        public int LowestScore { get; private set; } = int.MaxValue;
        public byte BestMask { get; private set; } = 255;
        public MaskPattern(ref VersionData versionData, ref MaskPatternData maskPatternData)
        {
            _maskPatternData = maskPatternData;
            _versionData = versionData;
            
            if (_maskPatternData == null)
            {
                Debug.LogError("Mask pattern data could not be loaded.");
                throw new FileNotFoundException();
            }
        }

        public void CheckPenalty(ref Texture2D texture, byte mask)
        {
            int currentScore = SumOfEvaluations(ref texture, mask);
                
            if (currentScore >= LowestScore) return;
                
            LowestScore = currentScore;
            BestMask = mask;
            CachedBestTexture = texture;
        }

        private int SumOfEvaluations(ref Texture2D texture, byte mask)
        {
            int sum = 0;
            SetMask(ref texture, mask);
            foreach (var evaluation in Evaluations)
            {
                sum += evaluation.Calculation(mask, ref texture);
            }
            UnMask(ref texture, mask);
            Debug.Log($"Mask:{mask} - Sum of evaluations is {sum}");
            return sum;
        }
        
        public void SetMask(ref Texture2D texture, byte mask)
        {
            var matrix = _versionData.BitMatrix;
            for (var i = 0; i < matrix.GetLength(0); i++)
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                if(!matrix[i, j]) continue;
                
                var maskFuncValue = _maskPatternData.MaskPatterns[mask](i, j);
                var texturePixelValue = texture.GetPixel2D(i, j);
                var maskedValue = maskFuncValue ? texturePixelValue : (texturePixelValue == Color.white ? Color.black : Color.white);  
                texture.SetPixel2D(i, j, maskedValue);
            }
        }


        private void UnMask(ref Texture2D texture, byte mask)
        {
            SetMask(ref texture, mask);
        }
    }
}