using KillChain.Input;
using KillChain.Player;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Space]
    [Header("Components")]
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _lookTransform;

    private Vector2 _rotation;

    private void Start()
    {
        // TODO : Better cursor locking
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Application.targetFrameRate = 144;
    }

    private void Update()
    {
        _rotation.y += _gameInput.MouseHorizontal * _playerData.HorizontalSensitivity;
        _rotation.x -= _gameInput.MouseVertical * _playerData.VerticalSensitivity;

        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        _lookTransform.rotation = Quaternion.Euler(0, _rotation.y, 0);
    }
}
