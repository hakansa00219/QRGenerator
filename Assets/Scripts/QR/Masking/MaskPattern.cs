using System.Collections.Generic;
using System.IO;
using QR.Encoding;
using QR.Scriptable;
using UnityEngine;

namespace QR.Masking
{
    public class MaskPattern
    {
        private readonly MaskPatternData _maskPatternData;
        private readonly VersionData _versionData;
        private readonly ITextureRenderer _textureRenderer;

        private readonly List<Evaluation> _evaluations = new List<Evaluation>()
        {
            new FirstEvaluation(),
            new SecondEvaluation(),
            new ThirdEvaluation(),
            new FourthEvaluation()
        };

        public int LowestScore { get; private set; } = int.MaxValue;
        public byte BestMask { get; private set; } = 255;
        public MaskPattern(ITextureRenderer textureRenderer, VersionData versionData, MaskPatternData maskPatternData)
        {
            _textureRenderer = textureRenderer;
            _maskPatternData = maskPatternData;
            _versionData = versionData;
            
            if (_maskPatternData == null)
            {
                Debug.LogError("Mask pattern data could not be loaded.");
                throw new FileNotFoundException();
            }
        }

        public void CheckPenalty(Texture2D texture, byte mask)
        {
            int currentScore = SumOfEvaluations(texture, mask);
                
            if (currentScore >= LowestScore) return;
                
            LowestScore = currentScore;
            BestMask = 1; //for test
        }

        private int SumOfEvaluations(Texture2D texture, byte mask)
        {
            
            
            int sum = 0;
            SetMask(texture, mask);
            Color[] pixels = texture.GetPixels();
            foreach (var evaluation in _evaluations)
            {
                sum += evaluation.Calculation(pixels, texture.width, mask);
            }
            UnMask(texture, mask);
            Debug.Log($"Mask:{mask} - Sum of evaluations is {sum}");
            return sum;
        }
        
        private void SetMask(Texture2D texture, byte mask)
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
        
        public void SetMask(byte mask)
        {
            var matrix = _versionData.BitMatrix;
            for (var i = 0; i < matrix.GetLength(0); i++)
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                if(!matrix[i, j]) continue;
                
                var maskFuncValue = _maskPatternData.MaskPatterns[mask](i, j);
                var texturePixelValue = _textureRenderer.Texture.GetPixel2D(i, j);
                var maskedValue = maskFuncValue ? texturePixelValue : (texturePixelValue == Color.white ? Color.black : Color.white);  
                _textureRenderer.Texture.SetPixel2D(i, j, maskedValue);
            }
        }
        private void UnMask(Texture2D texture, byte mask)
        {
            SetMask(texture, mask);
        }
    }
}