using DG.Tweening;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerRightHandAnimator : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private RectTransform _rightHandTransform;

        [Space]
        [Header("Settings")]
        [SerializeField] private float _bobAnimationHeight;
        [SerializeField] private float _bobAnimationDuration;
        [SerializeField] private Ease _animationEase;

        private Sequence _moveSequence = null;
        private Tween _idleTween = null;

        private void OnEnable()
        {
            _playerController.IsMoving.ValueChanged += IsMovingValueChangedHandler;
            _playerController.IsGrounded.ValueChanged += IsGroundedValueChangedHandler;
        }

        private void OnDisable()
        {
            _playerController.IsMoving.ValueChanged -= IsMovingValueChangedHandler;
            _playerController.IsGrounded.ValueChanged -= IsGroundedValueChangedHandler;
        }

        private void IsGroundedValueChangedHandler(bool isGrounded)
        {
            if (isGrounded)
            {
                if (_playerController.IsMoving.Value)
                    PlayMoveAnimation();
            }
            else
            {
                PlayIdleAnimation();
            }
        }

        private void IsMovingValueChangedHandler(bool isMoving)
        {
            if (!_playerController.IsGrounded.Value)
                return;

            if (isMoving)
                PlayMoveAnimation();
            else
                PlayIdleAnimation();
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
            KillIdleTween();

            _moveSequence = DOTween.Sequence();
            _moveSequence.Append(_rightHandTransform.DOLocalMoveY(_bobAnimationHeight, _bobAnimationDuration).SetEase(_animationEase));
            _moveSequence.Append(_rightHandTransform.DOLocalMoveY(0, _bobAnimationDuration).SetEase(_animationEase));
            _moveSequence.SetLoops(-1, LoopType.Yoyo);
        }

        private void PlayIdleAnimation()
        {
            KillMoveSequence();

            _idleTween = _rightHandTransform.DOLocalMoveY(0, _bobAnimationDuration).SetEase(_animationEase);
        }
    }
}

