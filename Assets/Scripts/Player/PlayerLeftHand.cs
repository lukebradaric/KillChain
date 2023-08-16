using UnityEngine;
using DG.Tweening;

namespace KillChain.Player
{
    public class PlayerLeftHand : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private RectTransform _leftHandTransform;

        [Space]
        [Header("Settings")]
        [SerializeField] private Vector3 _punchPosition;
        [SerializeField] private Ease _punchEase;
        [SerializeField] private Vector3 _punchReturnPosition;
        [SerializeField] private float _punchReturnTime;
        [SerializeField] private Ease _punchReturnEase;

        private void OnEnable()
        {
            PlayerMelee.Meleed += MeleedHandler;
        }

        private void OnDisable()
        {
            PlayerMelee.Meleed -= MeleedHandler;
        }

        private void MeleedHandler()
        {
            _leftHandTransform.localPosition = _punchPosition;
            _leftHandTransform.DOLocalMove(_punchReturnPosition, _punchReturnTime).SetEase(_punchReturnEase);
        }
    }
}
