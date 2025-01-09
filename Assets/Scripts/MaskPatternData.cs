using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace QR.Scriptable
{
    [CreateAssetMenu(fileName = "MaskPatternData", menuName = "Scriptable Objects/MaskPatternData")]
    public class MaskPatternData : SerializedScriptableObject
    {
        // If func result is 0 ? White : Black 
        [OdinSerialize] public Dictionary<byte, Func<byte, byte, bool>> MaskPatterns = new Dictionary<byte, Func<byte, byte , bool>>()
        {
            {0, (x,y) => (x * y) % 2 + (x * y) % 3 == 0},
            {1, (x,y) => (x / 2 + y / 3) % 2 == 0},
            {2, (x,y) => ((x * y) % 3 + x + y) % 2 == 0},
            {3, (x,y) => ((x * y) % 3 + x * y) % 2 == 0},
            {4, (x,_) => x % 2 == 0},
            {5, (x,y) => (x + y) % 2 == 0},
            {6, (x,y) => (x + y) % 3 == 0},
            {7, (_,y) => y % 3 == 0},

        };
    }
}

