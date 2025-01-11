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
        High = 2,
        Quality = 3,
        Medium = 0,
        Low = 1
    }

    public enum EncodingType : byte
    {
        Numeric = 1,
        Alphanumeric = 2,
        Byte = 3,
        Kanji = 4,
    }

    public enum Version : byte
    {
        Auto = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
    }
}
