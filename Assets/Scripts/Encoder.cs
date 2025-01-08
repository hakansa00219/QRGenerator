using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace QR
{
    public class Encoder
    {
        private readonly Dictionary<EncodingType, byte> _characterSizeTable = new ()
        {
            { EncodingType.Numeric,      41},
            { EncodingType.Alphanumeric, 25},
            { EncodingType.Byte,         17},
            { EncodingType.Kanji,        10}
        };
        private const string AlphaNumericPattern = @"^[0-9A-Za-z $%*+-./]+$";

        private readonly byte _charSize = 0;
        private readonly EncodingType _encodingType;
        private readonly Texture2D _texture;
        
        public Encoder(ref Texture2D qrTexture, string data, byte charSize)
        {
            _charSize = charSize;
            _texture = qrTexture;

            // Check the compatibility of data to find the encoding type.
            if (IsNumericCompatible(data))
                _encodingType = EncodingType.Numeric;
            else if (IsAlphanumericCompatible(data))
                _encodingType = EncodingType.Alphanumeric;
            else if (IsKanjiCompatible(data))
                _encodingType = EncodingType.Kanji;
            else if (IsByteCompatible())
                _encodingType = EncodingType.Byte;
            else
                Debug.LogError("No encoding compatible data.");
        }
        public void SetEncoding()
        {
            // Write data to QR code depending on encoding type.
            Color[] whiteArray = new Color[4];
            Array.Fill(whiteArray, Color.white);
            
            // Byte encoding 0100
            _texture.SetPixels(19, 0, 2, 2, whiteArray);
            
            switch (_encodingType)
            {
                case EncodingType.Numeric:
                    _texture.SetPixel(20, 0, Color.black);
                    break;
                case EncodingType.Alphanumeric:
                    _texture.SetPixel(19, 0 , Color.black);
                    break;
                case EncodingType.Byte:
                    _texture.SetPixel(20, 1, Color.black);
                    break;
                case EncodingType.Kanji:
                    _texture.SetPixel(19, 1, Color.black);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
      
        }

        // TODO: QR version will be dynamically changed depending to character size. Need to update this later.
        private bool IsNumericCompatible(string data)
        {
            return data.All(char.IsDigit) && _charSize <= _characterSizeTable[EncodingType.Numeric];
        }
        private bool IsAlphanumericCompatible(string data)
        {
            return Regex.IsMatch(data, AlphaNumericPattern) && _charSize <= _characterSizeTable[EncodingType.Alphanumeric];
        }
        private bool IsKanjiCompatible(string data)
        {
            return data.Any(x => x >= 0x4E00 && x <= 0x9FBF) && _charSize <= _characterSizeTable[EncodingType.Kanji];
        }
        private bool IsByteCompatible()
        {
            return _charSize <= _characterSizeTable[EncodingType.Byte];
        }

        public enum EncodingType : byte
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 3,
            Kanji = 4,
        }
    }
}