using UnityEngine;
using KillChain.Core.Bootstrap;

namespace KillChain.Interface
{
    [AutoBootstrap]
    public class DebugVersionText : MonoBehaviour
    {
        GUIStyle guiStyle = null;

        private void Awake()
        {
            guiStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
                normal = new GUIStyleState
                {
                    textColor = Color.white,
                }
            };
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 50, Screen.height - 20, 50, 20), $"v{Application.version}", guiStyle);
        }
    }
}

