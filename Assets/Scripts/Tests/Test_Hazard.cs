using KillChain.Core;
using UnityEngine;

namespace KillChain.Tests
{
    public class Test_Hazard : MonoBehaviour
    {
        public int damage;

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(damage);
            }
        }
    }
}

