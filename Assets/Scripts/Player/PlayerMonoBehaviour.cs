using UnityEngine;

namespace KillChain.Player
{
    public class PlayerMonoBehaviour : MonoBehaviour
    {
        [SerializeField][HideInInspector] protected Player _player;

        private void Reset()
        {
            _player = this.GetComponent<Player>();
        }

        private void Awake()
        {
            if (_player == null)
            {
                _player = this.GetComponent<Player>();
            }
        }
    }
}

