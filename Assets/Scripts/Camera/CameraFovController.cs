using KillChain.Player;
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
        [SerializeField] private PlayerController _playerController;

        private float _defaultCameraFov;

        private void Awake()
        {
            _defaultCameraFov = _camera.fieldOfView;
        }

        private void Update()
        {
            // TODO (002) : Actual/Better implementation
            // 90% of max speed = 90% of max fov increase

            // Calculate player velocity, ignoring Y speed
            Vector3 flatPlayerVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

            // Calculate new FOV, default if grounded
            float newCameraFov = _playerController.IsGrounded.Value ? _defaultCameraFov :
                _defaultCameraFov + (_cameraData.MaxFovIncrease / (_cameraData.MaxVelocityFovChange / flatPlayerVelocity.magnitude));

            // Clamp FOV value
            newCameraFov = Mathf.Clamp(newCameraFov, _defaultCameraFov, _defaultCameraFov + _cameraData.MaxFovIncrease);

            // Lerp FOV
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, newCameraFov, _cameraData.CameraFovLerpSpeed * Time.deltaTime);
        }
    }
}

