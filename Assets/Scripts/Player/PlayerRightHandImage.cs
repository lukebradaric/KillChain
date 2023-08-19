using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KillChain.Player
{
    [System.Serializable]
    internal class PlayerChainStateSprites
    {
        public PlayerChainState state;
        public Sprite sprite;
    }

    public class PlayerRightHandImage : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private Image _handImage;
        [SerializeField] private PlayerChain _playerChain;

        [Space]
        [Header("Settings")]
        [SerializeField] private List<PlayerChainStateSprites> _handSprites = new List<PlayerChainStateSprites>();
        // Dictionary of the sprite for each weapon state
        private Dictionary<PlayerChainState, Sprite> _handSpritesDictionary = new Dictionary<PlayerChainState, Sprite>();

        private void Awake()
        {
            foreach (var handSprite in _handSprites)
            {
                _handSpritesDictionary.Add(handSprite.state, handSprite.sprite);
            }
        }

        private void OnEnable()
        {
            _playerChain.CurrentState.ValueChanged += StateChangedHandler;
        }

        private void OnDisable()
        {
            _playerChain.CurrentState.ValueChanged -= StateChangedHandler;
        }

        private void StateChangedHandler(PlayerChainState playerChainState)
        {
            _handImage.sprite = _handSpritesDictionary[playerChainState];
        }
    }
}

