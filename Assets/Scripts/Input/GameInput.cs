using System;
using UnityEngine;

namespace KillChain.Input
{
    [CreateAssetMenu(menuName = "KillChain/GameInput")]
    public class GameInput : ScriptableObject
    {
        public Vector2 MoveInput { get; private set; }

        public float MouseHorizontal { get; private set; }
        public float MouseVertical { get; private set; }

        public bool IsSlidePressed { get; private set; }

        public event Action JumpPressed;
        public event Action FirePressed;
        public event Action FireReleased;
        // TODO: Rename alt fire (Right click)
        public event Action AltFirePressed;
        public event Action AltFireReleased;
        public event Action MeleePressed;
        public event Action SlamPressed;
        public event Action SlidePressed;
        public event Action SlideReleased;

        public void SetMoveInput(float x, float y)
        {
            MoveInput = new Vector2(x, y);
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

        public void OnFireReleased()
        {
            FireReleased?.Invoke();
        }

        public void OnFirePressed()
        {
            FirePressed?.Invoke();
        }

        public void OnAltFirePressed()
        {
            AltFirePressed?.Invoke();
        }

        public void OnAltFireReleased()
        {
            AltFireReleased?.Invoke();
        }

        public void OnMeleePressed()
        {
            MeleePressed?.Invoke();
        }

        public void OnSlamPressed()
        {
            SlamPressed?.Invoke();
        }

        public void OnSlidePressed()
        {
            SlidePressed?.Invoke();
            IsSlidePressed = true;
        }

        public void OnSlideReleased()
        {
            SlideReleased?.Invoke();
            IsSlidePressed = false;
        }
    }
}
