using UnityEngine;

namespace KillChain.Audio
{
    // Commented out because we only want one settings object :)
    [CreateAssetMenu(menuName = "KillChain/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        [Space]
        [Header("Pooling")]
        [Tooltip("Should the Sound object pool pre-instantiate objects for use?")]
        [SerializeField] private bool _prewarmPool = true;
        public bool PrewarmPool => _prewarmPool;

        [Tooltip("How many Sound objects should be pre-instantiated?")]
        [SerializeField] private int _poolPrewarmSize = 10;
        public int PoolPrewarmSize => _poolPrewarmSize;

        [Space]
        [Header("Repetition")]
        [Tooltip("Should there be a limit on how many times one Audio Asset can be played in a second? (Helps to reduce sound clutter)")]
        [SerializeField] private bool _limitAudioPlayRate = false;
        public bool LimitAudioPlayRate => _limitAudioPlayRate;

        [Tooltip("The maximum amount of times an Audio Asset can be played in one second. (Eg. 10 = 10 times per second)")]
        [SerializeField] private float _maxAudioPlayRate = 10;
        public float MaxAudioPlayRate => _maxAudioPlayRate;

        [Space]
        [Header("Playback")]
        [Tooltip("Time waited after playing audio before it is returned to the pool. (Prevents clipping)")]
        [SerializeField] private float _audioPlaytimeBuffer = 0.15f;
        public float AudioPlaytimeBuffer => _audioPlaytimeBuffer;
    }
}
