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
        private const int EndDataSize = 4;
        
        public QRData(ITextureRenderer textureRenderer, VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionLevel,  string data)
        {
            _textureRenderer = textureRenderer;
            _data = data;
            _versionData = versionData;
            _encodingType = encodingType;
            _errorCorrectionLevel = errorCorrectionLevel;
            
        }

        // !!! When data is over(Ver1) add 4 bit END block. Then add paddings to complete the data capacity.(so 3 more padding)
        public void SetData(ref OrganizedData organizedData)
        {
            // THE DATA
            RenderMainData(ref organizedData);
            
            int errorCorrectionDataSize = _versionData.ErrorCorrectionDataSizeTable[_errorCorrectionLevel];
            int ecBitCount = errorCorrectionDataSize * 8;
            int leftOverBitCount = _textureRenderer.RemainingBitCount - ecBitCount;

            if (leftOverBitCount <= 4)
            {
                // DATA END
                RenderEndData(ref organizedData, leftOverBitCount);
            }
            else
            {
                // Data End
                RenderEndData(ref organizedData, EndDataSize);
                
                int remainingSize = leftOverBitCount - EndDataSize;
                int paddingSize = remainingSize / 8;
                int byteAlignmentSize = remainingSize % 8;
                
                // Byte Alignment
                RenderByteAlignment(ref organizedData, byteAlignmentSize);
                
                // Paddings
                if (paddingSize <= 0) return;
                
                byte[] paddings = new byte[paddingSize];
                for (int i = 0; i < paddingSize; i++)
                {
                    byte selectedPadding = i % 2 == 0 ? FirstPadding : SecondPadding;
                    _textureRenderer.RenderingDataToTexture(selectedPadding, PaddingDataSize);
                    paddings[i] = selectedPadding;
                }
                organizedData.Padding = (paddings, PaddingDataSize);
            }
        }

        private void RenderByteAlignment(ref OrganizedData organizedData, int size)
        {
            byte alignmentData = 0b00000000;
            _textureRenderer.RenderingDataToTexture(alignmentData, size);
            organizedData.ByteAlignment = (alignmentData, size);
        }

        private void RenderEndData(ref OrganizedData organizedData, int size)
        {
            byte endData = 0b0000;
            _textureRenderer.RenderingDataToTexture(endData, size);
            organizedData.End = (endData, size);
        }

        private void RenderMainData(ref OrganizedData organizedData)
        {
            switch (_encodingType)
            {
                case EncodingType.Alphanumeric:
                    const int alphanumericSize = 45;
                    int pairedCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 2);
                    int soloCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 1);
                    int dataSize = _data.Length;
                    int pairCount = dataSize / 2;
                    int remainder = dataSize % 2;
                    bool isRemainderExists = remainder > 0;
                    
                    int[] mainData = new int[pairCount];
                    int[] subData = new int[remainder];
                    
                    char[] charData = _data.ToCharArray();
                    
                    for (int i = 0; i < pairCount; i++)
                    {
                        mainData[i] = Alphanumeric.Dictionary[charData[2 * i]] * alphanumericSize + Alphanumeric.Dictionary[charData[2 * i + 1]];
                    }
                    _textureRenderer.RenderingDataToTexture(mainData, pairedCharBitSize);
                    organizedData.Main = (mainData, pairedCharBitSize);

                    if (isRemainderExists)
                    {
                        int remainderData = Alphanumeric.Dictionary[charData[^1]];
                        subData[0] = remainderData;
                        _textureRenderer.RenderingDataToTexture(subData, soloCharBitSize);
                        organizedData.Remaining = (subData, soloCharBitSize);
                    }
                    break;
                case EncodingType.Byte:
                    int charBitSize = QRUtility.GetCharacterBitSize(_encodingType);
                    int[] combinedData = _data.ToCharArray().Select(c => (int)c).ToArray();
                    _textureRenderer.RenderingDataToTexture(combinedData, charBitSize);
                    organizedData.Main = (combinedData, charBitSize);
                    break;
                case EncodingType.Kanji:
                    break;
                case EncodingType.Numeric:
                    break;
            }
            
            Debug.Log("Data: " + string.Join(", ", organizedData.Main.data) + (organizedData.Remaining.data != null ? string.Join(", ", organizedData.Remaining.data) : ""));
        }
    }
}