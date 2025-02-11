using System;
using UnityEngine;

namespace QR.Algorithms
{
    public class ReedSolomonGenerator
    {
        private readonly int PRIMITIVE_POLY = 0x11D; // GF(256) için ilkel polinom
        private readonly byte[] expTable = new byte[256]; // Üstel tablo (antilog)
        private readonly byte[] logTable = new byte[256]; // Logaritma tablosu

        // GF(256) alanını başlat
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
            expTable[255] = expTable[0];
            
            Debug.Log("Exponentiation Table (expTable):");
            string s1 = "";
            for (int i = 0; i < 256; i++)
                s1 += $"{expTable[i]:X2} ";
            Debug.Log(s1);
            Debug.Log("Logarithm Table (logTable):");
            string s2 = "";
            for (int i = 0; i < 256; i++) 
                s2 += $"{logTable[i]:X2} ";
            Debug.Log(s2);
        }

        // GF(256) çarpma işlemi
        private byte GaloisMultiply(byte a, byte b)
        {
            if (a == 0 || b == 0) return 0;
            int logSum = logTable[a] + logTable[b];
            return expTable[logSum % 255]; // Mod 255 alınır (QR standardı)
        }

        private byte[] GenerateGeneratorPolynomial(int numECCodewords)
        {
            byte[] g = new byte[] {1}; // Start with 1 (x^0 term)

            for (int i = 0; i < numECCodewords; i++)
            {
                byte[] newG = new byte[g.Length + 1];

                for (int j = 0; j < g.Length; j++)
                {
                    newG[j] ^= GaloisMultiply(g[j], expTable[i]); // (x - α^i)
                }

                newG[g.Length] = 1;
                g = newG;
            }

            return g;
        }

        public byte[] GenerateECBlocks(byte[] data, int errorCorrectionDataBlockSize)
        {
            InitializeGaloisField();
            byte[] ecBlocks = ComputeErrorCorrection(data, errorCorrectionDataBlockSize);

            return ecBlocks;
        }
        
        private byte[] ComputeErrorCorrection(byte[] data, int numECCodewords)
        {
            byte[] generator = GenerateGeneratorPolynomial(numECCodewords);
            byte[] message = new byte[data.Length + numECCodewords];

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
            byte[] ecCodewords = new byte[numECCodewords];
            Array.Copy(message, data.Length, ecCodewords, 0, numECCodewords);

            return ecCodewords;
        }
    }
}