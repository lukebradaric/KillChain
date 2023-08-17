using System.Collections;
using UnityEngine;

namespace KillChain.Core.Managers
{
    [CreateAssetMenu(menuName = "KillChain/Managers/TimeManager")]
    public class TimeManager : ScriptableObject
    {
        public void StopTime(float duration)
        {
            CoroutineManager.Instance.StartCoroutine(StopTimeCoroutine(duration));
        }

        private IEnumerator StopTimeCoroutine(float duration)
        {
            UnityEngine.Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration);
            UnityEngine.Time.timeScale = 1;
        }
    }
}
