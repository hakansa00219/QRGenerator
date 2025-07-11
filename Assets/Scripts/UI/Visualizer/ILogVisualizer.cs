using UnityEngine;

namespace UI.Visualizer
{
    public interface ILogVisualizer
    {
        public void Visualize(string msg, Color color);
    }
}