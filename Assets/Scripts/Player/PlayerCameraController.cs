using KillChain.Input;
using KillChain.Player;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Space]
    [Header("Components")]
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _lookTransform;

    [Space]
    [Header("Settings")]
    [SerializeField] private float _maxFovIncrease;
    [SerializeField] private float _cameraFovLerpSpeed;

    private float _defaultCameraFov;
    private Vector2 _rotation;

    private void Awake()
    {
        _defaultCameraFov = _camera.fieldOfView;
    }

    private void Start()
    {
        // TODO : Better cursor locking
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Application.targetFrameRate = 144;
    }

    private void Update()
    {
        _rotation.y += _gameInput.MouseHorizontal * _playerData.HorizontalSensitivity * Time.deltaTime * 1000;
        _rotation.x -= _gameInput.MouseVertical * _playerData.VerticalSensitivity * Time.deltaTime * 1000;

        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        _lookTransform.rotation = Quaternion.Euler(0, _rotation.y, 0);

        // TODO (002) : Actual/Better implementation
        // 90% of max speed = 90% of max fov increase
        Vector3 flatPlayerVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        Debug.Log(flatPlayerVelocity.magnitude);
        float newCameraFov = _defaultCameraFov + (_maxFovIncrease / (_playerData.MaxAirSpeed / flatPlayerVelocity.magnitude));
        if (_playerController.IsGrounded.Value)
            newCameraFov = _defaultCameraFov;
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, newCameraFov, _cameraFovLerpSpeed * Time.deltaTime);
    }
}
