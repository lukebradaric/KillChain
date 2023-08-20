using KillChain.Audio;
using KillChain.Core.Events;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerAudio : PlayerMonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerChainBrokeEventChannel;
        [SerializeField] private VoidEventChannel _playerMeleeEventChannel;
        [SerializeField] private VoidEventChannel _playerParryEventChannel;
        [SerializeField] private VoidEventChannel _playerSlamEventChannel;

        [Space]
        [Header("Audio")]
        [SerializeField] private AudioAsset _playerChainAttachAudioAsset;
        [SerializeField] private AudioAsset _playerChainBreakAudioAsset;
        [SerializeField] private AudioAsset _playerMeleeAudioAsset;
        [SerializeField] private AudioAsset _playerParryAudioAsset;
        [SerializeField] private AudioAsset _playerSlamAudioAsset;

        private void OnEnable()
        {
            //_player.Chain.CurrentState.ValueChanged += StateChangedHandler;
            _playerChainBrokeEventChannel.Event += PlayerChainBrokeHandler;
            _playerMeleeEventChannel.Event += PlayerMeleeHandler;
            _playerParryEventChannel.Event += PlayerParryHandler;
            _playerSlamEventChannel.Event += PlayerSlamHandler;
        }

        private void OnDisable()
        {
            //_player.Chain.CurrentState.ValueChanged -= StateChangedHandler;
            _playerChainBrokeEventChannel.Event -= PlayerChainBrokeHandler;
            _playerMeleeEventChannel.Event -= PlayerMeleeHandler;
            _playerParryEventChannel.Event -= PlayerParryHandler;
            _playerSlamEventChannel.Event += PlayerSlamHandler;
        }

        //private void StateChangedHandler(PlayerChainStateEnum state)
        //{
        //    if (state == PlayerChainStateEnum.Throw) _playerChainAttachAudioAsset?.Play();
        //}

        private void PlayerChainBrokeHandler() => _playerChainBreakAudioAsset?.Play();

        private void PlayerMeleeHandler() => _playerMeleeAudioAsset?.Play();

        private void PlayerParryHandler() => _playerParryAudioAsset?.Play();

        private void PlayerSlamHandler() => _playerSlamAudioAsset?.Play();
    }
}

