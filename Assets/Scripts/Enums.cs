using UnityEngine;

namespace QR.Enums
{
    public enum BytePattern : byte
    {
        Up = 0,
        Down = 1,
        Clockwise = 2,
        CtrClockwise = 3,
        Encoding = 4,
        End = 5,
    }

    public enum ErrorCorrectionLevel : byte
    {
        High = 0,
        Quality = 1,
        Medium = 2,
        Low = 3
    }

    public enum EncodingType : byte
    {
        Numeric = 1,
        Alphanumeric = 2,
        Byte = 3,
        Kanji = 4,
    }
}
