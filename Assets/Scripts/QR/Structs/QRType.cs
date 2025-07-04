using System;
using QR.Enums;

namespace QR.Structs
{
    public readonly struct QRType : IComparable<QRType>
    {
        public readonly EncodingType EncodingType; //Both needs to be public since I need this to serialize in editor.
        public readonly ErrorCorrectionLevel ErrorCorrectionLevel;

        public QRType(EncodingType encodingType, ErrorCorrectionLevel errorCorrectionLevel)
        {
            EncodingType = encodingType;
            ErrorCorrectionLevel = errorCorrectionLevel;
        }

        public override int GetHashCode()
        {
            return ((byte)EncodingType << 4) | (byte)ErrorCorrectionLevel;
        }
        
        public int CompareTo(QRType other)
        {
            return other.GetHashCode().CompareTo(GetHashCode());
        }
    }
}