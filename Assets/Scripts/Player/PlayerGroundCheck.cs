using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [Space]
    [Header("Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _groundLayerMask;

    public bool Found()
    {
        return Physics.Raycast(transform.position, Vector3.down, (_playerHeight * 0.5f) + _checkDistance, _groundLayerMask);
    }
}
