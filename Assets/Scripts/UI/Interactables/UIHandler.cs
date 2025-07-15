using System.Runtime.InteropServices;
using QR;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Interactables
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Button genButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private InputField inputField;
        [SerializeField] private Text infoText;
        [SerializeField] private Text scaleText;
        [SerializeField] private Slider scaleSlider;

        private QRGenerator _qrGenerator;
        private QR.Logger.ILogger _logger;
        
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void DownloadFile(byte[] data, int length, string fileName)
#endif
        
        public void Init(QRGenerator qrGenerator, QR.Logger.ILogger logger)
        {
            _qrGenerator = qrGenerator;
            _logger = logger;
            genButton.onClick.AddListener(() => Generate(inputField.text));
            saveButton.onClick.AddListener(Save);
            scaleSlider.onValueChanged.AddListener(OnScaleSliderValueChanged);
        }

        private void OnScaleSliderValueChanged(float value)
        {
            scaleText.text = $"PNG Image Scale: x{value} \n" +
                             (_qrGenerator.LastTexture != null
                                 ? $"({_qrGenerator.LastTexture?.width * value}x{_qrGenerator.LastTexture?.height * value})"
                                 : "");
        }

        private void Generate(string input = "")
        {
            _qrGenerator.Generation(input);
            infoText.text =
                $"Version: {(int)_qrGenerator.Version}" + "\n" +
                $"Mask: {_qrGenerator.Mask}" + "\n" +
                $"EC: {_qrGenerator.ErrorCorrectionLevel}";
            
            scaleText.text = $"PNG Image Scale: x{scaleSlider.value} \n" +
                             (_qrGenerator.LastTexture != null
                                 ? $"({_qrGenerator.LastTexture?.width * scaleSlider.value}x{_qrGenerator.LastTexture?.height * scaleSlider.value})"
                                 : "");
        }

        private void Save()
        {
            if (_qrGenerator.LastTexture == null)
            {
                _logger.LogError("No QR code generated to save.", true);
                return;
            }
            Texture2D scaledTexture = _qrGenerator.LastTexture.ScaleTexture((int)scaleSlider.value);
            byte[] pngData = scaledTexture.EncodeToPNG();
            
#if UNITY_WEBGL && !UNITY_EDITOR
            DownloadFile(pngData, pngData.Length, "qr.png");
#else
            System.IO.File.WriteAllBytes(Application.dataPath + "/qr.png", pngData);
#endif
        }

    }
}