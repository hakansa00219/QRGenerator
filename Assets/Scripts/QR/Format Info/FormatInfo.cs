using System;
using QR.Encoding;

namespace QR
{
    public class FormatInfo
    {
        private readonly ITextureRenderer _textureRenderer;
        private readonly int _maskedFormatBits;
        
        public FormatInfo(ITextureRenderer textureRenderer, int maskedFormatBits)
        {
            _textureRenderer = textureRenderer;
            _maskedFormatBits = maskedFormatBits;
        }

        public void SetMaskedFormatBits()
        {
            string binary = System.Convert.ToString(_maskedFormatBits, 2).PadLeft(15, '0');
   
            for (var i = 0; i < binary.Length; i++)
            {
                bool value = binary[i] == '1';
                switch (i)
                { 
                    case < 7:
                        _textureRenderer.RenderBitToTexture(8, 20 - i, value); 
                        _textureRenderer.RenderBitToTexture(i + (int)MathF.Floor(i / 6), 8, value);
                        break;
                    case >= 7:
                        _textureRenderer.RenderBitToTexture(8, 15 + i + (int)MathF.Floor(i / 9), value); 
                        _textureRenderer.RenderBitToTexture(6 + i, 8, value);
                        break;
                }
            }
        }
    }
}