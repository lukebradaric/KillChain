using KillChain.Audio;
using System;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [Space]
        [Header("Audio")]
        [SerializeField] private AudioAsset _chainAttachAudioAsset;
        [SerializeField] private AudioAsset _chainBreakAudioAsset;
        [SerializeField] private AudioAsset _meleeAudioAsset;

        private void OnEnable()
        {
            PlayerWeapon.State.ValueChanged += StateChangedHandler;
            PlayerWeapon.ChainBroke += ChainBrokeHandler;
            PlayerMelee.MeleeStarted += MeleeStartedHandler;

        }

        private void OnDisable()
        {
            PlayerWeapon.State.ValueChanged -= StateChangedHandler;
            PlayerWeapon.ChainBroke -= ChainBrokeHandler;
            PlayerMelee.MeleeStarted -= MeleeStartedHandler;
        }

        private void StateChangedHandler(PlayerWeaponState state)
        {
            if (state == PlayerWeaponState.Attach) _chainAttachAudioAsset?.Play();
        }

        private void ChainBrokeHandler()
        {
            _chainBreakAudioAsset?.Play();
        }

        private void MeleeStartedHandler()
        {
            _meleeAudioAsset?.Play();
        }
    }
}

