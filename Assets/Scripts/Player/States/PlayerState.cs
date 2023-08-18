using KillChain.Input;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public abstract class PlayerState
    {
        [HideInInspector] public GameInput _gameInput;
        [HideInInspector] public PlayerData _playerData;
        [HideInInspector] public Rigidbody _rigidbody;
        [HideInInspector] public Transform _lookTransform;
        [HideInInspector] public PlayerWeapon _playerWeapon;
        [HideInInspector] public PlayerStateMachine _playerStateMachine;
        [HideInInspector] public PlayerGroundCheck _playerGroundCheck;

        public abstract void Enter();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void Exit();

        protected void LimitVelocity(float maxVelocityMagnitude)
        {
            Vector3 _flatVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

            if (_flatVelocity.magnitude > maxVelocityMagnitude)
            {
                Vector3 newVelocity = _flatVelocity.normalized * maxVelocityMagnitude;
                _rigidbody.velocity = new Vector3(newVelocity.x, _rigidbody.velocity.y, newVelocity.z);
            }
        }

        protected void Move(float velocityMultiplier = 1f)
        {
            // Movement
            Vector3 moveDirection = (_lookTransform.forward * _gameInput.VerticalInput + _lookTransform.right * _gameInput.HorizontalInput).normalized;
            _rigidbody.AddForce(moveDirection * _playerData.MoveSpeed * velocityMultiplier);
        }
    }
}

