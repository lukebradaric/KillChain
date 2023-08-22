using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillChain.Tests
{
    public class Test_ReloadScene : MonoBehaviour
    {
        public KeyCode keyCode;

        private void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
