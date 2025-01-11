using UnityEngine;

namespace QR
{
    public class FilterInfo
    {
        private readonly Texture2D _texture;
        private readonly int _maskedFilterBits;
        
        public FilterInfo(ref Texture2D texture, int maskedFilterBits)
        {
            _texture = texture;
            _maskedFilterBits = maskedFilterBits;
        }

        public void SetMaskedFilterBits()
        {
            string binary = System.Convert.ToString(_maskedFilterBits, 2).PadLeft(15, '0');
            Debug.Log(binary);
            
            for (var i = 0; i < binary.Length; i++)
            {
                switch (i)
                {
                    case < 7:
                        _texture.SetPixel(8, i, binary[i] == '0' ? Color.white : Color.black); 
                        _texture.SetPixel(i + (int)Mathf.Floor(i / 6), 12, binary[i] == '0' ? Color.white : Color.black);
                        break;
                    case >= 7:
                        _texture.SetPixel(8, 5 + i + (int)Mathf.Floor(i / 9), binary[i] == '0' ? Color.white : Color.black); 
                        _texture.SetPixel(6 + i, 12, binary[i] == '0' ? Color.white : Color.black);
                        break;
                }
            }
        }
    }
}