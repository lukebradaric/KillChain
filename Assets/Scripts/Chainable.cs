using UnityEngine;
using KillChain.Core;

namespace KillChain
{
    public class Chainable : MonoBehaviour, IChainable
    {
        Transform IChainable.Transform { get => transform; }
    }
}

