using System;
using QR.Converters;
using QR.Enums;
using QR.Scriptable;
using UnityEngine;

namespace QR
{
    public class QRData
    {
        private readonly Texture2D _texture;
        private readonly string _data;
        private readonly VersionData _versionData;
        private readonly EncodingType _encodingType;
        private readonly ErrorCorrectionLevel _errorCorrectionLevel;
        private const byte firstPadding = 0xEC;
        private const byte secondPadding = 0x11;
        public QRData(ref Texture2D texture, VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionLevel,  string data)
        {
            _texture = texture;
            _data = data;
            _versionData = versionData;
            _encodingType = encodingType;
            _errorCorrectionLevel = errorCorrectionLevel;
        }

        // !!! When data is over(Ver1) add 4 bit END block. Then add paddings to complete the data capacity.(so 3 more padding)
        // Rest is error correction.
        public void SetData()
        {
            int startByteIndex = 2;
            int endByteIndex = startByteIndex + _versionData.CharacterSizeTable[(_encodingType, _errorCorrectionLevel)].MaxMainData;
            for (int i = startByteIndex; i < endByteIndex; i++)
            {
                Debug.Log(i - 2 < _data.Length
                    ? (byte)_data[i - 2]
                    : ((i - 2 - _data.Length) % 2 == 0 ? firstPadding : secondPadding));
                // var bitDataTable = ByteDataConverter.Convert(1, _encodingType, (byte)i,
                //     i - 2 < _data.Length
                //         ? (byte)_data[i - 2]
                //         : ((i - 2 - _data.Length) % 2 == 0 ? firstPadding : secondPadding));
                // var initX = _versionData.Patterns[i].initPosition.X;
                // var initY = _versionData.Patterns[i].initPosition.Y;
                //
                // for (int y = 0; y < bitDataTable.GetLength(1); y++)
                // {
                //     for (int x = 0; x < bitDataTable.GetLength(0); x++)
                //     {
                //         Debug.Log($"({initX + x}, {initY + y}) - {(bitDataTable[x,y] ? Color.black : Color.white)}");
                //         _texture.SetPixel(initX + x, initY - y, bitDataTable[x,y] ? Color.black : Color.white);
                //     }
                // }
            }
        }
    }
}