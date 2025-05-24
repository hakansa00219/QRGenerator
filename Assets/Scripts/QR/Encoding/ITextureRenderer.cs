using QR.Analysis;
using UnityEngine;

namespace QR.Encoding
{
    public interface ITextureRenderer
    {
        public void RenderDataToTexture(int[] data, int charSize);
        public void RenderDataToTexture(byte[] data);
        public void RenderDataToTexture(int data, int charSize);
        public void RenderBitToTexture(int x, int y, bool value);
    }
}