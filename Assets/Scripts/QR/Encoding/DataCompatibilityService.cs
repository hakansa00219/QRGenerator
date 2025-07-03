using System.Linq;
using System.Text.RegularExpressions;

namespace QR.Encoding
{
    public class DataCompatibilityService : IDataCompatibilityChecker
    {
        private const string AlphaNumericPattern = @"^[0-9A-Z $%*+-./:]+$";
        
        public Compatibility CheckCompatibility(string data)
        {
            if (IsNumericCompatible(data))
                return Compatibility.Numeric;
            if (IsAlphanumericCompatible(data))
                return Compatibility.Alphanumeric;
            if (IsKanjiCompatible(data))
                return Compatibility.Kanji;
            
            return Compatibility.Byte;
        }
        
        public bool IsNumericCompatible(string data)
        {
            return data.Length == 0 || data.All(char.IsDigit);
        }
        public bool IsAlphanumericCompatible(string data)
        {
            if (data.Length == 0) return true;
            
            var match = Regex.IsMatch(data, AlphaNumericPattern);
            return match;
        }
        public bool IsKanjiCompatible(string data)
        {
            if (data.Length == 0) return true;
            
            System.Text.Encoding shiftJis = System.Text.Encoding.GetEncoding(932); // 932 = Shift-jis Japanese 
            return data.All(c =>
            {
                byte[] bytes = shiftJis.GetBytes(new char[] { c });
                if (bytes.Length != 2) return false;

                int value = (bytes[0] << 8) | bytes[1];
                return value is >= 0x8140 and <= 0x9FFC or
                    >= 0xE040 and <= 0xEBBF;
            });
        }
        public bool IsByteCompatible(string data)
        {
            //Every character is byte compatible.
            //Other compatibilities created for optimization.
            //This is not optimized.
            return true;
        }
    }
}