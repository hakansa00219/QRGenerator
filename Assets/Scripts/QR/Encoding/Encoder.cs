using System;
using QR.Analysis;
using UnityEngine;

namespace QR.Encoding
{
    public class Encoder : ITextureRenderer
    {
        private readonly IBitProvider _bitProvider;
        private readonly Texture2D _texture;
        public Encoder(ref Texture2D texture, IBitProvider bitProvider)
        {
            _bitProvider = bitProvider;
            _texture = texture;
            TextureSize = _texture.width;
        }

        public int TextureSize { get; }

        public void RenderingDataToTexture(int[] data, int charSize)
        {
            int dataSize = data.Length;
            for (int charIndex = 0; charIndex < dataSize; charIndex++)
            for (int bitIndex = charSize - 1; bitIndex >= 0; bitIndex--)
            {   
                var bitNode = _bitProvider.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, data[charIndex], bitIndex);
            }
        }
        public void RenderingDataToTexture(byte[] data)
        {
            int dataSize = data.Length;
            int charSize = 8;
            for (int charIndex = 0; charIndex < dataSize; charIndex++)
            for (int bitIndex = charSize - 1; bitIndex >= 0; bitIndex--)
            {   
                var bitNode = _bitProvider.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, data[charIndex], bitIndex);
            }
        }
        //If data is int charSize might be between 1 - 32. It can be byte short or int or even like 20 bit. (Some qr codes has weird size data)
        //TODO: 8 bit olmayanlar için bir test yapılması lazım. Emin değilim özellikle SetPixel2D ile birleşince.
        public void RenderingDataToTexture(int data, int charSize)
        {
            for (int bitIndex = charSize - 1; bitIndex >= 0; bitIndex--)
            {   
                var bitNode = _bitProvider.BitQueue.Dequeue();
                _texture.SetPixel2D(bitNode.X, bitNode.Y, data, bitIndex);
            }
        }

        public void RenderingBitToTexture(int x, int y, bool value)
        {
            _texture.SetPixel2D(x, y, value);
        }

        public void RenderingDataBlockToTexture(int x, int y, int blockWidth, int blockHeight, bool value)
        {
            _texture.SetPixel2D(x, y, blockWidth, blockHeight, value);
        }
    }
}