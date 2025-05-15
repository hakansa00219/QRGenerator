using UnityEngine;

namespace QR.Masking
{
    public abstract class Evaluation
    {
        public abstract int Calculation(byte mask, ref Texture2D texture);
    }
}