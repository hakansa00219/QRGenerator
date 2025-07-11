using System.IO;
using QR.Scriptable;
using ILogger = QR.Logger.ILogger;
using UnityEngine;

namespace QR.Masking
{
    public class MaskPatternTest
    {
        private readonly byte _pattern;
        private readonly MaskPatternData _maskPatternData;
        private readonly VersionData _versionData;
        
        public MaskPatternTest(VersionData versionData, byte selectedPattern)
        {
            _maskPatternData = Resources.Load<MaskPatternData>("Data/MaskPatternData");
            _versionData = versionData;
            
            if (_maskPatternData == null)
            {
                ILogger logger = new Logger.Logger(null);
                logger.LogError("Mask pattern data could not be loaded.");
                throw new FileNotFoundException();
            }
            
            _pattern = selectedPattern;
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