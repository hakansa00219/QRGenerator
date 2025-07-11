using UI.Visualizer;
using UnityEngine;

namespace QR.Logger
{
    public class Logger : ILogger
    {
        private readonly ILogVisualizer _visualizer;
        public Logger(ILogVisualizer visualizer)
        {
            _visualizer = visualizer;
        }
        
        public void Log(string msg, bool visualize = false)
        {
            UnityEngine.Debug.Log(msg);
            
            if (visualize)
                Visualize(msg, Color.white);
        }

        public void LogError(string msg, bool visualize = false)
        {
            UnityEngine.Debug.LogError(msg);
            
            if (visualize)
                Visualize(msg, Color.red);
        }
        
        public void LogWarning(string msg, bool visualize = false)
        {
            UnityEngine.Debug.LogWarning(msg);
            
            if (visualize)
                Visualize(msg, Color.yellow);
        }

        private void Visualize(string msg, Color color)
        {
            _visualizer.Visualize(msg, color);
        }
    }
}