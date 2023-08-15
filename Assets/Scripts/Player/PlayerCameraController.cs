using ChainKill.Input;
using ChainKill.Player;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Space]
    [Header("Components")]
    [SerializeField] private Transform _lookTransform;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private PlayerSettings _playerSettings;

    private Vector2 _rotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _rotation.y += _gameInput.MouseHorizontal * _playerSettings.HorizontalSensitivity;
        _rotation.x -= _gameInput.MouseVertical * _playerSettings.VerticalSensitivity;

        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        _lookTransform.rotation = Quaternion.Euler(0, _rotation.y, 0);
    }
}
