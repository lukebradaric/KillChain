using UnityEngine;
using KillChain.Core;

namespace KillChain.Common
{
    public class Chainable : MonoBehaviour, IChainable
    {
        Transform IChainable.Transform { get => transform; }
    }
}

