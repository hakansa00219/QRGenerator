using System.Collections.Generic;
using System.Linq;
using QR.Analysis;
using QR.Enums;
using QR.Scriptable;
using QR.Utilities;
using UnityEngine;

namespace QR
{
    public class QRData
    {
        private readonly Texture2D _texture;
        private readonly string _data;
        private readonly DataAnalyzer _analyzer;
        private readonly VersionData _versionData;
        private readonly EncodingType _encodingType;
        private readonly ErrorCorrectionLevel _errorCorrectionLevel;
        private const byte firstPadding = 0xEC;
        private const byte secondPadding = 0x11;
        public QRData(ref Texture2D texture, ref DataAnalyzer analyzer, VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionLevel,  string data)
        {
            _texture = texture;
            _analyzer = analyzer;
            _data = data;
            _versionData = versionData;
            _encodingType = encodingType;
            _errorCorrectionLevel = errorCorrectionLevel;
        }

        // !!! When data is over(Ver1) add 4 bit END block. Then add paddings to complete the data capacity.(so 3 more padding)
        // Rest is error correction.
        public void SetData(out byte[] combinedData)
        {
            combinedData = null;
            List<byte> combinedDataList = new List<byte>();
            
            // THE DATA
            int dataSize = _data.Length;
            int charBitSize = QRUtility.GetCharacterBitSize(_encodingType);

            byte[] convertedData = _data.ToCharArray().Select(c => (byte)c).ToArray();
            Debug.Log("Data: " + _data);

            for (int i = 0; i < dataSize; i++) //4 characters
            for (int j = charBitSize - 1; j >= 0; j--) //8 bits for each character
            {   
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((convertedData[i] >> j) & 1) == 1 ? Color.black : Color.white);
            }

            combinedDataList.AddRange(convertedData);
            
            // DATA END
            byte endData = 0b0000;
            int endDataSize = 4;

            for (int i = endDataSize - 1; i >= 0; i--)
            {
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((endData >> i) & 1) == 1 ? Color.black : Color.white);
            }
            
            combinedDataList.Add(endData);
            
            //PADDING IF NEED
            int mainDataSize = _versionData.CharacterSizeTable[new QRType(_encodingType, _errorCorrectionLevel)]
                .MaxMainData;
            int leftOverDataSize = mainDataSize - dataSize;
            
            switch (leftOverDataSize)
            {
                case 0:
                    combinedData = combinedDataList.ToArray();
                    return;
                case < 0:
                    Debug.LogError("Miss calculation");
                    return;
            }
            
            int paddingDataSize = 8;
            for (int i = 0; i < leftOverDataSize; i++)
            {
                byte selectedPadding = i % 2 == 0 ? firstPadding : secondPadding;
                combinedDataList.Add(selectedPadding);
                
                for (int j = paddingDataSize - 1; j >= 0; j--)
                {
                    var bitNode = _analyzer.BitQueue.Dequeue();
                    _texture.SetPixel2D(bitNode.X, bitNode.Y,
                        ((selectedPadding >> j) & 1) == 1 ? Color.black : Color.white);
                }
            }
            
            combinedData = combinedDataList.ToArray();
        }
    }
}