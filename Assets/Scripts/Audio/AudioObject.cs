using System.Collections;
using UnityEngine;

namespace KillChain.Audio
{
    public class AudioObject : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void LoadAsset(AudioAsset audioAsset)
        {
            if (audioAsset == null)
            {
                Debug.LogError("AudioAsset provided for loading is null.");
                return;
            }

            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            if (audioAsset.AudioMixerGroup != null)
                _audioSource.outputAudioMixerGroup = audioAsset.AudioMixerGroup;

            _audioSource.clip = audioAsset.AudioClip;
            _audioSource.priority = (int)audioAsset.AudioPriority;
            _audioSource.volume = audioAsset.Volume;
            _audioSource.pitch = audioAsset.Pitch;
        }

        public void Play(bool releaseToPool = true)
        {
            _audioSource.Play();

            if (releaseToPool)
                StartCoroutine(ReleaseCoroutine());
        }

        private IEnumerator ReleaseCoroutine()
        {
            yield return new WaitForSeconds
                ((_audioSource.clip.length / _audioSource.pitch)
                + AudioManager.AudioSettings.AudioPlaytimeBuffer
                );

            AudioObjectPool.Release(this);
        }
    }
}
