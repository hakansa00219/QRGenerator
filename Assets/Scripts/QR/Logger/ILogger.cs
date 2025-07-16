using UnityEngine;

namespace QR.Logger
{
    public interface ILogger
    {
        public void Log(string msg, bool visualize = false);
        public void LogError(string msg, bool visualize = false);
        public void LogWarning(string msg, bool visualize = false);
    }
}