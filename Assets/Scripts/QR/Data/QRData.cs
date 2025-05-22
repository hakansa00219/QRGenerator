using System.Collections.Generic;
using System.Linq;
using QR.Analysis;
using QR.Enums;
using QR.Scriptable;
using QR.Structs;
using QR.Utilities;
using UnityEngine;

namespace QR
{
    public class QRData
    {
        private readonly Texture2D _texture;
        private readonly string _data;
        private readonly IBitProvider _bitProvider;
        private readonly VersionData _versionData;
        private readonly EncodingType _encodingType;
        private readonly ErrorCorrectionLevel _errorCorrectionLevel;
        private const byte FirstPadding = 0xEC;
        private const byte SecondPadding = 0x11;
        public QRData(ref Texture2D texture, ref IBitProvider bitProvider, VersionData versionData, EncodingType encodingType, ErrorCorrectionLevel errorCorrectionLevel,  string data)
        {
            _texture = texture;
            _bitProvider = bitProvider;
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
            int dataSize = _data.Length;
            int charBitSize = QRUtility.GetCharacterBitSize(_encodingType);
            // convert string data to byte array.
            byte[] convertedData = _data.ToCharArray().Select(c => (byte)c).ToArray();
            Debug.Log("Data: " + _data);

            // Color the QR depending on the actual data byte array.
            for (int i = 0; i < dataSize; i++) //4 characters
            for (int j = charBitSize - 1; j >= 0; j--) //8 bits for each character
            {   
                var bitNode = _bitProvider.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, convertedData[i], j);
            }

            combinedDataList.AddRange(convertedData);
            
            // DATA END
            byte endData = 0b0000;
            int endDataSize = 4;

            // Color the QR depending on the end data(0000) byte array.
            for (int i = endDataSize - 1; i >= 0; i--)
            {
                var bitNode = _bitProvider.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, endData, i);
            }
            
            combinedDataList.Add(endData);
            
            //PADDING IF NEED
            // Get the maximum main data size depending on QR type.
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
                byte selectedPadding = i % 2 == 0 ? FirstPadding : SecondPadding;
                combinedDataList.Add(selectedPadding);
                // Foreach leftoverData color the QR.
                for (int j = paddingDataSize - 1; j >= 0; j--)
                {
                    var bitNode = _bitProvider.BitQueue.Dequeue();
                    _texture.SetPixel2D(bitNode.X, bitNode.Y, selectedPadding, j);
                }
            }
            
            combinedData = combinedDataList.ToArray();
        }
    }
}