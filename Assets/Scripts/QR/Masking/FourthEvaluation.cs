using System;
using QR.Logger;

namespace QR.Masking
{
    public class FourthEvaluation : Evaluation
    {
        public override int Calculation(in bool[,] bits, int horizontalSize, int verticalSize, ILogger logger)
        {
            int totalBits = bits.Length;
            int darkBits = 0;
            for (int y = 0; y < verticalSize; y++)
            for (int x = 0; x < horizontalSize; x++)
            {
                if (bits[x, y]) darkBits++;
            }

            float darkPercentage = ((float)darkBits / totalBits) * 100f;
            int previousMultiple5 = (int)darkPercentage - ((int)darkPercentage % 5);
            int nextMultiple5 = (int)darkPercentage + (5 - (int)darkPercentage % 5);

            int previousSubstract = Math.Abs(previousMultiple5 - 50);
            int nextSubstract = Math.Abs(nextMultiple5 - 50);

            int previousDivision = previousSubstract / 5;
            int nextDivision = nextSubstract / 5;
            
            int smallestDivision = Math.Min(previousDivision, nextDivision);
            
            int penalty = smallestDivision * 10;
            logger.Log($"Fourth Penalty: {penalty}");
            return penalty;
        }
    }
}