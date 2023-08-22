using UnityEngine;
using UnityEngine.SceneManagement;
using KillChain.Core.Bootstrap;

namespace KillChain.Debug
{
    [AutoBootstrap]
    public class DebugReloadScene : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

}
