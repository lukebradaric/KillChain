using System;
using UnityEngine;

namespace KillChain.Core
{
    public interface IChainTarget
    {
        public bool IsDashable { get; }
        public bool IsPullable { get; }
        public bool IsBoostable { get; }
        public bool InterruptMovement { get; }

        public Transform Transform { get; }

        public event Action Destroyed;

        public void StartPull(Transform transform, float speed);
        public void StopPull();
    }
}

