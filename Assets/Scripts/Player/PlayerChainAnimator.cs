using KillChain.Core;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerChainAnimator : PlayerMonoBehaviour
    {
        [Space]
        [Header("Settings")]
        [SerializeField] private int _quality;
        [SerializeField] private float _damper;
        [SerializeField] private float _strength;
        [SerializeField] private float _velocity;
        [SerializeField] private float _waveCount;
        [SerializeField] private float _waveHeight;
        [SerializeField] private float _duration;
        [SerializeField] private float _speed;
        [SerializeField] private AnimationCurve _affectCurve;

        private Spring _spring = new Spring();
        private Vector3 _currentGrapplePosition;

        private void Awake()
        {
            _spring.SetTarget(0);
        }

        private void LateUpdate()
        {
            if (_player.Chain.ChainTarget == null)
            {
                _currentGrapplePosition = _player.ChainStartTransform.position;
                _spring.Reset();
                if (_player.ChainLineRenderer.positionCount > 0)
                {
                    _player.ChainLineRenderer.positionCount = 0;
                }
                return;
            }

            if (_player.ChainLineRenderer.positionCount == 0)
            {
                _spring.SetVelocity(_velocity);
                _player.ChainLineRenderer.positionCount = _quality + 1;
            }

            _spring.SetDamper(_damper);
            _spring.SetStrength(_strength);
            _spring.Update(Time.deltaTime / _duration);

            Vector3 _currentChainablePosition = _player.Chain.ChainTarget.Transform.position;
            Vector3 _chainStartPosition = _player.ChainStartTransform.position;
            Vector3 up = Quaternion.LookRotation((_currentChainablePosition - _chainStartPosition).normalized) * Vector3.up;

            _currentGrapplePosition = Vector3.Lerp(_currentGrapplePosition, _currentChainablePosition, Time.deltaTime * _speed);

            for (int i = 0; i < _quality + 1; i++)
            {
                float delta = i / (float)_quality;
                var offset = up * _waveHeight * Mathf.Sin(delta * _waveCount * Mathf.PI) * _spring.Value * _affectCurve.Evaluate(delta);

                _player.ChainLineRenderer.SetPosition(i, Vector3.Lerp(_chainStartPosition, _currentGrapplePosition, delta) + offset);
            }
        }
    }
}

