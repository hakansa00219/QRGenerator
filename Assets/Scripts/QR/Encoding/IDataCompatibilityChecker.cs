using QR.Enums;

namespace QR.Encoding
{
    public interface IDataCompatibilityChecker
    {
        Compatibility CheckCompatibility(string data);
        bool IsNumericCompatible(string data);
        bool IsAlphanumericCompatible(string data);
        bool IsKanjiCompatible(string data);
        bool IsByteCompatible(string data);
        
    }
}