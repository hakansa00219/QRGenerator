using System;
using System.Linq;
using QR.Converters;
using QR.Enums;
using QR.Scriptable;
using UnityEngine;

namespace QR
{
    public class Length
    {
        private readonly Texture2D _texture;
        private readonly DataConversion _versionConversion;
        private readonly byte _dataOrder;
        private readonly byte _dataSize;
        
        public Length(ref Texture2D texture, DataConversion versionConversion, byte dataOrder, byte dataSize)
        {
            _versionConversion = versionConversion;
            _texture = texture;
            _dataOrder = dataOrder;
            _dataSize = dataSize;
        }
        
        public void SetLength()
        {
            bool[,] bitDataTable = ByteDataConverter.Convert(1, Encoder.EncodingType.Byte, _dataOrder, _dataSize);
            DataConversion.InitPosition initPosition = _versionConversion.VersionOnePatterns[_dataOrder].initPosition;

            for (int y = 0; y < bitDataTable.GetLength(1); y++)
            {
                for (int x = 0; x < bitDataTable.GetLength(0); x++)
                {
                    _texture.SetPixel(initPosition.X + x, initPosition.Y - y, bitDataTable[x,y] ? Color.black : Color.white);
                }
            }
        }
    }
}