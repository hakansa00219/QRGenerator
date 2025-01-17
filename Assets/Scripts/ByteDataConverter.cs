using System;
using UnityEngine;
using QR.Enums;
using QR.Scriptable;
using System.Linq;

namespace QR.Converters
{
    public static class ByteDataConverter
    {
        private static VersionData _versionOne;

        public static bool[,] Convert(byte qrVersion, EncodingType encodingType, BytePattern pattern, char character = ' ') 
        {
            // if (encodingType != EncodingType.Byte) throw new NotImplementedException();
            
            var patternBitOrder = _versionOne.GetBitDetails(pattern);
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
        
        public static bool[,] Convert(byte qrVersion, EncodingType encodingType, BytePattern pattern, byte data) 
        {
            // if (encodingType != EncodingType.Byte) throw new NotImplementedException();
            
            var patternBitOrder = _versionOne.GetBitDetails(pattern);
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
    }
}
