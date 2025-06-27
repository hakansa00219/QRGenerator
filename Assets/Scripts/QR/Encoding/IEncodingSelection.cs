using QR.Enums;
using QR.Structs;

namespace QR.Encoding
{
    public interface IEncodingSelection
    {
        EncodingType SelectedEncodingType { get; }
        ErrorCorrectionLevel SelectedErrorCorrectionLevel { get; }
        public void SetEncoding(ref OrganizedData organizedData);
    }
}