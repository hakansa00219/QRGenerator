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
        private readonly Texture2D _texture;

        public MaskPattern(ref Texture2D texture, byte selectedPattern = 255)
        {
            _maskPatternData = Resources.Load<MaskPatternData>("Data/MaskPatternData");
            _texture = texture;

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

        public void SetMaskPattern(out byte pattern)
        {
            string binaryVersion = Convert.ToString(_pattern, 2).PadLeft(3, '0');

            for (var i = 0; i < binaryVersion.Length; i++)
            {
                _texture.SetPixel(8, 2 + i, binaryVersion[i] == '0' ? Color.white : Color.black); 
                _texture.SetPixel(2 + i, 12, binaryVersion[i] == '0' ? Color.white : Color.black); 
            }
           
            pattern = _pattern;
        }

        public bool SetMask(byte x, byte y)
        {
            return _maskPatternData.MaskPatterns[_pattern](x, y);
        }
    }
}