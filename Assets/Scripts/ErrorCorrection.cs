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
            var poly = generator.Generate(_ecDataSize);

            int[] data = _data.ToCharArray().Select(c => (int)c).ToArray();

            int[] errorCorrectionData = generator.ComputeErrorCorrectionCodewords(data, poly, _ecDataSize);
            
            for (int i = 0; i < _ecDataSize; i++) //17 characters
            for (int j = ERROR_CORRECTION_LENGTH - 1; j >= 0; j--) //8 bits for each character
            {   
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((errorCorrectionData[i] >> j) & 1) == 1 ? Color.black : Color.white);
                if(i == 16 && j == 0)
                    Debug.Log($"({bitNode.X}, {bitNode.Y})");
            }
        }
        
        
    }
}