using System;
using QR.Enums;
using UnityEngine;

namespace QR
{
    public class ErrorCorrection
    {
        private readonly ErrorCorrectionLevel _level;
        private readonly Texture2D _texture;
        public ErrorCorrection(ref Texture2D texture, ErrorCorrectionLevel errorCorrectionLevel)
        {
            _level = errorCorrectionLevel;
            _texture = texture;
            
        }

        public void SetErrorCorrection()
        {
            string binaryVersion = Convert.ToString((byte)_level, 2).PadLeft(2, '0');
            // Debug.Log(binaryVersion);
            
            for (var i = 0; i < binaryVersion.Length; i++)
            {
                _texture.SetPixel(8, 0 + i, binaryVersion[i] == '0' ? Color.white : Color.black); 
                _texture.SetPixel(0 + i, 12, binaryVersion[i] == '0' ? Color.white : Color.black); 
            }
            
        }
    }
}