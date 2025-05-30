using UnityEngine;

namespace QR.Masking
{
    public class FirstEvaluation : Evaluation
    {
        private const int PenaltyCount = 5;
        private const int PenaltyIncrement = 3;
        public override int Calculation(in bool[,] bits, int horizontalSize, int verticalSize)
        {
            int horizontalTotal = HorizontalPenalty(bits, horizontalSize, verticalSize);
            int verticalTotal = VerticalPenalty(bits, horizontalSize, verticalSize);
            
            int sum = horizontalTotal + verticalTotal;
            Debug.Log($"First Evaluation Penalty: {sum}");
            return sum;
        }

        // Calculation of horizontal/vertical line penalty. If it has more than 5 same color back to back it gives penalty to evaluation.
        private static int HorizontalPenalty(in bool[,] pixels, int horizontalSize, int verticalSize)
        {
            int total = 0;
            for (int y = 0; y < verticalSize; y++)
            {
                bool lastBit = false;
                int stackCount = 0;
                for (int x = 0; x < horizontalSize; x++)
                {
                    bool nextBit = pixels[x,y];

                    if (lastBit == nextBit) stackCount++; // Same bit  ++
                    else // Different bit reset.
                    {
                        lastBit = nextBit;
                        stackCount = 1;
                        continue;
                    }

                    switch (stackCount)
                    {
                        case PenaltyCount:
                            total += PenaltyIncrement;
                            break;
                        case > PenaltyCount:
                            total++;
                            break;
                    }
                }
                
                // Debug.Log($"Line: {y} | Total: {total}");
            }
            
            return total;
        }
        
        private static int VerticalPenalty(in bool[,] pixels, int horizontalSize, int verticalSize)
        {
            int total = 0;
            for (int x = 0; x < horizontalSize; x++)
            {
                bool lastBit = false;
                int stackCount = 0;
                for (int y = 0; y < verticalSize; y++)
                {
                    bool nextBit = pixels[x,y];

                    if (lastBit == nextBit) stackCount++; // Same bit  ++
                    else // Different bit reset.
                    {
                        lastBit = nextBit;
                        stackCount = 1;
                        continue;
                    }

                    switch (stackCount)
                    {
                        case PenaltyCount:
                            total += PenaltyIncrement;
                            break;
                        case > PenaltyCount:
                            total++;
                            break;
                    }
                }
                
                // Debug.Log($"Line: {y} | Total: {total}");
            }
            
            return total;
        }
    }
}