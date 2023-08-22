using UnityEngine;

namespace KillChain.Debug
{
    public class DebugText : MonoBehaviour
    {
        protected GUIStyle _guiStyle = null;

        private void Awake()
        {
            _guiStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
                normal = new GUIStyleState
                {
                    textColor = Color.white,
                },
                alignment = TextAnchor.UpperLeft
            };
        }
    }
}

