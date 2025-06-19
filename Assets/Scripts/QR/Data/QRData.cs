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
            int dataSize = _data.Length;
            char[] charData = _data.ToCharArray();
            
            switch (_encodingType)
            {
                case EncodingType.Alphanumeric:
                    int pairedAlphaCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 2);
                    int soloAlphaCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 1);
                    int pairCount = dataSize / 2;
                    int remainder = dataSize % 2;
                    bool isRemainderExists = remainder > 0;
                    
                    int[] mainData = new int[pairCount];
                    
                    for (int i = 0; i < pairCount; i++)
                    {
                        mainData[i] = Alphanumeric.Dictionary[charData[2 * i]] * Alphanumeric.Size + Alphanumeric.Dictionary[charData[2 * i + 1]];
                    }
                    _textureRenderer.RenderingDataToTexture(mainData, pairedAlphaCharBitSize);
                    organizedData.Main = (mainData, pairedAlphaCharBitSize);

                    if (isRemainderExists)
                    {
                        int[] subData = new int[1];
                        
                        int remainderData = Alphanumeric.Dictionary[charData[^1]];
                        subData[0] = remainderData;
                        _textureRenderer.RenderingDataToTexture(subData, soloAlphaCharBitSize);
                        organizedData.Remaining = (subData, soloAlphaCharBitSize);
                    }
                    break;
                case EncodingType.Byte:
                    int charBitSize = QRUtility.GetCharacterBitSize(_encodingType);
                    byte[] combinedBytes = System.Text.Encoding.UTF8.GetBytes(_data);
                    _textureRenderer.RenderingDataToTexture(combinedBytes);
                    organizedData.Main = (combinedBytes.Select(c => (int)c).ToArray(), charBitSize);
                    break;
                case EncodingType.Kanji:
                    int kanjiCharBitSize = QRUtility.GetCharacterBitSize(_encodingType); // 13
                    int[] optimizedList = new int[dataSize];
                    var kanjiEnc = System.Text.Encoding.GetEncoding(932);
                    
                    for (var i = 0; i < _data.Length; i++)
                    {
                        var c = _data[i];
                        byte[] bytes = kanjiEnc.GetBytes(new[] { c });
                        int key = (bytes[0] << 8) | bytes[1];

                        if (!Kanji.Dictionary.TryGetValue(key, out int value))
                        {
                            Debug.LogError("Cannot find the kanji character in Kanji Lookup table.");
                            optimizedList[i] = -1;
                        }

                        optimizedList[i] = value;
                    }
                    
                    _textureRenderer.RenderingDataToTexture(optimizedList, kanjiCharBitSize);
                    organizedData.Main = (optimizedList, kanjiCharBitSize);
                    
                    break;
                case EncodingType.Numeric:
                    int soloNumericCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 1);
                    int duoNumericCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 2);
                    int trioNumericCharBitSize = QRUtility.GetCharacterBitSize(_encodingType, 3);
                    // Data can be = 293-283-12 or 948-235-1 or 485-381-238
                    // 3 char groups
                    int groupCount = dataSize / 3;
                    int restCount = dataSize % 3;
                    bool isRestExists = restCount > 0;
                    
                    int[] mainGroupData = new int[groupCount];

                    //Main data
                    for (int i = 0; i < groupCount; i++)
                    {
                        mainGroupData[i] = Numeric.Dictionary[charData[3 * i]]     * (int)Math.Pow(Numeric.Size, 2) +
                                           Numeric.Dictionary[charData[3 * i + 1]] * Numeric.Size +
                                           Numeric.Dictionary[charData[3 * i + 2]];
                    }
                    _textureRenderer.RenderingDataToTexture(mainGroupData, trioNumericCharBitSize);
                    organizedData.Main = (mainGroupData, trioNumericCharBitSize);
                    
                    if (isRestExists)
                    {
                        int[] subGroupData = new int[1];

                        switch (restCount)
                        {
                            case 1: 
                                //Data : 9 - 1001
                                subGroupData[0] = Numeric.Dictionary[charData[^1]];
                                _textureRenderer.RenderingDataToTexture(subGroupData, soloNumericCharBitSize);
                                organizedData.Remaining = (subGroupData, soloNumericCharBitSize);
                                break;
                            case 2:
                                //Data : 99 - 1100011
                                subGroupData[0] = Numeric.Dictionary[charData[^2]] * Numeric.Size +
                                                  Numeric.Dictionary[charData[^1]];
                                _textureRenderer.RenderingDataToTexture(subGroupData, duoNumericCharBitSize);
                                organizedData.Remaining = (subGroupData, duoNumericCharBitSize);
                                break;
                        }
                    }
                    
                    break;
            }
            
            Debug.Log("Data: " + string.Join(", ", organizedData.Main.data) + (organizedData.Remaining.data != null ? string.Join(", ", organizedData.Remaining.data) : ""));
        }
    }
}