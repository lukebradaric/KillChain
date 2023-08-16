using UnityEngine;

namespace KillChain.Player
{
    public class PlayerRightHandAnimator : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Animator _animator;

        private void Update()
        {
            if (!_playerController.IsGrounded)
            {
                _animator.Play("PlayerIdleAnimation");
                return;
            }

            if (_playerController.IsMoving)
                _animator.Play("PlayerMoveAnimation");
            else
                _animator.Play("PlayerIdleAnimation");
        }
    }
}

