using System;

namespace KillChain.Core
{
    public interface IDestroyable
    {
        public event Action Destroyed;

        public void Destroy();
    }
}

