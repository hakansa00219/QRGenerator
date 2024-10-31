using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace QR
{
    public class Encoder
    {
        private readonly Dictionary<EncodingType, byte> characterSizeTable = new Dictionary<EncodingType, byte>()
        {
            { EncodingType.Numeric,      41},
            { EncodingType.Alphanumeric, 25},
            { EncodingType.Byte,         17},
            { EncodingType.Kanji,        10}
        };
        private const string alphaNumericPattern = @"^[0-9A-Za-z $%*+-./]+$";

        private byte charSize = 0;
        private EncodingType encodingType;
        private Texture2D texture;
        public EncodingType GetEncoding() => encodingType;
        public Encoder(ref Texture2D QRTexture, string data, byte charSize)
        {
            this.charSize = charSize;
            this.texture = QRTexture;

            if (IsNumericCompatible(data))
                encodingType = EncodingType.Numeric;
            else if (IsAlphanumericCompatible(data))
                encodingType = EncodingType.Alphanumeric;
            else if (IsKanjiCompatible(data))
                encodingType = EncodingType.Kanji;
            else if (IsByteCompatible(data))
                encodingType = EncodingType.Byte;
            else
                Debug.LogError("No encoding compatible data.");
        }
        public void SetEncoding()
        {
            //Write data to QR code depending to encoding type.
            Color[] whiteArray = new Color[4];
            Array.Fill(whiteArray, Color.white);
            //byte encoding 0100
            texture.SetPixels(19, 0, 2, 2, whiteArray);
            byte encodingTypeB = (byte)encodingType;
            string binaryVersion = Convert.ToString(encodingTypeB, 2).PadLeft(8, '0');
            switch (encodingType)
            {
                case EncodingType.Numeric:
                    texture.SetPixel(20, 0, Color.black);
                    break;
                case EncodingType.Alphanumeric:
                    texture.SetPixel(19, 0 , Color.black);
                    break;
                case EncodingType.Byte:
                    texture.SetPixel(20, 1, Color.black);
                    break;
                case EncodingType.Kanji:
                    texture.SetPixel(19, 1, Color.black);
                    break;
            }
      
        }

        // Char size a göre QR değişeceğinden bunu dinamik olarak değiştirilecek sonra.
        private bool IsNumericCompatible(string data)
        {
            return data.All(char.IsDigit) && charSize <= characterSizeTable[EncodingType.Numeric];
        }
        private bool IsAlphanumericCompatible(string data)
        {
            return Regex.IsMatch(data, alphaNumericPattern) && charSize <= characterSizeTable[EncodingType.Alphanumeric];
        }
        private bool IsKanjiCompatible(string data)
        {
            return data.Any(x => x >= 0x4E00 && x <= 0x9FBF) && charSize <= characterSizeTable[EncodingType.Kanji];
        }
        private bool IsByteCompatible(string data)
        {
            return charSize <= characterSizeTable[EncodingType.Byte];
        }

        public enum EncodingType : byte
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            Kanji = 8,
        }
    }
}