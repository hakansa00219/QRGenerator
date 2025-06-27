using QR.Encoding;
using QR.Enums;
using QR.Scriptable;
using QR.Structs;
using QR.Utilities;

namespace QR
{
    public class Length
    {
        private readonly VersionData _versionData;
        private readonly ITextureRenderer _textureRenderer;
        private readonly int _characterSize;
        private readonly EncodingType _encodingType;
        
        public Length(ITextureRenderer textureRenderer, EncodingType encodingType, VersionData versionData, int characterSize)
        {
            _versionData = versionData;
            _encodingType = encodingType;
            _textureRenderer = textureRenderer;
            _characterSize = characterSize;
        }
        
        // Write Data Length size(as a byte value) to QR
        public void SetLength(ref OrganizedData organizedData)
        {
            int bitCount = VersionUtility.GetCharacterBitCount(_versionData.dataVersion, _encodingType);
            _textureRenderer.RenderingDataToTexture(_characterSize, bitCount);
            organizedData.Length = (_characterSize, bitCount);
        }
    }
}