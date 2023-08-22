using KillChain.Core.Bootstrap;
using UnityEngine;

namespace KillChain.Debug
{
    [AutoBootstrap]
    public class DebugStatesText : DebugText
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

            GUI.Label(new Rect(10, 10, 50, 20), $"P: {_player.StateMachine.CurrentState.GetType().Name}", _guiStyle);
            GUI.Label(new Rect(10, 30, 50, 20), $"C: {_player.ChainStateMachine.CurrentState.GetType().Name}", _guiStyle);
        }
    }
}

