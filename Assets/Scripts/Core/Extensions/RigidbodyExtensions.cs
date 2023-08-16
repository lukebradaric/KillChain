using UnityEngine;

namespace KillChain.Core.Extensions
{
    public static class RigidbodyExtensions
    {
        public static void SetVelocityY(this Rigidbody r, float y)
        {
            r.velocity = new Vector3(r.velocity.x, y, r.velocity.z);
        }
    }
}
