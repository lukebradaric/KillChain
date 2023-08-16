using KillChain.Bootstrap;
using UnityEngine;

namespace KillChain.Input
{
    [AutoBootstrap]
    public class InputReader : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput = default;

        private void Update()
        {
            _gameInput.SetHorizontalInput(UnityEngine.Input.GetAxisRaw("Horizontal"));
            _gameInput.SetVerticalInput(UnityEngine.Input.GetAxisRaw("Vertical"));

            _gameInput.SetMouseHorizontal(UnityEngine.Input.GetAxisRaw("Mouse X") * Time.deltaTime);
            _gameInput.SetMouseVertical(UnityEngine.Input.GetAxisRaw("Mouse Y") * Time.deltaTime);

            if (UnityEngine.Input.GetKeyDown("space")) _gameInput.OnJumpPressed();

            if (UnityEngine.Input.GetKeyDown("mouse 0")) _gameInput.OnFirePressed();

            if (UnityEngine.Input.GetKeyDown("mouse 1")) _gameInput.OnAltFirePressed();

            if (UnityEngine.Input.GetKeyDown("f")) _gameInput.OnMeleePressed();
        }
    }
}
