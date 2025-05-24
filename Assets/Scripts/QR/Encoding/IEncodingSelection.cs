using QR.Enums;

namespace QR.Encoding
{
    public interface IEncodingSelection
    {
        EncodingType SelectedEncodingType { get; }
        ErrorCorrectionLevel SelectedErrorCorrectionLevel { get; }
        public void SetEncoding();
    }
}