using System;
using System.Collections.Generic;
using System.Text;
using QR.Algorithms;
using QR.Encoding;
using QR.Enums;
using QR.Logger;
using QR.Scriptable;
using QR.Structs;

namespace QR
{
    public class ErrorCorrection
    {
        private readonly ITextureRenderer _textureRenderer;
        private readonly ILogger _logger;
        private readonly int _ecDataSize;
        
        public ErrorCorrection(ITextureRenderer textureRenderer, ILogger logger, VersionData versionData, ErrorCorrectionLevel errorCorrectionType)
        {
            _textureRenderer = textureRenderer;
            _logger = logger;
            _ecDataSize = versionData.ErrorCorrectionDataSizeTable[errorCorrectionType];
        }

        public void SetErrorCorrectionData(in OrganizedData organizedData)
        {
            ReedSolomonGenerator generator = new ReedSolomonGenerator();
            
            // Bit manipulation since each EC codeword 8 bits and some data in QR not 8 bits.
            StringBuilder bitString = new StringBuilder();
            
            var encoding = organizedData.Encoding;
            var length = organizedData.Length;
            var main = organizedData.Main;
            var remaining = organizedData.Remaining;
            var end = organizedData.End;
            var byteAlignment = organizedData.ByteAlignment;
            var padding = organizedData.Padding;
            
            switch ((EncodingType)encoding.data)
            {
                case EncodingType.Alphanumeric:
                    int pairCount = main.data.Length;
                    int remainderCount = remaining.data?.Length ?? 0;
                    bool isRemainderExists = remainderCount > 0;
                    
                    // Encoding Type
                    bitString.Append(ToBinaryString(encoding.data, encoding.bitCount));
                    // Data Length 
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
                    if (end.bitCount > 0)
                        bitString.Append(ToBinaryString(end.data, end.bitCount));
                    // Byte Alignment
                    if (byteAlignment.bitCount > 0)
                        bitString.Append(ToBinaryString(byteAlignment.data, byteAlignment.bitCount));
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
                    int groupCount = main.data.Length;
                    int restCount = remaining.data?.Length ?? 0;
                    bool isRestExists = restCount > 0;
                    
                    // Encoding Type
                    bitString.Append(ToBinaryString(encoding.data, encoding.bitCount));
                    // Data Length 
                    bitString.Append(ToBinaryString(length.data, length.bitCount)); 
                    // Numeric Pairs
                    for (var i = 0; i < groupCount; i++)
                    {
                        bitString.Append(ToBinaryString(main.data[i], main.bitCount));
                    }
                    // Single/Duo Numeric if exists
                    if (isRestExists)
                        bitString.Append(ToBinaryString(remaining.data[0], remaining.bitCount));
                    // End Data (0000)
                    if (end.bitCount > 0)
                        bitString.Append(ToBinaryString(end.data, end.bitCount));
                    // Byte Alignment
                    if (byteAlignment.bitCount > 0)
                        bitString.Append(ToBinaryString(byteAlignment.data, byteAlignment.bitCount));
                    // Paddings
                    if (padding.data is { Length: > 0 })
                    {
                        foreach (var value in padding.data)
                        {
                            bitString.Append(ToBinaryString(value, padding.bitCount));
                        }
                    }
                    break;
                case EncodingType.Kanji:
                    int kanjiSize = main.data.Length;
                    // Encoding
                    bitString.Append(ToBinaryString(encoding.data, encoding.bitCount));
                    // Data Length
                    bitString.Append(ToBinaryString(length.data, length.bitCount));
                    // Main Data
                    for (int i = 0; i < kanjiSize; i++)
                    {
                        bitString.Append(ToBinaryString(main.data[i], main.bitCount));
                    }
                    // End
                    bitString.Append(ToBinaryString(end.data, end.bitCount));
                    // Byte Alignment
                    if (byteAlignment.bitCount > 0)
                        bitString.Append(ToBinaryString(byteAlignment.data, byteAlignment.bitCount));
                    // Padding
                    if (padding.data is { Length: > 0 })
                    {
                        foreach (var value in padding.data)
                        {
                            bitString.Append(ToBinaryString(value, padding.bitCount));
                        }
                    }
                    break;
            }
            
            List<byte> convertedData = new List<byte>();
            for (int i = 0; i < bitString.Length; i += 8)
            {
                string byteStr = bitString.ToString().Substring(i, 8);
                byte value = Convert.ToByte(byteStr, 2);
                convertedData.Add(value);
            }
            
            byte[] ecBlocks = generator.GenerateErrorCorrectionBlocks(convertedData.ToArray() , _ecDataSize, _logger);
            
            _textureRenderer.RenderingDataToTexture(ecBlocks);
        }

        private string ToBinaryString(int value, int bitLength)
        {
            return Convert.ToString(value, 2).PadLeft(bitLength, '0');
        }
    }
}