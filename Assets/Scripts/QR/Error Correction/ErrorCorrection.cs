using System.Collections.Generic;
using System.Linq;
using QR.Algorithms;
using QR.Encoding;
using QR.Enums;
using QR.Scriptable;
using QR.Structs;

namespace QR
{
    public class ErrorCorrection
    {
        private readonly ITextureRenderer _textureRenderer;
        private readonly int _ecDataSize;
        private readonly byte[] _data;
        private readonly EncodingType _encodingType;
        private readonly int _dataLength;
        
        
        public ErrorCorrection(ITextureRenderer textureRenderer, VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionType, byte[] data, int dataLength)
        {
            _data = data;
            _textureRenderer = textureRenderer;
            _encodingType = encodingType;
            _dataLength = dataLength;
            _ecDataSize = versionData.CharacterSizeTable[new QRType(encodingType, errorCorrectionType)]
                .ErrorCorrectionData;
        }

        public void SetErrorCorrectionData()
        {
            ReedSolomonGenerator generator = new ReedSolomonGenerator();

            List<byte> dataList = new List<byte>();
            
            // Bit manipulation since each EC codeword 8 bits and some data in QR not 8 bits.
            // TODO: this might only works for byte! Need to check
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
            
            byte[] ecBlocks = generator.GenerateErrorCorrectionBlocks(dataList.ToArray() , _ecDataSize);
            
            _textureRenderer.RenderingDataToTexture(ecBlocks);
        }
        
        
    }
}