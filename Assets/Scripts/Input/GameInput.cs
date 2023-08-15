using System;
using UnityEngine;

namespace ChainKill.Input
{
    [CreateAssetMenu(menuName = "ChainKill/GameInput")]
    public class GameInput : ScriptableObject
    {
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }

        public float MouseHorizontal { get; private set; }
        public float MouseVertical { get; private set; }

        public event Action JumpPressed;
        public event Action FirePressed;
        // TODO: Rename alt fire (Right click)
        public event Action AltFirePressed;

        public void SetHorizontalInput(float value)
        {
            HorizontalInput = value;
        }

        public void SetVerticalInput(float value)
        {
            VerticalInput = value;
        }

        public void SetMouseHorizontal(float value)
        {
            MouseHorizontal = value;
        }

        public void SetMouseVertical(float value)
        {
            MouseVertical = value;
        }

        public void OnJumpPressed()
        {
            JumpPressed?.Invoke();
        }

        public void OnFirePressed()
        {
            FirePressed?.Invoke();
        }

        public void OnAltFirePressed()
        {
            AltFirePressed?.Invoke();
        }
    }
}
