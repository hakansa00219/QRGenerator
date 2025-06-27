namespace QR.Structs
{
    public struct OrganizedData
    {
        public (byte data, int bitCount) Encoding;
        public (int data, int bitCount) Length;
        public (int[] data, int bitCount) Main;
        public (int[] data, int bitCount) Remaining;
        public (byte data, int bitCount) End;
        public (byte data, int bitCount) ByteAlignment;
        public (byte[] data, int bitCount) Padding;
    }
}