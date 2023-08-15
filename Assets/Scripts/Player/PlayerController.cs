using ChainKill.Core.Extensions;
using ChainKill.Input;
using UnityEngine;

namespace ChainKill.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private PlayerGroundCheck _groundCheck;

        public bool IsGrounded { get; private set; }

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
            _rigidbody.AddForce(Vector3.up * _playerSettings.JumpForce, ForceMode.Impulse);
        }

        private void HandleGroundCheck()
        {
            IsGrounded = _groundCheck.Found();
        }

        private void HandleDrag()
        {
            if (IsGrounded)
            {
                _rigidbody.drag = _playerSettings.GroundDrag;
            }
            else
            {
                _rigidbody.drag = _playerSettings.AirDrag;
            }
        }

        private void HandleMovement()
        {
            _moveDirection = (_lookTransform.forward * _gameInput.VerticalInput + _lookTransform.right * _gameInput.HorizontalInput).normalized;

            if (IsGrounded)
            {
                _rigidbody.AddForce(_moveDirection * _playerSettings.MoveSpeed, ForceMode.Force);
            }
            else
            {
                _rigidbody.AddForce(_moveDirection * _playerSettings.MoveSpeed * _playerSettings.AirSpeedMultiplier, ForceMode.Force);
            }
        }

        private void HandleSpeed()
        {
            _flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

            if (_flatVelocity.magnitude > _playerSettings.MaxSpeed)
            {
                Vector3 newVelocity = _flatVelocity.normalized * _playerSettings.MaxSpeed;
                _rigidbody.velocity = new Vector3(newVelocity.x, _rigidbody.velocity.y, newVelocity.z);
            }
        }
    }
}
