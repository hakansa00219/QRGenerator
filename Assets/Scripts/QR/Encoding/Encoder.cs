using QR.Analysis;
using UnityEngine;

namespace QR.Encoding
{
    public class Encoder : ITextureRenderer
    {
        private readonly IBitProvider _bitProvider;
        public Encoder(ref Texture2D texture, IBitProvider bitProvider)
        {
            _bitProvider = bitProvider;
            Texture = texture;
            TextureSize = Texture.width;
        }

        public int TextureSize { get; }
        public Texture2D Texture { get; }
        public int RemainingBitCount => _bitProvider.RemainingBitCount;
        private Color[] Pixels => Texture.GetPixels();

        public Texture2D GetCopyTexture()
        {
            Texture2D copyTexture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGB565, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            copyTexture.SetPixels(Pixels);
            copyTexture.Apply();

            return copyTexture;
        }

        public void RenderingDataToTexture(int[] data, int charSize)
        {
            int dataSize = data.Length;
            for (int charIndex = 0; charIndex < dataSize; charIndex++)
            for (int bitIndex = charSize - 1; bitIndex >= 0; bitIndex--)
            {   
                var bitNode = _bitProvider.BitQueue.Dequeue();
                Texture.SetPixel2D(bitNode.X, bitNode.Y, data[charIndex], bitIndex);
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
                Texture.SetPixel2D(bitNode.X, bitNode.Y, data[charIndex], bitIndex);
            }
        }
        //If data is int charSize might be between 1 - 32. It can be byte short or int or even like 20 bit. (Some qr codes has weird size data)
        public void RenderingDataToTexture(int data, int charSize)
        {
            for (int bitIndex = charSize - 1; bitIndex >= 0; bitIndex--)
            {   
                var bitNode = _bitProvider.BitQueue.Dequeue();
                Texture.SetPixel2D(bitNode.X, bitNode.Y, data, bitIndex);
            }
        }

        public void RenderingBitToTexture(int x, int y, bool value)
        {
            Texture.SetPixel2D(x, y, value);
        }

        public void RenderingDataBlockToTexture(int x, int y, int blockWidth, int blockHeight, bool value)
        {
            Texture.SetPixel2D(x, y, blockWidth, blockHeight, value);
        }
    }
}