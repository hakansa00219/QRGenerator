using System.IO;
using QR.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QR.Masking
{
    public struct MaskPattern
    {
        private readonly byte _pattern;
        private readonly MaskPatternData _maskPatternData;
        private readonly VersionData _versionData;

        public MaskPattern(out byte pattern, ref VersionData versionData, byte selectedPattern = 255)
        {
            _maskPatternData = Resources.Load<MaskPatternData>("Data/MaskPatternData");
            _versionData = versionData;

            if (_maskPatternData == null)
            {
                Debug.LogError("Mask pattern data could not be loaded.");
                throw new FileNotFoundException();
            }
            
            if (selectedPattern == 255)
            {
                int rndNo = Random.Range(0, _maskPatternData.MaskPatterns.Count);
                _pattern = (byte)rndNo;
            }
            else _pattern = selectedPattern;

            pattern = _pattern;
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

        public bool[,] UnmaskedVersion(bool[,] bitMatrix)
        {
            bool[,] bitMatrixCopy = new bool[bitMatrix.GetLength(0), bitMatrix.GetLength(1)];
            var matrix = _versionData.BitMatrix;
            
            for (var i = 0; i < matrix.GetLength(0); i++)
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                bitMatrixCopy[i, j] = bitMatrix[i, j];
                if(!matrix[i, j]) continue;
                
                var maskFuncValue = _maskPatternData.MaskPatterns[_pattern](i, j);
                var texturePixelValue = bitMatrixCopy[i, j];
                var maskedValue = maskFuncValue ? texturePixelValue : !bitMatrixCopy[i, j];  
                bitMatrixCopy[i,j] = maskedValue;
            }
            
            return bitMatrixCopy;
        }
    }
}