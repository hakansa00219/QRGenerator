using System;
using QR.Analysis;
using UnityEngine;

namespace QR.Encoding
{
    public interface ITextureRenderer
    {
        public int TextureSize { get; }
        /// <summary>
        /// Rendering data array (Multiple characters with different sized) to texture. Position doesn't matter since module rendering it the right way.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="charSize"></param>
        public void RenderingDataToTexture(int[] data, int charSize);
        
        /// <summary>
        /// Rendering data array (Multiple characters) to texture. Position doesn't matter since module rendering it the right way.
        /// </summary>
        /// <param name="data"></param>
        public void RenderingDataToTexture(byte[] data);
        
        /// <summary>
        /// Rendering "charSize" sized data to texture. Position doesn't matter since module rendering it the right way.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="charSize"></param>
        public void RenderingDataToTexture(int data, int charSize);
        
        /// <summary>
        /// Rendering 1 bit/pixel to texture
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public void RenderingBitToTexture(int x, int y, bool value);

        /// <summary>
        /// Rendering block same valued data to texture. "x", "y" starting position than increasing with "blockWidth" and "blockHeight". 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="blockWidth"></param>
        /// <param name="blockHeight"></param>
        /// <param name="value"></param>
        public void RenderingDataBlockToTexture(int x, int y, int blockWidth, int blockHeight, bool value);
    }
}