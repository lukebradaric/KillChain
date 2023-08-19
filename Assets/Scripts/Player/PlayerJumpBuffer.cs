using System.Collections;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerJumpBuffer : PlayerMonoBehaviour
    {
        public bool Enabled => _jumpBufferCoroutine != null;

        private Coroutine _jumpBufferCoroutine;

        private void OnEnable()
        {
            _player.GameInput.JumpPressed += JumpPressedHandler;
        }

        private void OnDisable()
        {
            _player.GameInput.JumpPressed -= JumpPressedHandler;
        }

        private void JumpPressedHandler()
        {
            if(_jumpBufferCoroutine != null)
            {
                StopCoroutine(_jumpBufferCoroutine);
            }

            _jumpBufferCoroutine = StartCoroutine(JumpBufferCoroutine());
        }

        private IEnumerator JumpBufferCoroutine()
        {
            yield return new WaitForSeconds(_player.Data.JumpBufferTime);
            _jumpBufferCoroutine = null;
        }

        public void Consume()
        {
            StopCoroutine(_jumpBufferCoroutine);
            _jumpBufferCoroutine = null;
        }
    }
}

