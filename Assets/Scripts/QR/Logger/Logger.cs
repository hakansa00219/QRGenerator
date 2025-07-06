using UnityEngine;

namespace QR.Enums.Logger
{
    public class Logger
    {
        private readonly RectTransform _logPanel;
        
        public Logger(RectTransform logPanel)
        {
            _logPanel = logPanel;
        }
        
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(msg);
            
            Visualize(msg, Color.white);
        }

        public static void LogError(string msg)
        {
            UnityEngine.Debug.LogError(msg);
            
            Visualize(msg, Color.red);
        }
        
        public static void LogWarning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
            
            Visualize(msg, Color.yellow);
        }

        private static void Visualize(string msg, Color color)
        {
            
        }
    }
}