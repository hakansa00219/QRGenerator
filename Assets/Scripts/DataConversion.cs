using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using QR.Enums;

namespace QR.Scriptable
{
    [CreateAssetMenu(fileName = "QRDataConversion", menuName = "Scriptable Objects/QRDataConversion")]
    public class DataConversion : SerializedScriptableObject
    {
        [OdinSerialize]
        public Dictionary<int, (BytePattern pattern, InitPosition initPosition)> VersionOnePatterns = new Dictionary<int, (BytePattern pattern, InitPosition initPosition)>();

        [OdinSerialize]
        public Dictionary<BytePattern, ((int x,int y) bitSize, int[] bitOrder)> PatternBitOrder = new Dictionary<BytePattern, ((int x,int y) bitSize, int[] bitOrder)>();

        [OdinSerialize] 
        public Dictionary<(EncodingType, ErrorCorrectionLevel), byte> CharacterSizeTable = new();
        public ((int x, int y) bitSize, int[] bitOrder) GetBitDetails(BytePattern pattern) => PatternBitOrder[pattern];

        public struct InitPosition
        {
            public readonly int X;
            public readonly int Y;

            public InitPosition(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }

}

