using DG.Tweening;
using KillChain.Core.Generics;

namespace KillChain.Camera
{
    public class CameraShaker : Singleton<CameraShaker>
    {
        public void Shake(CameraShakeData cameraShakeData)
        {
            Shake(
                cameraShakeData.Duration,
                cameraShakeData.Strength,
                cameraShakeData.Vibrato,
                cameraShakeData.Randomness,
                cameraShakeData.FadeOut
                );
        }

        public void Shake(float duration, float strength, int vibrato, float randomness, bool fadeOut)
        {
            transform.DOShakeRotation(
                duration,
                strength,
                vibrato,
                randomness,
                fadeOut
                );
        }

        private void OnDestroy()
        {
            DOTween.Clear(true);
        }
    }
}
