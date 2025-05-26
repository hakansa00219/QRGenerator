using UnityEngine;

namespace QR.Masking
{
    public class FourthEvaluation : Evaluation
    {
        public override int Calculation(in Color[] pixels, int texSize, byte mask)
        {
            return 0;
        }
    }
}