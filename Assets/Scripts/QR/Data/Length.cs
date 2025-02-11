using QR.Analysis;
using QR.Enums;
using QR.Scriptable;
using QR.Utilities;
using UnityEngine;

namespace QR
{
    public class Length
    {
        private readonly Texture2D _texture;
        private readonly VersionData _versionData;
        private readonly DataAnalyzer _analyzer;
        private readonly byte _data;
        private readonly EncodingType _encodingType;
        
        public Length(ref Texture2D texture, ref DataAnalyzer analyzer, EncodingType encodingType, VersionData versionData, byte data)
        {
            _versionData = versionData;
            _encodingType = encodingType;
            _analyzer = analyzer;
            _texture = texture;
            _data = data;
        }
        
        public void SetLength()
        {
            int dataSize = VersionUtility.GetCharacterBitLength(_versionData.dataVersion, _encodingType);
            for (int i = dataSize - 1; i >= 0; i--)
            {
                var bitNode = _analyzer.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((_data >> i) & 1) == 1 ? Color.black : Color.white);
            }
        }
    }
}