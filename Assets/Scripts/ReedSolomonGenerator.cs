using System;
using UnityEngine;

namespace QR
{
    public class ReedSolomonGenerator
    {
        private readonly int PRIMITIVE_POLY = 0x11D; // GF(256) için ilkel polinom
        private readonly int[] expTable = new int[256]; // Üstel tablo (antilog)
        private readonly int[] logTable = new int[256]; // Logaritma tablosu

        // GF(256) alanını başlat
        private void InitializeGaloisField()
        {
            int x = 1;
            for (int i = 0; i < 255; i++)
            {
                expTable[i] = x;
                logTable[x] = i;
                x <<= 1;
                if (x >= 256) x ^= PRIMITIVE_POLY; // GF(256) için mod işlemi
            }
            expTable[255] = expTable[0]; // Döngüyü tamamla
        }

        // Polinom çarpımı (mod 256 içinde)
        private int[] MultiplyPolynomials(int[] poly1, int[] poly2)
        {
            int[] result = new int[poly1.Length + poly2.Length - 1];

            for (int i = 0; i < poly1.Length; i++)
            {
                for (int j = 0; j < poly2.Length; j++)
                {
                    result[i + j] ^= GaloisMultiply(poly1[i], poly2[j]); // GF(256) içinde çarp
                }
            }

            return result;
        }

        // GF(256) çarpma işlemi
        private int GaloisMultiply(int a, int b)
        {
            if (a == 0 || b == 0) return 0;
            int sumLog = logTable[a] + logTable[b];
            return expTable[sumLog % 255]; // Mod 255 alınır (QR standardı)
        }

        // Reed-Solomon Üretici Polinomunu Oluştur
        private int[] GenerateGeneratorPolynomial(int ecCodewords)
        {
            int[] generator = { 1 }; // Başlangıçta G(x) = 1

            for (int i = 0; i < ecCodewords; i++)
            {
                int[] factor = { 1, expTable[i + 1] }; // (x - α^i)
                generator = MultiplyPolynomials(generator, factor);
            }

            return generator;
        }

        public int[] Generate(int errorCorrectionDataBlockSize)
        {
            InitializeGaloisField();
            int[] generatorPolynomial = GenerateGeneratorPolynomial(errorCorrectionDataBlockSize);

            return generatorPolynomial;
            Debug.Log("Reed-Solomon Generator Polynomial:");
            Debug.Log(string.Join(", ", generatorPolynomial));
        }
        
        public int[] ComputeErrorCorrectionCodewords(int[] data, int[] generatorPoly, int ecCodewords)
        {
            int[] paddedData = new int[data.Length + ecCodewords];
            Array.Copy(data, paddedData, data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                int coef = paddedData[i];
                if (coef != 0)
                {
                    int logCoef = logTable[coef];
                    for (int j = 0; j < generatorPoly.Length; j++)
                    {
                        paddedData[i + j] ^= expTable[(logCoef + logTable[generatorPoly[j]]) % 255];
                    }
                }
            }

            int[] errorCorrection = new int[ecCodewords];
            Array.Copy(paddedData, data.Length, errorCorrection, 0, ecCodewords);
            return errorCorrection;
        }
    }
}