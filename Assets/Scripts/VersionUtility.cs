using System;
using QR.Enums;
using Version = QR.Enums.Version;

namespace DefaultNamespace
{
    public class VersionUtility
    {
        public static int GetCharacterBitLength(Version version, EncodingType encodingType)
        {
            int length = 0;
            switch ((int)version)
            {
                case >= 1 and <= 9:
                    length = encodingType switch
                    {
                        EncodingType.Numeric => 10,
                        EncodingType.Alphanumeric => 9,
                        EncodingType.Byte => 8,
                        EncodingType.Kanji => 8,
                        _ => throw new ArgumentOutOfRangeException(nameof(encodingType), encodingType, null)
                    };
                    break;
                case >= 10 and <= 26:
                    length = encodingType switch
                    {
                        EncodingType.Numeric => 12,
                        EncodingType.Alphanumeric => 11,
                        EncodingType.Byte => 16,
                        EncodingType.Kanji => 10,
                        _ => throw new ArgumentOutOfRangeException(nameof(encodingType), encodingType, null)
                    };
                    break;
                case >= 27 and <= 40:
                    length = encodingType switch
                    {
                        EncodingType.Numeric => 14,
                        EncodingType.Alphanumeric => 13,
                        EncodingType.Byte => 16,
                        EncodingType.Kanji => 12,
                        _ => throw new ArgumentOutOfRangeException(nameof(encodingType), encodingType, null)
                    };
                    break;
            }
            return length;
        }


        public static int GetBitCount(Version version)
        {
            return version switch
            {
                Version.One => 208,
                _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
            };
        }
    }
}