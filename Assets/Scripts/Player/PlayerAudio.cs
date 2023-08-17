using KillChain.Audio;
using KillChain.Core.Events;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerChainBrokeEventChannel;
        [SerializeField] private VoidEventChannel _playerMeleeEventChannel;

        [Space]
        [Header("Components")]
        [SerializeField] private PlayerWeapon _playerWeapon;

        [Space]
        [Header("Audio")]
        [SerializeField] private AudioAsset _chainAttachAudioAsset;
        [SerializeField] private AudioAsset _chainBreakAudioAsset;
        [SerializeField] private AudioAsset _meleeAudioAsset;

        private void OnEnable()
        {
            _playerWeapon.State.ValueChanged += StateChangedHandler;
            _playerChainBrokeEventChannel.Event += PlayerChainBrokeHandler;
            _playerMeleeEventChannel.Event += PlayerMeleeHandler;

        }

        private void OnDisable()
        {
            _playerWeapon.State.ValueChanged -= StateChangedHandler;
            _playerChainBrokeEventChannel.Event -= PlayerChainBrokeHandler;
            _playerMeleeEventChannel.Event -= PlayerMeleeHandler;
        }

        private void StateChangedHandler(PlayerWeaponState state)
        {
            if (state == PlayerWeaponState.Attach) _chainAttachAudioAsset?.Play();
        }

        private void PlayerChainBrokeHandler()
        {
            _chainBreakAudioAsset?.Play();
        }

        private void PlayerMeleeHandler()
        {
            _meleeAudioAsset?.Play();
        }
    }
}

