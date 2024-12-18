using System;
using UnityEngine;
using QR.Enums;
using QR.Scriptables;
using System.Linq;

namespace QR.Converters
{
    public static class ByteDataConverter
    {
        public static DataConversion versionOne;

        public static bool[,] Convert(byte qrVersion, Encoder.EncodingType encodingType, byte dataOrder, char character = ' ') 
        {
            if (encodingType != Encoder.EncodingType.Byte) throw new NotImplementedException();

            BytePattern pattern = CheckPattern(qrVersion, encodingType, dataOrder);

            var patternBitOrder = versionOne.GetBitDetails(pattern);
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
        
        public static bool[,] Convert(byte qrVersion, Encoder.EncodingType encodingType, byte dataOrder, byte data) 
        {
            if (encodingType != Encoder.EncodingType.Byte) throw new NotImplementedException();

            BytePattern pattern = CheckPattern(qrVersion, encodingType, dataOrder);

            var patternBitOrder = versionOne.GetBitDetails(pattern);
            return GetBitTable(data, patternBitOrder.bitSize, patternBitOrder.bitOrder);        
        }

        private static bool[,] GetBitTable(byte data, (int x,int y) byteDataSize, int[] bitOrder)
        {
            int rowSize = byteDataSize.x;
            int columnSize = byteDataSize.y;
            bool[,] byteData = new bool[rowSize, columnSize];
            string binaryVersion = System.Convert.ToString(data, 2).PadLeft(rowSize * columnSize, '0');
            bool[] bitArray = binaryVersion.Select(x => x == '1').ToArray();

            for (int i = 0; i < bitArray.Length; i++)
            {
                byteData[i % rowSize, i / rowSize] = bitArray[bitOrder[i]];
            }
            return byteData;
        }

        private static BytePattern CheckPattern(byte qrVersion, Encoder.EncodingType encodingType, byte dataOrder)
        {
            if (qrVersion != 1) throw new NotImplementedException();
            if (encodingType != Encoder.EncodingType.Byte) throw new NotImplementedException();
            if (versionOne == null) versionOne = Resources.Load<DataConversion>("Data/Version1");

            return versionOne.VersionOnePatterns[dataOrder].Item1;
        }



    }

}
