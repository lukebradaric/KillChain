using KillChain.Audio;
using KillChain.Camera;
using KillChain.Core;
using UnityEngine;

namespace KillChain.Tests
{
    public class Test_Breakable : MonoBehaviour, IMeleeable
    {
        public GameObject _breakParticlePrefab;
        public CameraShakeData _cameraShakeData;
        public AudioAsset _breakAudioAsset;

        public void Melee()
        {
            Instantiate(_breakParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            _cameraShakeData?.Play();
            _breakAudioAsset?.Play();
        }
    }
}
