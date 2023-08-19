using System;
using UnityEngine;

namespace KillChain.Core
{
    public interface IChainTarget
    {
        public bool IsDashable { get; }
        public bool IsPullable { get; }

        public Transform Transform { get; }

        public void StartPull(Transform transform, float speed);
        public void StopPull();
    }
}

