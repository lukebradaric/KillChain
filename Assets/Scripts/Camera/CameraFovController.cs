using KillChain.Player.States;
using UnityEngine;

namespace KillChain.Camera
{
    public class CameraFovController : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private CameraData _cameraData;
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private PlayerStateMachine _playerStateMachine;

        private float _defaultCameraFov;

        private void Awake()
        {
            _defaultCameraFov = _camera.fieldOfView;
        }

        private void Update()
        {
            // Calculate new FOV, default if grounded
            float newCameraFov = _playerStateMachine.CurrentState == _playerStateMachine.MoveState ? _defaultCameraFov :
                _defaultCameraFov + (_cameraData.MaxFovIncrease / (_cameraData.MaxVelocityFovChange / _rigidbody.velocity.magnitude));

            // Clamp FOV value
            newCameraFov = Mathf.Clamp(newCameraFov, _defaultCameraFov, _defaultCameraFov + _cameraData.MaxFovIncrease);

            // Lerp FOV
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, newCameraFov, _cameraData.CameraFovLerpSpeed * Time.deltaTime);
        }
    }
}

