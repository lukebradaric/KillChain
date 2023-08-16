using KillChain.Core.Extensions;
using KillChain.Input;
using UnityEngine;

namespace KillChain.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerGroundCheck _groundCheck;

        public bool IsGrounded { get; private set; }
        public bool IsMoving { get; private set; }

        private Vector3 _moveDirection;
        private Vector3 _flatVelocity;

        private void OnEnable()
        {
            _gameInput.JumpPressed += JumpPressedHandler;
        }

        private void OnDisable()
        {
            _gameInput.JumpPressed -= JumpPressedHandler;
        }

        private void FixedUpdate()
        {
            HandleGroundCheck();
            HandleDrag();
            HandleMovement();
            HandleSpeed();
        }

        private void JumpPressedHandler()
        {
            if (!IsGrounded)
            {
                return;
            }

            // Reset player Y velocity
            _rigidbody.SetVelocityY(0);
            _rigidbody.AddForce(Vector3.up * _playerData.JumpForce, ForceMode.Impulse);
        }

        private void HandleGroundCheck()
        {
            IsGrounded = _groundCheck.Found();
        }

        private void HandleDrag()
        {
            if (IsGrounded)
            {
                _rigidbody.drag = _playerData.GroundDrag;
            }
            else
            {
                _rigidbody.drag = _playerData.AirDrag;
            }
        }

        private void HandleMovement()
        {
            _moveDirection = (_lookTransform.forward * _gameInput.VerticalInput + _lookTransform.right * _gameInput.HorizontalInput).normalized;

            if (IsGrounded)
            {
                _rigidbody.AddForce(_moveDirection * _playerData.MoveSpeed, ForceMode.Force);
            }
            else
            {
                _rigidbody.AddForce(_moveDirection * _playerData.MoveSpeed * _playerData.AirSpeedMultiplier, ForceMode.Force);
            }

            IsMoving = _moveDirection.magnitude > 0;
        }

        private void HandleSpeed()
        {
            _flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

            if (_flatVelocity.magnitude > _playerData.MaxSpeed)
            {
                Vector3 newVelocity = _flatVelocity.normalized * _playerData.MaxSpeed;
                _rigidbody.velocity = new Vector3(newVelocity.x, _rigidbody.velocity.y, newVelocity.z);
            }
        }
    }
}
