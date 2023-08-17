using UnityEngine;

namespace KillChain.Camera
{
    [CreateAssetMenu(menuName = "KillChain/Camera/CameraData")]
    public class CameraData : ScriptableObject
    {
        [SerializeField] private float _maxFovIncrease;
        public float MaxFovIncrease => _maxFovIncrease;

        [SerializeField] private float _cameraFovLerpSpeed;
        public float CameraFovLerpSpeed => _cameraFovLerpSpeed;

        [Tooltip("The maximum player velocity that will affect camera fov. Velocity beyond this value will not increase camera fov.")]
        [SerializeField] private float _maxVelocityFovChange;
        public float MaxVelocityFovChange => _maxVelocityFovChange;
    }
}

