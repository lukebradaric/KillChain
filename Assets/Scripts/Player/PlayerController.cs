using KillChain.Core;
using KillChain.Core.Events;
using KillChain.Core.Extensions;
using KillChain.Core.Generics;
using KillChain.Core.Gizmos;
using KillChain.Input;
using UnityEngine;

namespace KillChain.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerSlamEventChannel;

        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private Transform _slamHitboxTransform;
        [SerializeField] private PlayerGroundCheck _groundCheck;
        [SerializeField] private PlayerWeapon _playerWeapon;
        [SerializeField] private GameObject _slamParticlePrefab;

        [Space]
        [Header("Settings")]
        [SerializeField] private LayerMask _slamLayerMask;

        public Observable<bool> IsGrounded { get; private set; } = new Observable<bool>(false);
        public Observable<bool> IsMoving { get; private set; } = new Observable<bool>(false);
        public Observable<bool> IsSlamming { get; private set; } = new Observable<bool>(false);

        private Vector3 _moveDirection;
        private Vector3 _flatVelocity;

        private void OnEnable()
        {
            _gameInput.JumpPressed += JumpPressedHandler;
            _gameInput.SlamPressed += SlamPressedHandler;
            IsGrounded.ValueChanged += IsGroundedChangedHandler;
        }

        private void OnDisable()
        {
            _gameInput.JumpPressed -= JumpPressedHandler;
            _gameInput.SlamPressed -= SlamPressedHandler;
            IsGrounded.ValueChanged -= IsGroundedChangedHandler;
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
            if (!IsGrounded.Value)
            {
                return;
            }

            // Reset player Y velocity
            _rigidbody.SetVelocityY(0);
            _rigidbody.AddForce(Vector3.up * _playerData.JumpForce, ForceMode.Impulse);
        }

        private void SlamPressedHandler()
        {
            // If grounded or already slamming, return
            if (IsGrounded.Value || IsSlamming.Value || _playerWeapon.State.Value == PlayerWeaponState.Dash)
                return;

            // Move downwards and set slamming to true
            _rigidbody.SetVelocityY(_playerData.SlamSpeed);
            IsSlamming.Value = true;
        }

        private void IsGroundedChangedHandler(bool isGrounded)
        {
            // If we are now grounded and are slamming, invoke slam event
            if (isGrounded && IsSlamming.Value)
            {
                this.Slam();
            }
        }

        private void HandleGroundCheck()
        {
            IsGrounded.Value = _groundCheck.Found();
        }

        private void HandleDrag()
        {
            if (IsGrounded.Value)
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

            if (IsGrounded.Value)
            {
                _rigidbody.AddForce(_moveDirection * _playerData.MoveSpeed, ForceMode.Force);
            }
            else
            {
                _rigidbody.AddForce(_moveDirection * _playerData.MoveSpeed * _playerData.AirSpeedMultiplier, ForceMode.Force);
            }

            IsMoving.Value = _moveDirection.magnitude > 0;
        }

        private void HandleSpeed()
        {
            if (_playerWeapon.State.Value == PlayerWeaponState.Dash) return;

            _flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

            float maxSpeed = IsGrounded.Value ? _playerData.MaxSpeed : _playerData.MaxAirSpeed;

            if (_flatVelocity.magnitude > maxSpeed)
            {
                Vector3 newVelocity = _flatVelocity.normalized * maxSpeed;
                _rigidbody.velocity = new Vector3(newVelocity.x, _rigidbody.velocity.y, newVelocity.z);
            }
        }

        private void Slam()
        {
            _playerSlamEventChannel?.Invoke();
            IsSlamming.Value = false;
            Instantiate(_slamParticlePrefab, _slamHitboxTransform.position, Quaternion.identity);

            Collider[] colliders = Physics.OverlapBox(_slamHitboxTransform.position, new Vector3(5, 1, 5), Quaternion.identity, _slamLayerMask);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(_playerData.SlamDamage);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_slamHitboxTransform.position, new Vector3(5, 1, 5));
        }
    }
}
