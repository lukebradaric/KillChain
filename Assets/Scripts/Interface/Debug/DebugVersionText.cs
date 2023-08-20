using UnityEngine;
using KillChain.Core.Bootstrap;

namespace KillChain.Interface.Debug
{
    [AutoBootstrap]
    public class DebugVersionText : DebugText
    {
        private void OnGUI()
        {
            _guiStyle.alignment = TextAnchor.LowerRight;
            GUI.Label(new Rect(Screen.width - 55, Screen.height - 25, 50, 20), $"v{Application.version}", _guiStyle);
        }
    }
}

