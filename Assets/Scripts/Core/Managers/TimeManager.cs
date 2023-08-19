using System.Collections;
using UnityEngine;

namespace KillChain.Core.Managers
{
    // Commented out because we should only have 1
    //[CreateAssetMenu(menuName = "KillChain/Managers/TimeManager")]
    public class TimeManager : ScriptableObject
    {
        private Coroutine _stopTimeCoroutine;

        public void TimeStop(float duration)
        {
            if(_stopTimeCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_stopTimeCoroutine);
            }

            _stopTimeCoroutine = CoroutineManager.Instance.StartCoroutine(StopTimeCoroutine(duration));
        }

        private IEnumerator StopTimeCoroutine(float duration)
        {
            UnityEngine.Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration);
            UnityEngine.Time.timeScale = 1;
            _stopTimeCoroutine = null;
        }
    }
}
