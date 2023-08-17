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
        [SerializeField] private VoidEventChannel _playerParryEventChannel;

        [Space]
        [Header("Components")]
        [SerializeField] private PlayerWeapon _playerWeapon;

        [Space]
        [Header("Audio")]
        [SerializeField] private AudioAsset _playerChainAttachAudioAsset;
        [SerializeField] private AudioAsset _playerChainBreakAudioAsset;
        [SerializeField] private AudioAsset _playerMeleeAudioAsset;
        [SerializeField] private AudioAsset _playerParryAudioAsset;

        private void OnEnable()
        {
            _playerWeapon.State.ValueChanged += StateChangedHandler;
            _playerChainBrokeEventChannel.Event += PlayerChainBrokeHandler;
            _playerMeleeEventChannel.Event += PlayerMeleeHandler;
            _playerParryEventChannel.Event += PlayerParryHandler;
        }

        private void OnDisable()
        {
            _playerWeapon.State.ValueChanged -= StateChangedHandler;
            _playerChainBrokeEventChannel.Event -= PlayerChainBrokeHandler;
            _playerMeleeEventChannel.Event -= PlayerMeleeHandler;
            _playerParryEventChannel.Event -= PlayerParryHandler;
        }

        private void StateChangedHandler(PlayerWeaponState state)
        {
            if (state == PlayerWeaponState.Attach) _playerChainAttachAudioAsset?.Play();
        }

        private void PlayerChainBrokeHandler() => _playerChainBreakAudioAsset?.Play();

        private void PlayerMeleeHandler() => _playerMeleeAudioAsset?.Play();

        private void PlayerParryHandler() => _playerParryAudioAsset?.Play();
    }
}

