using System.Collections.Generic;
using QR.Enums;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace QR.Scriptable
{
    [CreateAssetMenu(fileName = "QRResolutionData", menuName = "Scriptable Objects/QRResolutionData")]
    public class QRResolution : SerializedScriptableObject
    {
        [OdinSerialize]
        public Dictionary<Version, int> VersionResolutions = new Dictionary<Version, int>();
    }
}

