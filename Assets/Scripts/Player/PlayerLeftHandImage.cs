using UnityEngine;
using DG.Tweening;

namespace KillChain.Player
{
    public class PlayerLeftHandImage : MonoBehaviour
    {
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
            PlayerMelee.MeleeStarted += MeleeStartedHandler;
            PlayerMelee.MeleeCooldownStarted += MeleeCooldownStartedHandler;
        }

        private void OnDisable()
        {
            PlayerMelee.MeleeStarted -= MeleeStartedHandler;
            PlayerMelee.MeleeCooldownStarted -= MeleeCooldownStartedHandler;
        }

        private void MeleeStartedHandler()
        {
            if (_currentTween != null && (bool)_currentTween?.IsPlaying())
            {
                _currentTween.Kill();
                _currentTween = null;
            }

            _leftHandTransform.localPosition = _punchPosition;
        }

        private void MeleeCooldownStartedHandler(float cooldownDuration)
        {
            // Lerp hand to resting based on punch cooldown (+ small buffer)
            _currentTween = _leftHandTransform.DOLocalMove(_punchReturnPosition, cooldownDuration + 0.1f).SetEase(_punchReturnEase).OnKill(() =>
            {
                _currentTween = null;
            });
        }
    }
}
