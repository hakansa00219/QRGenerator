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
        private readonly IBitProvider _bitProvider;
        private readonly byte _data;
        private readonly EncodingType _encodingType;
        
        public Length(ref Texture2D texture, ref IBitProvider bitProvider, EncodingType encodingType, VersionData versionData, byte data)
        {
            _versionData = versionData;
            _encodingType = encodingType;
            _bitProvider = bitProvider;
            _texture = texture;
            _data = data;
        }
        
        // Write Data Length size(as a byte value) to QR
        public void SetLength()
        {
            int dataSize = VersionUtility.GetCharacterBitLength(_versionData.dataVersion, _encodingType);
            for (int i = dataSize - 1; i >= 0; i--)
            {
                var bitNode = _bitProvider.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, ((_data >> i) & 1) == 1 ? Color.black : Color.white);
            }
        }
    }
}