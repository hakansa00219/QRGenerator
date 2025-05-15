using System;
using QR.Enums;

namespace QR.Utilities
{
    public class QRUtility
    {
        public static int GetCharacterBitSize(EncodingType encodingType, int pairSize = 1)
        {
            return encodingType switch
            {
                EncodingType.Numeric when pairSize == 3 => 10,
                EncodingType.Numeric when pairSize == 2 => 7,
                EncodingType.Numeric when pairSize == 1 => 4,
                EncodingType.Alphanumeric when pairSize == 2 => 11,
                EncodingType.Alphanumeric when pairSize == 1 => 6,
                EncodingType.Byte => 8,
                EncodingType.Kanji => 13,
                _ => throw new ArgumentOutOfRangeException(nameof(encodingType), encodingType, null)
            };
        }
    }
}