using QR.Encoder;
using System;
using UnityEngine;
using QR.Enums;
using QR.Scriptables;
using System.Linq;

namespace QR.Converters
{
    public static class QRByteDataConverter
    {
        public static QRDataConversion versionOne;
        private struct QRByteData
        {
        }

        public static bool[,] Convert(byte qrVersion, QREncoder.EncodingType encodingType, byte charSize, byte dataOrder, char character = ' ') 
        {
            if (encodingType != QREncoder.EncodingType.Byte) throw new NotImplementedException();

            QREnums.BytePattern pattern = CheckPattern(qrVersion, encodingType, dataOrder);

            var patternBitOrder = versionOne.patternBitOrder[pattern];
            return GetBitTable(character, patternBitOrder.bitSize, patternBitOrder.bitOrder);        
        }

        private static bool[,] GetBitTable(char character, (int x,int y) byteDataSize, int[] bitOrder)
        {
            int rowSize = byteDataSize.x;
            int columnSize = byteDataSize.y;
            bool[,] byteData = new bool[rowSize, columnSize];
            string binaryVersion = System.Convert.ToString(character, 2).PadLeft(rowSize * columnSize, '0');
            bool[] bitArray = binaryVersion.Select(x => x == '1').ToArray();

            for (int i = 0; i < bitArray.Length; i++)
            {
                byteData[i % rowSize, i / rowSize] = bitArray[bitOrder[i]];
            }
            return byteData;
        }

        private static QREnums.BytePattern CheckPattern(byte qrVersion, QREncoder.EncodingType encodingType, byte dataOrder)
        {
            if (qrVersion != 1) throw new NotImplementedException();
            if (encodingType != QREncoder.EncodingType.Byte) throw new NotImplementedException();
            if (versionOne == null) versionOne = Resources.Load<QRDataConversion>("Data/Version1");

            return versionOne.VersionOnePatterns[dataOrder];
        }



    }

}
