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
        public void SetData()
        {
            // THE DATA
            int charSize = _data.Length;
            int dataSize = QRUtility.GetCharacterBitSize(_encodingType);

            byte[] convertedData = _data.ToCharArray().Select(c => (byte)c).ToArray();

            for (int i = 0; i < charSize; i++) //4 characters
            for (int j = dataSize - 1; j >= 0; j--) //8 bits for each character
            {   
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((convertedData[i] >> j) & 1) == 1 ? Color.black : Color.white);
            }
            // DATA END
            byte endData = 0b0000;
            int endDataSize = 4;

            for (int i = endDataSize - 1; i >= 0; i--)
            {
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((endData >> i) & 1) == 1 ? Color.black : Color.white);
            }
            
            //PADDING IF NEED
            int mainDataSize = _versionData.CharacterSizeTable[new QRType(_encodingType, _errorCorrectionLevel)]
                .MaxMainData;
            int leftOverDataSize = mainDataSize - charSize;
            
            switch (leftOverDataSize)
            {
                case 0:
                    return;
                case < 0:
                    Debug.LogError("Miss calculation");
                    return;
            }
            
            int paddingDataSize = 8;
            for (int i = 0; i < leftOverDataSize; i++)
            {
                for (int j = paddingDataSize - 1; j >= 0; j--)
                {
                    var bitNode = _analyzer.BitQueue.Dequeue();
                    _texture.SetPixel2D(bitNode.X, bitNode.Y,
                        (((i % 2 == 0 ? firstPadding : secondPadding) >> j) & 1) == 1 ? Color.black : Color.white);
                }
            }
        }
    }
}