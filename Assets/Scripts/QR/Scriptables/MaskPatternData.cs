using System;
using System.Collections.Generic;
using UnityEngine;

namespace QR.Scriptable
{
    [CreateAssetMenu(fileName = "MaskPatternData", menuName = "Scriptable Objects/MaskPatternData")]
    public class MaskPatternData : ScriptableObject
    {
        // If func result is 0 ? Black : White 
        public readonly Dictionary<byte, Func<int, int, bool>> MaskPatterns = new Dictionary<byte, Func<int, int , bool>>()
        {
            {0, (x,y) => (y + x) % 2 != 0},
            {1, (x,y) => y % 2 != 0},
            {2, (x,y) => x % 3 != 0},
            {3, (x,y) => (y + x) % 3 != 0},
            {4, (x,y) => (MathF.Floor(y / 2) + MathF.Floor(x / 3)) % 2 != 0},
            {5, (x,y) => (y * x) % 2 + (y * x) % 3 != 0},
            {6, (x,y) => ((y * x) % 2 + (y * x) % 3) % 2 != 0},
            {7, (x,y) => ((y + x) % 2 + (y * x) % 3) % 2 != 0},

        };
    }
}

