using UnityEngine;

namespace ChainKill.Input
{
    public class InputReader : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;

        private void Update()
        {
            _gameInput.SetHorizontalInput(UnityEngine.Input.GetAxisRaw("Horizontal"));
            _gameInput.SetVerticalInput(UnityEngine.Input.GetAxisRaw("Vertical"));

            _gameInput.SetMouseHorizontal(UnityEngine.Input.GetAxisRaw("Mouse X") * Time.deltaTime);
            _gameInput.SetMouseVertical(UnityEngine.Input.GetAxisRaw("Mouse Y") * Time.deltaTime);

            if (UnityEngine.Input.GetKeyDown("space")) _gameInput.OnJumpPressed();

            if (UnityEngine.Input.GetKeyDown("mouse 0")) _gameInput.OnFirePressed();

            if (UnityEngine.Input.GetKeyDown("mouse 1")) _gameInput.OnAltFirePressed();
        }
    }
}
