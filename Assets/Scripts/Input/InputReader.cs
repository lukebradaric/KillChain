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

        private void Awake()
        {
            _gameInput = Resources.Load<GameInput>("Input/GameInput");
        }

        private void Update()
        {
            _gameInput.SetHorizontalInput(UnityEngine.Input.GetAxisRaw("Horizontal"));
            _gameInput.SetVerticalInput(UnityEngine.Input.GetAxisRaw("Vertical"));

            _gameInput.SetMouseHorizontal(UnityEngine.Input.GetAxisRaw("Mouse X") * Time.deltaTime);
            _gameInput.SetMouseVertical(UnityEngine.Input.GetAxisRaw("Mouse Y") * Time.deltaTime);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) _gameInput.OnJumpPressed();

            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) _gameInput.OnFirePressed();

            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1)) _gameInput.OnAltFirePressed();

            if (UnityEngine.Input.GetKeyDown(KeyCode.F)) _gameInput.OnMeleePressed();

            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl)) _gameInput.OnSlamPressed();
        }
    }
}
