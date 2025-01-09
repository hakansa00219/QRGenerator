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

        public MaskPattern(byte selectedPattern = 255)
        {
            _maskPatternData = Resources.Load<MaskPatternData>("Data/MaskPatternData");

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
        }

        public bool SetMask(byte x, byte y)
        {
            return _maskPatternData.MaskPatterns[_pattern](x, y);
        }
    }
}