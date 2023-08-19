using DG.Tweening;
using KillChain.Input;
using KillChain.Player.States;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerRightHandAnimator : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private PlayerStateMachine _playerStateMachine;
        [SerializeField] private RectTransform _rightHandTransform;

        [Space]
        [Header("Settings")]
        [SerializeField] private float _bobAnimationHeight;
        [SerializeField] private float _bobAnimationDuration;
        [SerializeField] private Ease _animationEase;

        private Sequence _moveSequence = null;
        private Tween _idleTween = null;

        private void Update()
        {
            if (_playerStateMachine.CurrentState == _playerStateMachine.MoveState && _gameInput.MoveInput.magnitude > 0)
            {
                PlayMoveAnimation();
            }
            else
            {
                PlayIdleAnimation();
            }
        }

        private void KillIdleTween()
        {
            if (_idleTween != null)
            {
                _idleTween.Kill();
                _idleTween = null;
            }
        }

        private void KillMoveSequence()
        {
            if (_moveSequence != null)
            {
                _moveSequence.Kill();
                _moveSequence = null;
            }
        }

        private void PlayMoveAnimation()
        {
            if (_moveSequence != null)
            {
                return;
            }

            KillIdleTween();

            _moveSequence = DOTween.Sequence();
            _moveSequence.Append(_rightHandTransform.DOLocalMoveY(_bobAnimationHeight, _bobAnimationDuration).SetEase(_animationEase));
            _moveSequence.Append(_rightHandTransform.DOLocalMoveY(0, _bobAnimationDuration).SetEase(_animationEase));
            _moveSequence.SetLoops(-1, LoopType.Yoyo);
        }

        private void PlayIdleAnimation()
        {
            if (_idleTween != null)
            {
                return;
            }

            KillMoveSequence();

            _idleTween = _rightHandTransform.DOLocalMoveY(0, _bobAnimationDuration).SetEase(_animationEase);
        }
    }
}

