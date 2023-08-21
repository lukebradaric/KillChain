using KillChain.Audio;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerAudio : PlayerMonoBehaviour
    {
        [Space]
        [Header("Audio")]
        [SerializeField] private AudioAsset _playerChainThrowAudioAsset;
        [SerializeField] private AudioAsset _playerChainBreakAudioAsset;
        [SerializeField] private AudioAsset _playerMeleeAudioAsset;
        [SerializeField] private AudioAsset _playerParryAudioAsset;
        [SerializeField] private AudioAsset _playerSlamAudioAsset;

        private void OnEnable()
        {
            _player.ChainThrowEventChannel.Event += PlayerChainThrowEventHandler;
            _player.ChainBreakEventChannel.Event += PlayerChainBrokeHandler;
            _player.MeleeEventChannel.Event += PlayerMeleeHandler;
            _player.ParryEventChannel.Event += PlayerParryHandler;
            _player.SlamEventChannel.Event += PlayerSlamHandler;
        }

        private void OnDisable()
        {
            _player.ChainThrowEventChannel.Event -= PlayerChainThrowEventHandler;
            _player.ChainBreakEventChannel.Event -= PlayerChainBrokeHandler;
            _player.MeleeEventChannel.Event -= PlayerMeleeHandler;
            _player.ParryEventChannel.Event -= PlayerParryHandler;
            _player.SlamEventChannel.Event -= PlayerSlamHandler;
        }

        private void PlayerChainThrowEventHandler() => _playerChainThrowAudioAsset?.Play();

        private void PlayerChainBrokeHandler() => _playerChainBreakAudioAsset?.Play();

        private void PlayerMeleeHandler() => _playerMeleeAudioAsset?.Play();

        private void PlayerParryHandler() => _playerParryAudioAsset?.Play();

        private void PlayerSlamHandler() => _playerSlamAudioAsset?.Play();
    }
}

