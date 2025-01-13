using System;
using UnityEngine;

namespace QR.Utilities
{
    public class DataTypeUtility
    {
        public static int GetBitSizePerChar_Numeric(int groupSize)
        {
            if (groupSize is > 3 or <= 0)
            {
                Debug.LogError("Group size must be between 1 and 3");
                return 0;
            }

            return groupSize switch
            {
                1 => 4,
                2 => 7,
                3 => 10,
                _ => throw new ArgumentOutOfRangeException(nameof(groupSize), groupSize, null)
            };
        }

        public static int GetBitSizePerChar_AlphaNumeric(int groupSize)
        {
            if (groupSize is > 2 or <= 0)
            {
                Debug.LogError("Group size must be between 1 and 2");
                return 0;
            }

            return groupSize switch
            {
                1 => 6,
                2 => 11,
                _ => throw new ArgumentOutOfRangeException(nameof(groupSize), groupSize, null)
            };
        }

        public static int GetBitSizePerChar_Byte()
        {
            return 8;
        }

        public static int GetBitSizePerChar_Kanji()
        {
            return 13;
        }
    }
}