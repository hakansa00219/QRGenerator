using System;
using QR.Enums;
using QR.Logger;
using Version = QR.Enums.Version;

namespace QR.Utilities
{
    public static class VersionUtility
    {
        public static int GetCharacterBitCount(Version version, EncodingType encodingType)
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

        public static int GetEncodingModeBitCount()
        {
            return 4;
        }

        public static int GetTotalBitCount(Version version, ILogger logger = null)
        {
            if (version == Version.One) return 208;

            logger?.LogError($"Not implemented for {version} version. Changed version to 1.");
            return 208;
        }
    }
}