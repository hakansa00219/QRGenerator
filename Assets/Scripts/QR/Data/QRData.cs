using System;
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
        public void SetData(out int[] combinedData)
        {
            combinedData = null;
            List<int> combinedDataList = new List<int>();
            
            // THE DATA
            RenderMainData(out var convertedData);
            combinedDataList.AddRange(convertedData);
            
            
            int errorCorrectionDataSize = _versionData.ErrorCorrectionDataSizeTable[_errorCorrectionLevel];
            int ecBitCount = errorCorrectionDataSize * 8;
            int leftOverBitCount = _textureRenderer.RemainingBitCount - ecBitCount;

            if (leftOverBitCount <= 4)
            {
                // 0 1 2 3 4  Add end (0) data depending the empty slot
                // DATA END
                RenderEndData(out var endData, leftOverBitCount);
                combinedDataList.Add(endData);
                
            }
            else
            {
                int paddingSize = leftOverBitCount / 8;
                int paddingRemainSize = leftOverBitCount % 8;
                
                // DATA END
                RenderEndData(out var endData, paddingRemainSize);
                combinedDataList.Add(endData);
                
                // paddings
                for (int i = 0; i < paddingSize; i++)
                {
                    byte selectedPadding = i % 2 == 0 ? FirstPadding : SecondPadding;
                    combinedDataList.Add(selectedPadding);
                    // Foreach leftoverData color the QR.
                    _textureRenderer.RenderingDataToTexture(selectedPadding, PaddingDataSize);
                }
            }
            
            combinedData = combinedDataList.ToArray();
        }

        private void RenderEndData(out byte endData, int size)
        {
            //TODO: Only if we have 4 empty slot 0000 is right. The empty slot can be 1-2-3-4 or none.
            endData = 0b0000;
            _textureRenderer.RenderingDataToTexture(endData, size);
        }

        private void RenderMainData(out int[] convertedData)
        {
            // convert string data to byte array. TODO: only works for byte (charBitSize == 8)!!
            convertedData = null;
            switch (_encodingType)
            {
                case EncodingType.Alphanumeric:
                    const int alphanumericSize = 45;
                    int pairedCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 2);
                    int soloCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 1);
                    int dataSize = _data.Length;
                    int pairCount = dataSize / 2;
                    int remainder = dataSize % 2;
                    
                    char[] charData = _data.ToCharArray();
                    convertedData = new int[pairCount];
                    int remainderData = 0;
                    for (int i = 0; i < pairCount; i++)
                    {
                        convertedData[i] = Alphanumeric.Dictionary[charData[2 * i]] * alphanumericSize + Alphanumeric.Dictionary[charData[2 * i + 1]];
                    }
                    _textureRenderer.RenderingDataToTexture(convertedData, pairedCharBitSize);
                    
                    if (remainder > 0) remainderData = Alphanumeric.Dictionary[charData[^1]]; 
                    _textureRenderer.RenderingDataToTexture(remainderData, soloCharBitSize);
                    
                    Debug.Log("Data: " + string.Join(", ", convertedData) + (remainder > 0 ? $", {remainderData}" : string.Empty));
                    
                    break;
                case EncodingType.Byte:
                    int charBitSize = QRUtility.GetCharacterBitSize(_encodingType);
                    convertedData = _data.ToCharArray().Select(c => (int)c).ToArray();
                    _textureRenderer.RenderingDataToTexture(convertedData, charBitSize);

                    Debug.Log("Data: " + string.Join(", ", convertedData));
                    break;
                case EncodingType.Kanji:
                    break;
                case EncodingType.Numeric:
                    break;
            }

            if (convertedData == null) throw new Exception();
            
        }
    }
}