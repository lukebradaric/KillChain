using System.Collections;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [Space]
    [Header("Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _groundLayerMask;

    private bool _disabled;

    // TODO : refactor this
    public void TempDisable()
    {
        StartCoroutine(TempDisableCoroutine());
    }

    private IEnumerator TempDisableCoroutine()
    {
        _disabled = true;
        yield return new WaitForSeconds(0.25f);
        _disabled = false;
    }

    public bool Found()
    {
        if (_disabled) return false;

        return Physics.Raycast(transform.position, Vector3.down, (_playerHeight * 0.5f) + _checkDistance, _groundLayerMask);
    }
}
