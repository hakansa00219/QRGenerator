using System;
using System.IO;
using QR.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QR
{
    public struct MaskPattern
    {
        private readonly byte _pattern;
        private readonly MaskPatternData _maskPatternData;
        private readonly VersionData _versionData;

        public MaskPattern(out byte pattern, VersionData versionData, byte selectedPattern = 255)
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
            foreach (var versionDataPattern in _versionData.Patterns)
            {
                var bitSize = _versionData.PatternBitOrder[versionDataPattern.Value.pattern].bitSize;
                var initPosition = versionDataPattern.Value.initPosition;
                
                for (int x = 0; x < bitSize.x; x++)
                for (int y = 0; y < bitSize.y; y++)
                {
                    //(19,0) (19,1) 
                    var xPos = (byte)(initPosition.X + x); 
                    var yPos = (byte)(initPosition.Y - y);
                    var maskFuncValue = _maskPatternData.MaskPatterns[_pattern](xPos, yPos);
                    var texturePixelValue = texture.GetPixel(xPos, yPos);
                    var maskedValue = maskFuncValue ? texturePixelValue : (texturePixelValue == Color.white ? Color.black : Color.white);  
                    texture.SetPixel(xPos, yPos, maskedValue);
                }
            }
        }
    }
}