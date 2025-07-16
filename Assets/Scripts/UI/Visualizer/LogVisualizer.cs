using System.Collections;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Visualizer
{
    public class LogVisualizer : MonoBehaviour, ILogVisualizer
    {
        [SerializeField, ReadOnly] private RectTransform logTextPrefab;
        [SerializeField, ReadOnly] private RectTransform logContainer;
        
        //Can be done with a pool, but for now, this is fine.
        public void Visualize(string msg, Color color)
        {
            
            if (logContainer == null)
            {
                Debug.LogError("Log container is not assigned in LogVisualizer.");
                return;
            }

            if (logTextPrefab == null)
            {
                Debug.LogError("Log text prefab is not assigned in LogVisualizer.");
                return;
            }
            
            RectTransform logText = Instantiate(logTextPrefab, logContainer);
            
            // Set text and color
            Text text = logText.GetComponent<Text>();
            text.text = msg;
            text.color = color;

            StartCoroutine(DisappearingAnimation(logText));
        }

        private IEnumerator DisappearingAnimation(RectTransform logText)
        {
            yield return new WaitForSecondsRealtime(5);
            
            if (logText == null)
            {
                yield break; // Exit if logText is null
            }
            
            Text textComponent = logText.GetComponent<Text>();

            while (true)
            {
                // Fade out the text
                Color color = textComponent.color;
                color.a -= Time.deltaTime / 3f; 

                if (color.a <= 0)
                {
                    Destroy(logText.gameObject);
                    break;
                }

                textComponent.color = color;

                yield return null;
            }
        }
    }
}