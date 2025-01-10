using System;

namespace QR
{
    public class BCH
    {
        private const int PolynomialGenerator = 0b10100110111;
        private const int FormatMask = 0b101010000010010;

        private readonly byte _maskPattern;
        private readonly byte _errorCorrection;
        
        public BCH(byte maskPattern, byte errorCorrection)
        {
            _maskPattern = maskPattern;
            _errorCorrection = errorCorrection;
        }
        
        public int Calculation()
        {
            // Combined error correction and mask pattern bytes. | 11100
            int ecMaskBits = (_errorCorrection << 3) | _maskPattern;
            // Shift left by 10 | 111000000000000
            int formatBits = ecMaskBits << 10;

            // Perform modulo-2 polynomial division
            for (int i = 14; i >= 10; i--)
            {
                // Check if the current bit is set
                if ((formatBits & (1 << i)) != 0)
                {
                    // XOR with the generator polynomial shifted to align with the current bit.
                    // 14 - 100000000000000 xor 101001101110000 = 010001101110000
                    // 13 -  10001101110000 xor 10100110111000 = 00101011001000
                    // 11 -    101011001000 xor 101001101110 = 000010100110
                    formatBits ^= PolynomialGenerator << (i - 10);
                }
            }
            
            // The remainder is the last 10 bits
            int remainder = formatBits & 0b1111111111;
            
            // Combine the 5-bit input with the 10-bit remainder
            return ((ecMaskBits << 10) | remainder) ^ FormatMask;
        }
    }
}