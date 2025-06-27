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
        public ErrorCorrectionLevel SelectedErrorCorrectionLevel { get; private set; }
        
        public EncodingSelector(ITextureRenderer textureRenderer, VersionData versionData, string data)
        {
            _textureRenderer = textureRenderer;
            
            // Check the compatibility of data to find the encoding type.
            IDataCompatibilityChecker compatibilityService = new DataCompatibilityService();
            var comp = compatibilityService.CheckCompatibility(data);
            var encodingType = EncodingAdaptor(comp);

            int ecComp = FindErrorCorrectionCompatibility(data.Length, versionData, encodingType);

            //TODO: Automatically select higher level version if selected Auto to version if you developed higher versions
            if (ecComp == -1)
                Debug.LogError("Not compatible encoder format. Probably you need higher version QR code.");
            
            SelectedEncodingType = encodingType;
            SelectedErrorCorrectionLevel = (ErrorCorrectionLevel)ecComp;
        }

        public void SetEncoding(ref OrganizedData organizedData)
        {
            // Write data to QR code depending on encoding type.
            int bitCount = VersionUtility.GetEncodingModeBitCount();
            _textureRenderer.RenderingDataToTexture((byte)SelectedEncodingType, bitCount);
            organizedData.Encoding = ((byte)SelectedEncodingType, bitCount);
        }
        
        private int FindErrorCorrectionCompatibility(int dataSize, VersionData versionData, EncodingType encodingType)
        {
            if (dataSize <= versionData.MaxMainDataSizeTable[new QRType(encodingType, ErrorCorrectionLevel.High)]) 
                return (int)ErrorCorrectionLevel.High;
            if (dataSize <= versionData.MaxMainDataSizeTable[new QRType(encodingType, ErrorCorrectionLevel.Quality)])
                return (int)ErrorCorrectionLevel.Quality;
            if (dataSize <= versionData.MaxMainDataSizeTable[new QRType(encodingType, ErrorCorrectionLevel.Medium)])
                return (int)ErrorCorrectionLevel.Medium;
            if (dataSize <= versionData.MaxMainDataSizeTable[new QRType(encodingType, ErrorCorrectionLevel.Low)])
                return (int)ErrorCorrectionLevel.Low;

            return -1;
        }

        private EncodingType EncodingAdaptor(Compatibility compatibility)
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