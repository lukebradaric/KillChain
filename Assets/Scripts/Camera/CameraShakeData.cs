using UnityEngine;

namespace KillChain.Camera
{
    [CreateAssetMenu(menuName = "KillChain/CameraShakeData")]
    public class CameraShakeData : ScriptableObject
    {

        [SerializeField] private float _duration;
        public float Duration => _duration;

        [SerializeField] private float _strength;
        public float Strength => _strength;

        [SerializeField] private int _vibrato;
        public int Vibrato => _vibrato;

        [SerializeField] private float _randomness;
        public float Randomness => _randomness;

        [SerializeField] private bool _fadeOut;
        public bool FadeOut => _fadeOut;

        public void Play()
        {
            CameraShaker.Instance.Shake(this);
        }
    }
}
