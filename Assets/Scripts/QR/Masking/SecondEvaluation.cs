using System.Text;
using UnityEngine;

namespace QR.Masking
{
    public class SecondEvaluation : Evaluation
    {
        // Every 2x2 same colored blocks = 3 penalty points.
        public override int Calculation(in bool[] bits, int horizontalSize, int verticalSize)
        {
            int penaltyCount = 0;
            for (int y = verticalSize - 1; y > 0; y--)
            {
                for (int x = 0; x < horizontalSize; x++)
                {
                    int bitIndex = x + y * horizontalSize;
                    
                    if (bitIndex % (horizontalSize) == 20) continue; // border pixel cannot be starter for penalty.
                    
                    bool bit = bits[bitIndex];
                    
                    if (bits[bitIndex + 1] == bit && // right pixel
                        bits[bitIndex - horizontalSize + 1] == bit && // down right pixel
                        bits[bitIndex - horizontalSize] == bit) // down pixel
                    {
                        penaltyCount++;
                    }
                }
            }
            
            int penalty = penaltyCount * 3;
            Debug.Log($"Second Evaluation Penalty: {penalty}");
            return penalty;
        }
    }
}