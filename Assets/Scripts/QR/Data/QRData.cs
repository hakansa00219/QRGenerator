using System.Collections.Generic;
using System.Linq;
using QR.Encoding;
using QR.Enums;
using QR.Scriptable;
using QR.Structs;
using QR.Utilities;
using UnityEngine;

namespace QR
{
    public class QRData
    {
        private readonly string _data;
        private readonly ITextureRenderer _textureRenderer;
        private readonly VersionData _versionData;
        private readonly EncodingType _encodingType;
        private readonly ErrorCorrectionLevel _errorCorrectionLevel;
        
        private const byte FirstPadding = 0xEC;
        private const byte SecondPadding = 0x11;
        private const int PaddingDataSize = 8;
        
        public QRData(ITextureRenderer textureRenderer, VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionLevel,  string data)
        {
            _textureRenderer = textureRenderer;
            _data = data;
            _versionData = versionData;
            _encodingType = encodingType;
            _errorCorrectionLevel = errorCorrectionLevel;
        }

        // !!! When data is over(Ver1) add 4 bit END block. Then add paddings to complete the data capacity.(so 3 more padding)
        public void SetData(out byte[] combinedData)
        {
            combinedData = null;
            List<byte> combinedDataList = new List<byte>();
            
            // THE DATA
            RenderMainData(out var dataSize, out var convertedData);
            combinedDataList.AddRange(convertedData);
            
            // DATA END
            RenderEndData(out var endData);
            combinedDataList.Add(endData);
            
            //PADDING IF NEED
            // Get the maximum main data size depending on QR type.
            int qrMainDataSize = _versionData.CharacterSizeTable[new QRType(_encodingType, _errorCorrectionLevel)]
                .MaxMainData;
            int leftOverDataSize = qrMainDataSize - dataSize;
            
            switch (leftOverDataSize)
            {
                case 0:
                    combinedData = combinedDataList.ToArray();
                    return;
                case < 0:
                    Debug.LogError("Miss calculation");
                    return;
            }
            
            for (int i = 0; i < leftOverDataSize; i++)
            {
                byte selectedPadding = i % 2 == 0 ? FirstPadding : SecondPadding;
                combinedDataList.Add(selectedPadding);
                // Foreach leftoverData color the QR.
                _textureRenderer.RenderDataToTexture(selectedPadding, PaddingDataSize);
            }
            
            combinedData = combinedDataList.ToArray();
        }

        private void RenderEndData(out byte endData)
        {
            endData = 0b0000;
            int endDataSize = 4;
            
            _textureRenderer.RenderDataToTexture(endData, endDataSize);
        }

        private void RenderMainData(out int dataSize, out byte[] convertedData)
        {
            dataSize = _data.Length;
            int charBitSize = QRUtility.GetCharacterBitSize(_encodingType);
            // convert string data to byte array. TODO: only works for byte (charBitSize == 8)!!
            convertedData = _data.ToCharArray().Select(c => (byte)c).ToArray();
            Debug.Log("Data: " + _data);
            
            //TODO: Whats going to happen if data is not byte. Need to check this.
            _textureRenderer.RenderDataToTexture(convertedData);
        }
    }
}