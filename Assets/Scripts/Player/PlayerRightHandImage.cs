using KillChain.Core.StateMachine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KillChain.Player
{
    public class PlayerRightHandImage : SerializedMonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private Image _handImage;
        [SerializeField] private Player _player;

        [Space]
        [Header("Settings")]
        [SerializeField] private Dictionary<string, Sprite> _handSpritesDictionary = new Dictionary<string, Sprite>();

        private void OnEnable()
        {
            _player.ChainStateMachine.StateChanged += StateChangedHandler;
        }

        private void OnDisable()
        {
            _player.ChainStateMachine.StateChanged -= StateChangedHandler;
        }

        private void StateChangedHandler(State state)
        {
            _handImage.sprite = _handSpritesDictionary[state.GetType().Name];
        }
    }
}

