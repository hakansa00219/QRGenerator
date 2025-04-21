using System;
using QR.Masking;
using QR.Scriptable;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Version = QR.Enums.Version;

[CreateAssetMenu(fileName = "QRSample", menuName = "Scriptable Objects/QRSample")]
public class QRSample : SerializedScriptableObject
{
    [OnValueChanged("CreateBitMatrix")]
    public Version dataVersion;
    public byte maskPattern;
    
    [TableMatrix(DrawElementMethod = "DrawBits", HorizontalTitle = "X", VerticalTitle = "Y")]
    public bool[,] BitMatrix;
    
    [TableMatrix(DrawElementMethod = "DrawBits", HorizontalTitle = "X", VerticalTitle = "Y")]
    public bool[,] UnmaskedBitMatrix;

    private void OnValidate()
    {
        if (UnmaskedBitMatrix == null) UnmaskedBitMatrix = new bool[BitMatrix.GetLength(0), BitMatrix.GetLength(1)];
        
        VersionData versionOne = Resources.Load<VersionData>("Data/Version1");
        MaskPattern maskTool = new MaskPattern(out _, ref versionOne, maskPattern);
        UnmaskedBitMatrix = maskTool.UnmaskedVersion(BitMatrix);
        
        // for (var i = 0; i < UnmaskedBitMatrix.GetLength(0); i++)
        // for (var j = 0; j < UnmaskedBitMatrix.GetLength(1); j++)
        // {
        //     UnmaskedBitMatrix[i, j] = !BitMatrix[i, j];
        // }
    }

    private void CreateBitMatrix()
    {
        QRResolution qrResolution = Resources.Load<QRResolution>("Data/QRResolutionData");
        int resolution = qrResolution.VersionResolutions[dataVersion];
        BitMatrix = new bool[resolution, resolution];
        UnmaskedBitMatrix = new bool[resolution, resolution];
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
}
