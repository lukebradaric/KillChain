using KillChain.Input;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public abstract class PlayerState
    {
        public void SetPlayer(Player player) => _player = player;
        protected Player _player;

        public abstract void Enter();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void Exit();

        public virtual void OnDrawGizmos() { }

        protected void Move(float velocityMultiplier = 1f)
        {
            // Movement
            Vector3 moveDirection = (_player.LookTransform.forward * _player.GameInput.MoveInput.y + _player.LookTransform.right * _player.GameInput.MoveInput.x).normalized;
            _player.Rigidbody.AddForce(moveDirection * _player.Data.MoveSpeed * velocityMultiplier);
        }
    }
}

