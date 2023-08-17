using UnityEngine;

namespace KillChain.Core
{
    public interface IPullable : IChainable
    {
        public void Pull(Transform transform, float pullSpeed);
        public void Stop();
    }
}

