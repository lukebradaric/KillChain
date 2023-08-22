using KillChain.Core.Bootstrap;
using UnityEngine;

namespace KillChain.Debug
{
    [AutoBootstrap]
    public class DebugPlayerVelocity : DebugText
    {
        private Player.Player _player;

        private void Start()
        {
            _player = FindObjectOfType<Player.Player>();
        }

        private void OnGUI()
        {
            if (_player == null)
            {
                return;
            }

            _guiStyle.alignment = TextAnchor.LowerLeft;
            GUI.Label(new Rect(5, Screen.height - 23, 30, 20), $"Velocity: {_player.Rigidbody.velocity.magnitude}", _guiStyle);
        }
    }
}

