using KillChain.Core;
using KillChain.Core.StateMachine;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public abstract class PlayerChainState : State
    {
        public void SetPlayer(Player player) => _player = player;
        protected Player _player;

        new protected PlayerChainStateMachine _stateMachine => (PlayerChainStateMachine)base._stateMachine;

        protected virtual IChainTarget GetLookTarget()
        {
            // If hit nothing, return null
            if (!Physics.Raycast(_player.CameraTransform.position, _player.CameraTransform.forward, out RaycastHit raycastHit, _player.Data.MaxChainDistance, _player.Data.ChainTargetLayerMask))
            {
                return null;
            }

            // If hit collider does not have chain target, return null
            if (!raycastHit.collider.TryGetComponent<IChainTarget>(out IChainTarget chainTarget))
            {
                return null;
            }

            // Return chain target
            return chainTarget;
        }

        protected virtual bool IsTargetInLineOfSight(IChainTarget chainTarget)
        {
            Physics.Raycast(_player.CameraTransform.position,
                (chainTarget.Transform.position - _player.CameraTransform.position).normalized,
                out RaycastHit raycastHit,
                Vector3.Distance(_player.CameraTransform.position, chainTarget.Transform.position),
                _player.Data.ChainBreakLayerMask);

            // If we hit nothing, or we hit our target, return true
            return raycastHit.collider == null || raycastHit.transform == chainTarget.Transform;
        }
    }
}

