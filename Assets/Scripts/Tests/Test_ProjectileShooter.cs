using UnityEngine;

namespace KillChain.Tests
{
    public class Test_ProjectileShooter : MonoBehaviour
    {
        //
        public GameObject projectilePrefab;
        public Transform firePointTransform;
        public float projectileSpeed;

        private void Start()
        {
            InvokeRepeating(nameof(ShootProjectile), 1, 3f);
        }

        private void ShootProjectile()
        {
            GameObject go = Instantiate(projectilePrefab, firePointTransform.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
        }
    }
}

