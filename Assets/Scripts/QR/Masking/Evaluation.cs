using QR.Logger;

namespace QR.Masking
{
    public abstract class Evaluation
    {
        public abstract int Calculation(in bool[,] bits, int horizontalSize, int verticalSize, ILogger logger);
    }
}