using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using QR.Enums;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine.Serialization;
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
        public Dictionary<QRType, CharacterSize> CharacterSizeTable = new Dictionary<QRType, CharacterSize>();

        [TableMatrix(DrawElementMethod = "DrawBits", HorizontalTitle = "X", VerticalTitle = "Y")]
        public bool[,] BitMatrix;

        [ReadOnly] public int dataSize;
        public ((int x, int y) bitSize, int[] bitOrder) GetBitDetails(BytePattern pattern) => PatternBitOrder[pattern];

        private void OnValidate()
        {
            dataSize = BitMatrix.Cast<bool>().Count(b => b);
        }

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
    
    public struct QRType
    {
        public EncodingType EncodingType;
        public ErrorCorrectionLevel ErrorCorrectionLevel;

        public QRType(EncodingType encodingType, ErrorCorrectionLevel errorCorrectionLevel)
        {
            EncodingType = encodingType;
            ErrorCorrectionLevel = errorCorrectionLevel;
        }
    }
}

