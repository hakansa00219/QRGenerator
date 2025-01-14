using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using QR.Enums;

namespace QR.Scriptable
{
    [CreateAssetMenu(fileName = "QRDataConversion", menuName = "Scriptable Objects/QRDataConversion")]
    public class VersionData : SerializedScriptableObject
    {
        [OdinSerialize]
        public Dictionary<BytePattern, ((int x,int y) bitSize, int[] bitOrder)> PatternBitOrder = new Dictionary<BytePattern, ((int x,int y) bitSize, int[] bitOrder)>();

        [OdinSerialize] 
        public Dictionary<(EncodingType, ErrorCorrectionLevel), CharacterSize> CharacterSizeTable = new Dictionary<(EncodingType, ErrorCorrectionLevel), CharacterSize>();
        
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

        public readonly struct CharacterSize
        {
            public readonly int MaxMainData;
            public readonly int ErrorCorrectionData;
        }
    }
}

