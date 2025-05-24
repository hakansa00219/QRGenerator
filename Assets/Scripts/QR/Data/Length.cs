using QR.Encoding;
using QR.Enums;
using QR.Scriptable;
using QR.Utilities;

namespace QR
{
    public class Length
    {
        private readonly VersionData _versionData;
        private readonly ITextureRenderer _textureRenderer;
        private readonly byte _data;
        private readonly EncodingType _encodingType;
        
        public Length(ITextureRenderer textureRenderer, EncodingType encodingType, VersionData versionData, byte data)
        {
            _versionData = versionData;
            _encodingType = encodingType;
            _textureRenderer = textureRenderer;
            _data = data;
        }
        
        // Write Data Length size(as a byte value) to QR
        public void SetLength()
        {
            int dataSize = VersionUtility.GetCharacterBitLength(_versionData.dataVersion, _encodingType);
            _textureRenderer.RenderingDataToTexture(_data, dataSize);
        }
    }
}