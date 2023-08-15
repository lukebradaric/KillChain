using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChainKill.Player
{
    [System.Serializable]
    internal class PlayerWeaponStateSprite
    {
        public PlayerWeaponState state;
        public Sprite sprite;
    }

    public class PlayerHandImage : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private PlayerWeapon _playerWeapon;
        [SerializeField] private Image _handImage;

        [Space]
        [Header("Settings")]
        [SerializeField] private List<PlayerWeaponStateSprite> _handSprites = new List<PlayerWeaponStateSprite>();
        // Dictionary of the sprite for each weapon state
        private Dictionary<PlayerWeaponState, Sprite> _handSpritesDictionary = new Dictionary<PlayerWeaponState, Sprite>();

        private void Awake()
        {
            foreach (var handSprite in _handSprites)
            {
                _handSpritesDictionary.Add(handSprite.state, handSprite.sprite);
            }
        }

        private void OnEnable()
        {
            _playerWeapon.StateChanged += StateChangedHandler;
        }

        private void OnDisable()
        {
            _playerWeapon.StateChanged -= StateChangedHandler;
        }

        private void StateChangedHandler(PlayerWeaponState playerWeaponState)
        {
            _handImage.sprite = _handSpritesDictionary[playerWeaponState];
        }
    }
}

