using System.Collections;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [Space]
    [Header("Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _groundLayerMask;

    private bool _found;
    private bool _disabled;

    private void FixedUpdate()
    {
        _found = Physics.Raycast(transform.position, Vector3.down, (_playerHeight * 0.5f) + _checkDistance, _groundLayerMask);
    }

    public void Disable(float time)
    {
        StartCoroutine(TempDisableCoroutine(time));
    }

    private IEnumerator TempDisableCoroutine(float time)
    {
        _disabled = true;
        yield return new WaitForSeconds(time);
        _disabled = false;
    }

    public bool IsFound()
    {
        if (_disabled)
        {
            return false;
        }

        return _found;
    }

    public float GetGroundAngle()
    {
        Physics.Raycast(transform.position, Vector3.down, out var raycastHit, (_playerHeight * 0.5f) + _checkDistance, _groundLayerMask);
        return Vector3.Angle(raycastHit.normal, Vector3.down);
    }
}
