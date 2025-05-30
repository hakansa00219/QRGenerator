using System.Linq;
using UnityEngine;

namespace QR.Masking
{
    public class ThirdEvaluation : Evaluation
    {
        // Looks for patterns of dark-light-dark-dark-dark-light-dark that have four
        // light modules on either side. In other words, it looks for any of the
        // following two patterns: 1011101 0000 or 0000 1011101.
        private const int Pattern = 0b1011101;
        private const int PatternLength = 6;
        private const int ExtraPatternLength = 4;
        private const int PenaltyMultiplier = 40;
        
        public override int Calculation(in bool[,] bits, int horizontalSize, int verticalSize)
        {
            int penalty = 0;
            
            penalty += HorizontalPenalty(bits, horizontalSize, verticalSize);
            penalty += VerticalPenalty(bits, horizontalSize, verticalSize);
            
            Debug.Log($"Third Evaluation Penalty: {penalty}");
            return penalty;
        }

        private int HorizontalPenalty(in bool[,] bits, int horizontalSize, int verticalSize)
        {
            int penaltyCount = 0;
            
            for (int y = 0; y < verticalSize; y++)
            {
                int currentPattern = 0;
                for (int x = 0; x < horizontalSize; x++)
                {
                    //x=0,          0 or 1
                    //x=1,         00 or 10         =>      00    01      or      10    11
                    //x=2, 000 or 010 or 100 or 110 => 000 001 or 010 011 or 100 101 or 110 111
                    if(x > 0) currentPattern <<= 1;
                    // 1 = black = true, 0 = white = false
                    currentPattern += bits[x, y] ? 1 : 0;
                    
                    if (x < PatternLength) continue;
                    //  1011010 (90) << 1   *= 2;
                    // 10110100 (180) & 1111111       if (1) -128 == -2^7 if (0) do nothing 
                    //  0110100 (52)

                    // If pattern is not same
                    if (currentPattern != Pattern)
                    {
                        // left shift operation than get rid of 7th bit. Continue with the rest 6 bit.
                        currentPattern &= 0b00111111;
                    }
                    else // If pattern is same 
                    {
                        // Check Left and Right pixels. If 0000 exists add penalty.
                        int rightStart = x + 1;
                        int leftStart = x - ExtraPatternLength - PatternLength;
                        
                        bool rightCheck = x + ExtraPatternLength <= horizontalSize && IsAllZero(in bits, rightStart, y);
                        bool leftCheck = x - PatternLength >= ExtraPatternLength && IsAllZero(in bits, leftStart, y);
                        
                        //If any check is true = penalty
                        if (rightCheck || leftCheck)
                        {
                            if(leftCheck)
                                Debug.Log($"Horizontal Penalty!  {(x - ExtraPatternLength - PatternLength,y)}-{(x,y)}");
                            if(rightCheck)
                                Debug.Log($"Horizontal Penalty!  {(x - PatternLength, y)}-{(x + ExtraPatternLength,y)}");
                            penaltyCount++;
                        }
                    }
                }
            }
            
            bool IsAllZero(in bool[,] bits, int x, int y)
            {
                if (x + ExtraPatternLength > horizontalSize) return false;
                
                for (int i = 0; i < ExtraPatternLength; i++)
                {
                    if (bits[x + i, y]) return false;
                }
                return true;
            }
            
            return penaltyCount * PenaltyMultiplier;
        }
        private int VerticalPenalty(in bool[,] bits, int horizontalSize, int verticalSize)
        {
            int penaltyCount = 0;
            
            for (int x = 0; x < horizontalSize; x++)
            {
                int currentPattern = 0;
                for (int y = 0; y < verticalSize; y++)
                {
                    if(y > 0) currentPattern <<= 1;
                        
                    currentPattern += bits[x,y] ? 1 : 0;
                    
                    if (y < PatternLength) continue;

                    // If pattern is not same
                    if (currentPattern != Pattern)
                    {
                        // left shift operation than get rid of 7th bit. Continue with the rest 6 bit.
                        currentPattern &= 0b00111111;
                    }
                    else // If pattern is same 
                    {
                        // Check Up and Down pixels. If 0000 exists add penalty.
                        
                        int upstart = y + 1;
                        int downStart = y - ExtraPatternLength - PatternLength;
                        bool upCheck = y + ExtraPatternLength <= verticalSize && IsAllZero(in bits, x, upstart);
                        bool downCheck = y - PatternLength >= ExtraPatternLength && IsAllZero(in bits, x, downStart);
                        
                        //If any check is true = penalty
                        if (upCheck || downCheck)
                        {
                            if(downCheck)
                                Debug.Log($"Vertical Penalty!  {(x, downStart)}-{(x,y)}");
                            if(upCheck)
                                Debug.Log($"Vertical Penalty!  {(x, y - PatternLength)}-{(x, y + ExtraPatternLength)}");
                            penaltyCount++;
                        }
                    }
                }
            }
            
            bool IsAllZero(in bool[,] bits, int x, int y)
            {
                if (y + ExtraPatternLength > verticalSize) return false;
                
                for (int i = 0; i < ExtraPatternLength; i++)
                {
                    if (bits[x, y + i]) return false;
                }
                return true;
            }
            
            return penaltyCount * PenaltyMultiplier;
        }
    }
}