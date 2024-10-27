using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using QR.Enums;

namespace QR.Scriptables
{
    [CreateAssetMenu(fileName = "QRDataConversion", menuName = "Scriptable Objects/QRDataConversion")]
    public class QRDataConversion : SerializedScriptableObject
    {
        [OdinSerialize]
        public Dictionary<int, QREnums.BytePattern> VersionOnePatterns = new Dictionary<int, QREnums.BytePattern>();

        [OdinSerialize]
        public Dictionary<QREnums.BytePattern, ((int x,int y) bitSize, int[] bitOrder)> patternBitOrder = new Dictionary<QREnums.BytePattern, ((int x,int y) bitSize, int[] bitOrder)>();
    }

}

