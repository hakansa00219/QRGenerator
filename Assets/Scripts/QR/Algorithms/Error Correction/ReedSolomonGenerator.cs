using System;
using ILogger = QR.Logger.ILogger;

namespace QR.Algorithms
{
    public class ReedSolomonGenerator
    {
        private ILogger _logger;
        
        private readonly int PRIMITIVE_POLY = 0x11D; // GF(256) primitive polynomial
        private readonly byte[] expTable = new byte[256]; // expo table
        private readonly byte[] logTable = new byte[256]; // logarithmic table

        // GF(256) field
        private void InitializeGaloisField()
        {
            int value = 1;

            // Generate exponentiation (antilog) and logarithm tables
            for (int i = 0; i < 255; i++)
            {
                expTable[i] = (byte)value; // Store α^i
                logTable[value] = (byte)i; // Store log_α(value)

                // Multiply by primitive element (α = 0x02 in GF(256))
                value <<= 1; // Multiply by 2 (left shift)

                // If value overflows 8 bits, reduce using primitive polynomial (mod 0x11D)
                if (value >= 256)
                {
                    value ^= PRIMITIVE_POLY;
                }
            }

            // Duplicate last entry for easier modulo operation in lookup
            expTable[255] = 0;
        }

        // GF(256) multiply
        private byte GaloisMultiply(byte a, byte b)
        {
            if (a == 0 || b == 0) return 0;
            int logSum = logTable[a] + logTable[b];
            return expTable[logSum % 255]; // mod 255
        }

        private byte[] CreateGeneratorPolynomial(int codeWordsSize)
        {
            byte[] g = new byte[] {1}; // Start with 1 (x^0 term)

            for (int i = 0; i < codeWordsSize; i++)
            {
                byte[] temp = new byte[g.Length + 1];

                for (int j = 0; j < g.Length; j++)
                {
                    temp[j + 1] ^= GaloisMultiply(g[j], expTable[i]); 
                    temp[j] ^= g[j];
                }
                
                g = temp;
            }

            return g;
        }

        public byte[] GenerateErrorCorrectionBlocks(byte[] data, int errorCorrectionDataBlockSize, ILogger logger)
        {
            _logger = logger;
            
            InitializeGaloisField();
            byte[] ecBlocks = ComputeErrorCorrection(data, errorCorrectionDataBlockSize);

            return ecBlocks;
        }
        
        private byte[] ComputeErrorCorrection(byte[] data, int codeWordsSize)
        {
            byte[] generator = CreateGeneratorPolynomial(codeWordsSize);
            byte[] message = new byte[codeWordsSize + data.Length];

            _logger.Log("Data Words: " + string.Join(", ", data), false);
            Array.Copy(data, message, data.Length); // Copy data into the message

            for (int i = 0; i < data.Length; i++)
            {
                byte coefficient = message[i];

                if (coefficient != 0) // Skip zero terms
                {
                    for (int j = 0; j < generator.Length; j++)
                    {
                        message[i + j] ^= GaloisMultiply(coefficient, generator[j]);
                    }
                }
            }

            // Last 'numECCodewords' are the error correction codewords
            byte[] ecCodewords = new byte[codeWordsSize];
            // Debug.Log("150, 106, 201, 175, 226, 23, 128, 154, 76, 96, 209, 69, 45, 171, 227, 182, 8");
            Array.Copy(message, data.Length, ecCodewords, 0, codeWordsSize);
            _logger.Log("Error Correction Words: " + string.Join(", ", ecCodewords), false); //142, 180, 184, 32, 189, 250, 7, 144, 6, 122, 38, 178, 179, 128, 182, 185, 0
            // return new byte[] {150, 106, 201, 175, 226, 23, 128, 154, 76, 96, 209, 69, 45, 171, 227, 182, 8};
            return ecCodewords;
        }
    }
}