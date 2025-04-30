using System.Collections.Generic;
using System.IO;
using QR.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QR.Masking
{
    public class MaskPattern
    {
        private readonly byte _pattern;
        private readonly MaskPatternData _maskPatternData;
        private readonly VersionData _versionData;

        private readonly List<Evaluation> Evaluations = new List<Evaluation>()
        {
            new FirstEvaluation(),
            new SecondEvaluation(),
            new ThirdEvaluation(),
            new FourthEvaluation()
        };
        
        public MaskPattern(out byte pattern, ref VersionData versionData, ref Texture2D texture)
        {
            _maskPatternData = Resources.Load<MaskPatternData>("Data/MaskPatternData");
            _versionData = versionData;
            
            if (_maskPatternData == null)
            {
                Debug.LogError("Mask pattern data could not be loaded.");
                throw new FileNotFoundException();
            }

            pattern = GetBestPattern(ref texture);
        }

        private byte GetBestPattern(ref Texture2D texture)
        {
            int lowestScore = int.MaxValue;
            byte bestMask = 255;
            for (byte i = 0; i < _maskPatternData.MaskPatterns.Count; i++)
            {
                int currentScore = SumOfEvaluations(i, ref texture);
                
                if (currentScore >= lowestScore) continue;
                
                lowestScore = currentScore;
                bestMask = (byte)i;
            }
            return bestMask;
        }

        private int SumOfEvaluations(byte mask, ref Texture2D texture)
        {
            int sum = 0;
            foreach (var evaluation in Evaluations)
            {
                sum += evaluation.Calculation(mask, ref texture);
            }
            return sum;
        }

        public void SetMask(ref Texture2D texture)
        {
            var matrix = _versionData.BitMatrix;
            for (var i = 0; i < matrix.GetLength(0); i++)
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                if(!matrix[i, j]) continue;
                
                var maskFuncValue = _maskPatternData.MaskPatterns[_pattern](i, j);
                var texturePixelValue = texture.GetPixel2D(i, j);
                var maskedValue = maskFuncValue ? texturePixelValue : (texturePixelValue == Color.white ? Color.black : Color.white);  
                texture.SetPixel2D(i, j, maskedValue);
            }
        }
    }
}