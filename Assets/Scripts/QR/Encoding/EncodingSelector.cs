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
        private readonly ITextureRenderer _textureRenderer;
        public EncodingType SelectedEncodingType { get; private set; }
        public ErrorCorrectionLevel SelectedErrorCorrectionLevel { get; private set; } = ErrorCorrectionLevel.High;
        
        public EncodingSelector(ITextureRenderer textureRenderer, VersionData versionData, string data)
        {
            _textureRenderer = textureRenderer;
            
            // Check the compatibility of data to find the encoding type.
            IDataCompatibilityChecker compatibilityService = new DataCompatibilityService();
            var comp = compatibilityService.CheckCompatibility(data);
            var encodingType = EncodingAdaptor(comp);
            
            int maxCharSize = versionData.MaxMainDataSizeTable[new QRType(encodingType, )]

            
            if (!CheckCompatibility(SelectedErrorCorrectionLevel, data))
            {
                Debug.LogError("Not compatible encoder format. Probably you need higher version QR code.");
                //TODO: Automatically select higher level version if selected Auto to version if you developed higher versions
            }
        }
        public void SetEncoding(ref OrganizedData organizedData)
        {
            // Write data to QR code depending on encoding type.
            int bitCount = VersionUtility.GetEncodingModeBitCount();
            _textureRenderer.RenderingDataToTexture((byte)SelectedEncodingType, bitCount);
            organizedData.Encoding = ((byte)SelectedEncodingType, bitCount);
        }

        public bool CheckVersionComp(ErrorCorrectionLevel errorCorrectionLevel, string data)
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
        
        public bool IsNumericCompatible(string data)
        {
            return data.All(char.IsDigit) && _charSize <= _versionData.MaxMainDataSizeTable[new QRType(EncodingType.Numeric, errorCorrectionLevel)];
        }
        public bool IsAlphanumericCompatible(string data)
        {
            var match = Regex.IsMatch(data, AlphaNumericPattern);
            return match && _charSize <= _versionData.MaxMainDataSizeTable[new QRType(EncodingType.Alphanumeric, errorCorrectionLevel)];
        }
        public bool IsKanjiCompatible(string data)
        {
            return data.Any(x => x >= 0x4E00 && x <= 0x9FBF) && _charSize <= _versionData.MaxMainDataSizeTable[new QRType(EncodingType.Kanji, errorCorrectionLevel)];
        }
        public bool IsByteCompatible(string data)
        {
            return !data.Any(x => x >= 0x4E00 && x <= 0x9FBF) && _charSize <= _versionData.MaxMainDataSizeTable[new QRType(EncodingType.Byte, errorCorrectionLevel)];
        }

        public EncodingType EncodingAdaptor(Compatibility compatibility)
        {
            return compatibility switch
            {
                Compatibility.Numeric => EncodingType.Numeric,
                Compatibility.Alphanumeric => EncodingType.Alphanumeric,
                Compatibility.Kanji => EncodingType.Kanji,
                Compatibility.Byte => EncodingType.Byte,
                _ => throw new ArgumentOutOfRangeException(nameof(compatibility), compatibility, null)
            };
        }
    }
}