using UnityEngine;

namespace QR.Masking
{
    public class FirstEvaluation : Evaluation
    {
        public override int Calculation(in Color[] pixels, int texSize, byte mask)
        {
            int horizontalTotal = CalculateTotals(pixels, texSize, true);
            int verticalTotal = CalculateTotals(pixels, texSize, false);
            
            return horizontalTotal + verticalTotal;
        }

        /// <summary>
        /// If horizontal/vertical line has more than 5 same color back to back it gives penalty to evaluation.
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="texSize"></param>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        private static int CalculateTotals(in Color[] pixels, int texSize, bool isHorizontal)
        {
            const int penaltyCount = 5;
            const int penaltyIncrement = 3;
            
            int total = 0;
            for (int y = 0; y < texSize; y++)
            {
                Color lastBitColor = Color.clear;
                int stackCount = 0;
                for (int x = 0; x < texSize; x++)
                {
                    int index = isHorizontal ? y * texSize + x : x * texSize + y;
                    Color nextBitColor = pixels[index];
                    
                    if (lastBitColor == nextBitColor) stackCount++;
                    else
                    {
                        lastBitColor = nextBitColor;
                        stackCount = 1;
                        continue;
                    }

                    switch (stackCount)
                    {
                        case penaltyCount:
                            total += penaltyIncrement;
                            break;
                        case > penaltyCount:
                            total++;
                            break;
                    }
                }
            }

            return total;
        }
    }
}