using System;
using UnityEngine;
using System.Linq;

namespace QR.Masking
{
    public class FourthEvaluation : Evaluation
    {
        public override int Calculation(in bool[,] bits, int horizontalSize, int verticalSize)
        {
            
            int penalty = 0;
            Debug.Log($"Fourth Penalty: {penalty}");
            return penalty;
        }
    }
}