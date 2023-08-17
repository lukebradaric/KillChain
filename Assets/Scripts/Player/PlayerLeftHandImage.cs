using UnityEngine;
using DG.Tweening;
using KillChain.Core.Events;

namespace KillChain.Player
{
    public class PlayerLeftHandImage : MonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerMeleeEventChannel;
        [SerializeField] private FloatEventChannel _playerMeleeCooldownStartedEventChannel;

        [Space]
        [Header("Components")]
        [SerializeField] private RectTransform _leftHandTransform;

        [Space]
        [Header("Settings")]
        [SerializeField] private Vector3 _punchPosition;
        [SerializeField] private Vector3 _punchReturnPosition;
        [SerializeField] private Ease _punchReturnEase;

        private Tween _currentTween = null;

        private void OnEnable()
        {
            _playerMeleeEventChannel.Event += PlayerMeleeHandler;
            _playerMeleeCooldownStartedEventChannel.Event += PlayerMeleeCooldownStartedHandler;
        }

        private void OnDisable()
        {
            _playerMeleeEventChannel.Event -= PlayerMeleeHandler;
            _playerMeleeCooldownStartedEventChannel.Event -= PlayerMeleeCooldownStartedHandler;
        }

        private void PlayerMeleeHandler()
        {
            if (_currentTween != null && (bool)_currentTween?.IsPlaying())
            {
                _currentTween.Kill();
                _currentTween = null;
            }

            _leftHandTransform.localPosition = _punchPosition;
        }

        private void PlayerMeleeCooldownStartedHandler(float cooldownDuration)
        {
            // Lerp hand to resting based on punch cooldown (+ small buffer)
            _currentTween = _leftHandTransform.DOLocalMove(_punchReturnPosition, cooldownDuration + 0.1f).SetEase(_punchReturnEase).OnKill(() =>
            {
                _currentTween = null;
            });
        }
    }
}
