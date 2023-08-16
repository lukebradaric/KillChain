using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace KillChain.Audio
{
    public class AudioObjectPool
    {
        public static Transform AudioObjectTransformParent;

        private static ObjectPool<AudioObject> _pool = new ObjectPool<AudioObject>(Create, OnGet, OnRelease);

        public static void Prewarm(int count)
        {
            HashSet<AudioObject> audioObjects = new HashSet<AudioObject>();

            for (int i = 0; i < count; i++)
                audioObjects.Add(Get());

            foreach (AudioObject obj in audioObjects)
                Release(obj);
        }

        public static AudioObject Create()
        {
            GameObject gameObject = new GameObject(typeof(AudioObject).Name);
            gameObject.transform.SetParent(AudioObjectTransformParent);
            AudioObject audioObject = gameObject.AddComponent<AudioObject>();
            return audioObject;
        }

        public static AudioObject Get() => _pool.Get();

        public static void Release(AudioObject audioObject) => _pool.Release(audioObject);

        private static void OnGet(AudioObject audioObject)
        {
            audioObject.gameObject.SetActive(true);
        }

        private static void OnRelease(AudioObject audioObject)
        {
            audioObject.gameObject.SetActive(false);
        }

    }
}
