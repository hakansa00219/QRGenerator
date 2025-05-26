using System;
using System.Linq;
using System.Text.RegularExpressions;
using QR.Utilities;
using QR.Enums;
using QR.Scriptable;
using QR.Structs;
using UnityEngine;

namespace QR.Encoding
{
    public class EncodingSelector : IEncodingSelection
    {
        private const string AlphaNumericPattern = @"^[0-9A-Z $%*+-./:]+$";

        private readonly byte _charSize = 0;
        private readonly VersionData _versionData;
        private readonly ITextureRenderer _textureRenderer;
        
        public EncodingType SelectedEncodingType { get; private set; }
        public ErrorCorrectionLevel SelectedErrorCorrectionLevel { get; private set; } = ErrorCorrectionLevel.High;
        
        public EncodingSelector(ITextureRenderer textureRenderer, VersionData versionData, string data, byte charSize)
        {
            _textureRenderer = textureRenderer;
            _charSize = charSize;
            _versionData = versionData;

            // Check the compatibility of data to find the encoding type. TODO: later fix this rn only byte exist
            
            if (!CheckCompatibility(SelectedErrorCorrectionLevel, data))
            {
                Debug.LogError("Not compatible encoder format. Probably you need higher version QR code.");
                //TODO: Automatically select higher level version if selected Auto to version if you developed higher versions
            }
        }
        public void SetEncoding()
        {
            // Write data to QR code depending on encoding type.
            int dataSize = VersionUtility.GetEncodingModeBitCount();
            _textureRenderer.RenderingDataToTexture((byte)SelectedEncodingType, dataSize);
        }

        private bool CheckCompatibility(ErrorCorrectionLevel errorCorrectionLevel, string data)
        {
            SelectedErrorCorrectionLevel = errorCorrectionLevel;
            if (IsNumericCompatible(data, errorCorrectionLevel))
            {
                SelectedEncodingType = EncodingType.Numeric;
                return true;
            }
            if (IsAlphanumericCompatible(data, errorCorrectionLevel))
            {
                SelectedEncodingType = EncodingType.Alphanumeric;
                return true;
            }
            if (IsByteCompatible(data, errorCorrectionLevel))
            {
                SelectedEncodingType = EncodingType.Byte;
                return true;
            }
            if (IsKanjiCompatible(data, errorCorrectionLevel))
            {
                SelectedEncodingType = EncodingType.Kanji;
                return true;
            }
            if (errorCorrectionLevel != ErrorCorrectionLevel.Low)
            {
                return CheckCompatibility(errorCorrectionLevel switch
                {
                    ErrorCorrectionLevel.High => ErrorCorrectionLevel.Quality,
                    ErrorCorrectionLevel.Quality => ErrorCorrectionLevel.Medium,
                    ErrorCorrectionLevel.Medium  => ErrorCorrectionLevel.Low,
                    _ => throw new ArgumentOutOfRangeException(nameof(errorCorrectionLevel), errorCorrectionLevel, null)
                }, data);
            }

            SelectedEncodingType = EncodingType.Byte;
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