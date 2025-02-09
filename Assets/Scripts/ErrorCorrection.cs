using System;
using System.Linq;
using QR.Enums;
using QR.Scriptable;
using UnityEngine;

namespace QR
{
    public class ErrorCorrection
    {
        private readonly Texture2D _texture;
        private readonly DataAnalyzer _analyzer;
        private readonly int _ecDataSize;
        private readonly string _data;
        private const int ERROR_CORRECTION_LENGTH = 8;
        
        
        public ErrorCorrection(ref Texture2D texture,ref DataAnalyzer analyzer, ref VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionType, string data)
        {
            _texture = texture;    
            _analyzer = analyzer;
            _data = data;

            _ecDataSize = versionData.CharacterSizeTable[new QRType(encodingType, errorCorrectionType)]
                .ErrorCorrectionData;
        }

        public void SetErrorCorrectionData()
        {
            // Ver1 Byte High EC 17 bytes
            ReedSolomonGenerator generator = new ReedSolomonGenerator();
            
            // byte[] dataArray = _data.ToCharArray().Select(x => (byte)x).ToArray();
            byte[] dataArray =
            {
                0b01000000,
                0b01000101,
                0b01100110,
                0b01010111,
                0b00100011,
                0b00010000,
                0b11101100,
                0b00010001,
                0b11101100
            };
            
            var ecblocks = generator.GenerateECBlocks(dataArray, _ecDataSize);
            
            for (int i = 0; i < _ecDataSize; i++) //17 characters 
            for (int j = ERROR_CORRECTION_LENGTH - 1; j >= 0; j--) //8 bits for each character
            {   
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((ecblocks[i] >> j) & 1) == 1 ? Color.black : Color.white);
                Debug.Log(((ecblocks[i] >> j) & 1) == 1 ? "1" : "0");
            }
        }
        
        
    }
}