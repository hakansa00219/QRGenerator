using System;
using System.Collections.Generic;
using System.Text;
using QR.Algorithms;
using QR.Encoding;
using QR.Enums;
using QR.Scriptable;
using QR.Structs;
using QR.Utilities;
using Sirenix.Utilities.Editor;
using Version = QR.Enums.Version;

namespace QR
{
    public class ErrorCorrection
    {
        private readonly ITextureRenderer _textureRenderer;
        private readonly int _ecDataSize;
        
        public ErrorCorrection(ITextureRenderer textureRenderer, VersionData versionData, ErrorCorrectionLevel errorCorrectionType)
        {
            _textureRenderer = textureRenderer;
            _ecDataSize = versionData.ErrorCorrectionDataSizeTable[errorCorrectionType];
        }

        public void SetErrorCorrectionData(in OrganizedData organizedData)
        {
            ReedSolomonGenerator generator = new ReedSolomonGenerator();

            // List<byte> dataList = new List<byte>();
            
            // Bit manipulation since each EC codeword 8 bits and some data in QR not 8 bits.
            // TODO: this might only works for byte! Need to check
            StringBuilder bitString = new StringBuilder();
            
            var encoding = organizedData.Encoding;
            var length = organizedData.Length;
            var main = organizedData.Main;
            var remaining = organizedData.Remaining;
            var end = organizedData.End;
            var padding = organizedData.Padding;
            
            switch ((EncodingType)encoding.data)
            {
                case EncodingType.Alphanumeric:
                    int pairCount = main.data.Length;
                    int remainderCount = remaining.data.Length;
                    bool isRemainderExists = remainderCount > 0;
                    
                    // Encoding Type
                    bitString.Append(ToBinaryString(encoding.data, encoding.bitCount));
                    // Data Length TODO: Only works with version 1
                    bitString.Append(ToBinaryString(length.data, length.bitCount)); 
                    // Alphanumeric Pairs
                    for (var i = 0; i < pairCount; i++)
                    {
                        bitString.Append(ToBinaryString(main.data[i], main.bitCount));
                    }
                    // Single Alphanumeric if exists
                    if (isRemainderExists)
                        bitString.Append(ToBinaryString(remaining.data[0], remaining.bitCount));
                    // End Data (0000)
                    bitString.Append(ToBinaryString(end.data, end.bitCount));
                    // Paddings
                    if (padding.data is { Length: > 0 })
                    {
                        foreach (var value in padding.data)
                        {
                            bitString.Append(ToBinaryString(value, padding.bitCount));
                        }
                    }
                    break;
                case EncodingType.Byte:
                    int dataSize = main.data.Length;
                    // Encoding
                    bitString.Append(ToBinaryString(encoding.data, encoding.bitCount));
                    // Data Length
                    bitString.Append(ToBinaryString(length.data, length.bitCount));
                    // Main Data
                    for (int i = 0; i < dataSize; i++)
                    {
                        bitString.Append(ToBinaryString(main.data[i], main.bitCount));
                    }
                    // End
                    bitString.Append(ToBinaryString(end.data, end.bitCount));
                    // Padding
                    if (padding.data is { Length: > 0 })
                    {
                        foreach (var value in padding.data)
                        {
                            bitString.Append(ToBinaryString(value, padding.bitCount));
                        }
                    }
                    break;
                case EncodingType.Numeric:
                    break;
                case EncodingType.Kanji:
                    break;
            }
            
            List<byte> convertedData = new List<byte>();
            for (int i = 0; i < bitString.Length; i += 8)
            {
                string byteStr = bitString.ToString().Substring(i, 8);
                byte value = Convert.ToByte(byteStr, 2);
                convertedData.Add(value);
            }
            
            byte[] ecBlocks = generator.GenerateErrorCorrectionBlocks(convertedData.ToArray() , _ecDataSize);
            
            _textureRenderer.RenderingDataToTexture(ecBlocks);
        }

        private string ToBinaryString(int value, int bitLength)
        {
            return Convert.ToString(value, 2).PadLeft(bitLength, '0');
        }
    }
}