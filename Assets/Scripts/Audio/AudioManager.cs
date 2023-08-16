using KillChain.Bootstrap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KillChain.Audio
{
    [AutoBootstrap]
    public class AudioManager : MonoBehaviour
    {
        public static AudioSettings AudioSettings { get; private set; }

        private static HashSet<IAudioAsset> _limitedAudio = new HashSet<IAudioAsset>();

        private void Awake() => AudioSettings = Resources.Load<AudioSettings>("Audio/AudioSettings");

        private void OnEnable()
        {
            // Set this transform as AudioObject transform parent
            AudioObjectPool.AudioObjectTransformParent = this.transform;

            if (AudioSettings.PrewarmPool)
                AudioObjectPool.Prewarm(AudioSettings.PoolPrewarmSize);

            if (AudioSettings.LimitAudioPlayRate)
                StartCoroutine(AudioPlayRateCoroutine());
        }

        public static void Play(AudioAsset audioAsset)
        {
            // If repetition limiting is being used
            if (AudioSettings.LimitAudioPlayRate)
            {
                // If sound should not ignore limit and is found in limit hashset, return
                if (!audioAsset.IgnoreAudioPlayRate && _limitedAudio.Contains(audioAsset))
                    return;

                _limitedAudio.Add(audioAsset);
            }

            AudioObject audioObject = AudioObjectPool.Get();
            audioObject.LoadAsset(audioAsset);
            audioObject.Play();
        }

        public static void PlayEditor(AudioAsset audioAsset)
        {
            AudioObject audioObject = AudioObjectPool.Create();
            audioObject.gameObject.name = "Editor" + audioObject.gameObject.name;
            audioObject.gameObject.hideFlags = HideFlags.HideAndDontSave;
            audioObject.LoadAsset(audioAsset);
            audioObject.Play(false);
        }

        private IEnumerator AudioPlayRateCoroutine()
        {
            yield return new WaitForSeconds(1 / AudioSettings.MaxAudioPlayRate);
            _limitedAudio.Clear();
            StartCoroutine(AudioPlayRateCoroutine());
        }
    }
}
