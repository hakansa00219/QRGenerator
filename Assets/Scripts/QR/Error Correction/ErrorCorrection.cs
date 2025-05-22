using System.Collections.Generic;
using System.Linq;
using QR.Algorithms;
using QR.Analysis;
using QR.Enums;
using QR.Scriptable;
using QR.Structs;
using UnityEngine;

namespace QR
{
    public class ErrorCorrection
    {
        private readonly Texture2D _texture;
        private readonly IBitProvider _bitProvider;
        private readonly int _ecDataSize;
        private readonly byte[] _data;
        private readonly EncodingType _encodingType;
        private readonly int _dataLength;
        private const int ERROR_CORRECTION_LENGTH = 8;
        
        
        public ErrorCorrection(ref Texture2D texture,ref IBitProvider bitProvider, ref VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionType, byte[] data, int dataLength)
        {
            _texture = texture;    
            _bitProvider = bitProvider;
            _data = data;
            _encodingType = encodingType;
            _dataLength = dataLength;

            _ecDataSize = versionData.CharacterSizeTable[new QRType(encodingType, errorCorrectionType)]
                .ErrorCorrectionData;
        }

        public void SetErrorCorrectionData()
        {
            ReedSolomonGenerator generator = new ReedSolomonGenerator();

            List<byte> dataList = new List<byte>();
            dataList.Add((byte)((byte)_encodingType << 4 | _dataLength >> 4));
            dataList.Add((byte)(_dataLength << 4 | _data[0] >> 4));
            for (var i = 0; i < _dataLength; i++)
            { 
                dataList.Add((byte)(_data[i] << 4 | _data[i + 1] >> 4));
            }
            dataList.AddRange(_data.Skip(_dataLength + 1));
            
            // 0100        Enc
            // 00000100    Length    
            // 01010110    Data Start
            // 01100101    *
            // 01110010    *
            // 00110001    Data End
            // 0000        End
            // 11101100    Padding
            // 00010001    Padding
            // 11101100    Padding
            
            var ecblocks = generator.GenerateErrorCorrectionBlocks(dataList.ToArray() , _ecDataSize);
            
            for (int i = 0; i < _ecDataSize; i++) //17 characters 
            for (int j = ERROR_CORRECTION_LENGTH - 1; j >= 0; j--) //8 bits for each character
            {   
                var bitNode = _bitProvider.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((ecblocks[i] >> j) & 1) == 1 ? Color.black : Color.white);
                // Debug.Log($"{bitNode.X}, {bitNode.Y}");
            }
        }
        
        
    }
}