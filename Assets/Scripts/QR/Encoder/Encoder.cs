using System;
using System.Linq;
using System.Text.RegularExpressions;
using QR.Analysis;
using QR.Utilities;
using QR.Enums;
using QR.Scriptable;
using UnityEngine;

namespace QR
{
    public class Encoder
    {
        private const string AlphaNumericPattern = @"^[0-9A-Z $%*+-./:]+$";

        private readonly byte _charSize = 0;
        private readonly EncodingType _encodingType;
        private ErrorCorrectionLevel _errorCorrectionLevel;
        private readonly Texture2D _texture;
        private readonly VersionData _versionData;
        private readonly DataAnalyzer _analyzer;
        
        public Encoder(ref Texture2D qrTexture, ref DataAnalyzer analyzer, VersionData versionData, ErrorCorrectionLevel errorCorrectionLevel, string data, byte charSize)
        {
            _analyzer = analyzer;
            _charSize = charSize;
            _texture = qrTexture;
            _versionData = versionData;
            _errorCorrectionLevel = errorCorrectionLevel;

            // Check the compatibility of data to find the encoding type. TODO: later fix this rn only byte exist
            //
            if (!CheckCompatibility(errorCorrectionLevel, data, out _encodingType))
            {
                Debug.LogError("Not compatible encoder format. Probably you need higher version QR code.");
                //TODO: Automatically select higher level version if selected Auto to version
            }
        }
        public EncodingType SetEncoding(out ErrorCorrectionLevel errorCorrectionLevel)
        {
            // Write data to QR code depending on encoding type.
            int dataSize = VersionUtility.GetEncodingModeBitCount();
            byte encodingType = (byte)_encodingType;
            for (var i = dataSize - 1; i >= 0; i--)
            {
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((encodingType >> i) & 1) == 1 ? Color.black : Color.white);
            }
            errorCorrectionLevel = _errorCorrectionLevel;
            return _encodingType;
        }

        private bool CheckCompatibility(ErrorCorrectionLevel errorCorrectionLevel, string data, out EncodingType encodingType)
        {
            _errorCorrectionLevel = errorCorrectionLevel;
            if (IsNumericCompatible(data, errorCorrectionLevel))
            {
                encodingType = EncodingType.Numeric;
                return true;
            }
            if (IsAlphanumericCompatible(data, errorCorrectionLevel))
            {
                encodingType = EncodingType.Alphanumeric;
                return true;
            }
            if (IsByteCompatible(data, errorCorrectionLevel))
            {
                encodingType = EncodingType.Byte;
                return true;
            }
            if (IsKanjiCompatible(data, errorCorrectionLevel))
            {
                encodingType = EncodingType.Kanji;
                return true;
            }
            if (errorCorrectionLevel != ErrorCorrectionLevel.Low)
            {
                return CheckCompatibility(errorCorrectionLevel switch
                {
                    ErrorCorrectionLevel.High => ErrorCorrectionLevel.Quality,
                    ErrorCorrectionLevel.Quality => ErrorCorrectionLevel.Medium,
                    ErrorCorrectionLevel.Medium  => ErrorCorrectionLevel.Low
                }, data, out encodingType);
            }

            encodingType = EncodingType.Byte;
            return false;
        }
        
        private bool IsNumericCompatible(string data, ErrorCorrectionLevel errorCorrectionLevel)
        {
            return data.All(char.IsDigit) && _charSize <= _versionData.CharacterSizeTable[new QRType(EncodingType.Numeric, errorCorrectionLevel)].MaxMainData;
        }
        private bool IsAlphanumericCompatible(string data, ErrorCorrectionLevel errorCorrectionLevel)
        {
            var match = Regex.IsMatch(data, AlphaNumericPattern);
            return match && _charSize <= _versionData.CharacterSizeTable[new QRType(EncodingType.Alphanumeric, errorCorrectionLevel)].MaxMainData;
        }
        private bool IsKanjiCompatible(string data, ErrorCorrectionLevel errorCorrectionLevel)
        {
            return data.Any(x => x >= 0x4E00 && x <= 0x9FBF) && _charSize <= _versionData.CharacterSizeTable[new QRType(EncodingType.Kanji, errorCorrectionLevel)].MaxMainData;
        }
        private bool IsByteCompatible(string data, ErrorCorrectionLevel errorCorrectionLevel)
        {
            return !data.Any(x => x >= 0x4E00 && x <= 0x9FBF) && _charSize <= _versionData.CharacterSizeTable[new QRType(EncodingType.Byte, errorCorrectionLevel)].MaxMainData;
        }

        
    }
}