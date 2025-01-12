using System;
using System.Collections.Generic;
using UnityEngine;

namespace QR.Scriptable
{
    [CreateAssetMenu(fileName = "MaskPatternData", menuName = "Scriptable Objects/MaskPatternData")]
    public class MaskPatternData : ScriptableObject
    {
        // If func result is 0 ? Black : White 
        public readonly Dictionary<byte, Func<byte, byte, bool>> MaskPatterns = new Dictionary<byte, Func<byte, byte , bool>>()
        {
            {0, (x,y) => (y + x) % 2 != 0},
            {1, (x,y) => y % 2 != 0},
            {2, (x,y) => x % 3 != 0},
            {3, (x,y) => (y + x) % 3 != 0},
            {4, (x,y) => (Mathf.Floor(y / 2) + Mathf.Floor(x / 3)) % 2 == 0},
            {5, (x,y) => (y * x) % 2 + (y * x) % 3 == 0},
            {6, (x,y) => ((y * x) % 2 + (y * x) % 3) % 2 == 0},
            {7, (x,y) => ((y + x) % 2 + (y * x) % 3) % 2 == 0},

        };
    }
}

