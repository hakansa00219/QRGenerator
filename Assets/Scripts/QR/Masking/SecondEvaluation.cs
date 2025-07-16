
using QR.Logger;

namespace QR.Masking
{
    public class SecondEvaluation : Evaluation
    {
        // Every 2x2 same colored blocks = 3 penalty points.
        public override int Calculation(in bool[,] bits, int horizontalSize, int verticalSize, ILogger logger)
        {
            int penaltyCount = 0;
            for (int y = verticalSize - 1; y > 0; y--)
            {
                for (int x = 0; x < horizontalSize - 1; x++)
                {
                    bool bit = bits[x,y];
                    
                    if (bits[x + 1, y] == bit && // right pixel
                        bits[x + 1, y - 1] == bit && // down right pixel
                        bits[x, y - 1] == bit) // down pixel
                    {
                        penaltyCount++;
                    }
                }
            }
            
            int penalty = penaltyCount * 3;
            logger.Log($"Second Evaluation Penalty: {penalty}", false);
            return penalty;
        }
    }
}