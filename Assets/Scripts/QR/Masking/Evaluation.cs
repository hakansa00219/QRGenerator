using UnityEngine;

namespace QR.Masking
{
    public abstract class Evaluation
    {
        public abstract int Calculation(in Color[] pixels, int texSize, byte mask);
    }
}