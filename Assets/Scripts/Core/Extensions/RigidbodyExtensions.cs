using UnityEngine;

namespace KillChain.Core.Extensions
{
    public static class RigidbodyExtensions
    {
        public static void SetVelocityY(this Rigidbody r, float y)
        {
            r.velocity = new Vector3(r.velocity.x, y, r.velocity.z);
        }

        public static void SetVelocity(this Rigidbody r, float x, float y, float z)
        {
            r.velocity = new Vector3(x, y, z);
        }

        public static void SetVelocity(this Rigidbody r, Vector3 vector3)
        {
            r.velocity = vector3;
        }
    }
}
