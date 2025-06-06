
namespace QR.Enums
{
    public enum ErrorCorrectionLevel : byte
    {
        High = 0b10,
        Quality = 0b11,
        Medium = 0b00,
        Low = 0b01
    }

    public enum EncodingType : byte
    {
        Numeric = 0b0001,
        Alphanumeric = 0b0010,
        Byte = 0b0100,
        Kanji = 0b1000,
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
