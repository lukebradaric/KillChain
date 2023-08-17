using UnityEngine;

namespace KillChain.Camera
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(UnityEngine.Camera.main.transform.position);
        }
    }
}

