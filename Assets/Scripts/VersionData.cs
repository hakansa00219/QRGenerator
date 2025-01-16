using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using QR.Enums;
using Sirenix.Utilities;
using UnityEditor;
using Version = QR.Enums.Version;

namespace QR.Scriptable
{
    [CreateAssetMenu(fileName = "QRDataConversion", menuName = "Scriptable Objects/QRDataConversion")]
    public class VersionData : SerializedScriptableObject
    {
        [OnValueChanged("CreateBitMatrix")]
        public Version dataVersion;
        [OdinSerialize]
        public Dictionary<BytePattern, ((int x,int y) bitSize, int[] bitOrder)> PatternBitOrder = new Dictionary<BytePattern, ((int x,int y) bitSize, int[] bitOrder)>();

        [OdinSerialize] 
        public Dictionary<(EncodingType, ErrorCorrectionLevel), CharacterSize> CharacterSizeTable = new Dictionary<(EncodingType, ErrorCorrectionLevel), CharacterSize>();

        [TableMatrix(DrawElementMethod = "DrawBits")]
        public bool[,] BitMatrix;
        
        public ((int x, int y) bitSize, int[] bitOrder) GetBitDetails(BytePattern pattern) => PatternBitOrder[pattern];

        private void CreateBitMatrix()
        {
            QRResolution qrResolution = Resources.Load<QRResolution>("Data/QRResolutionData");
            int resolution = qrResolution.VersionResolutions[dataVersion];
            BitMatrix = new bool[resolution, resolution];
        }
#if UNITY_EDITOR
        private static bool DrawBits(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown &&
                rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }
            
            EditorGUI.DrawRect(rect.Padding(1), value ? new Color(1,1,1,1) : new Color(0,0,0,100));

            return value;
        }
#endif
        public readonly struct CharacterSize
        {
            public readonly int MaxMainData;
            public readonly int ErrorCorrectionData;
        }
    }
}

