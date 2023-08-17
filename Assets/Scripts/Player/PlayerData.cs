using UnityEngine;

namespace KillChain.Player
{
    [CreateAssetMenu(menuName = "KillChain/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Space]
        [Header("Movement")]
        [Tooltip("Speed of basic movement.")]
        [SerializeField] private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;

        [Tooltip("Max speed player can reach.")]
        [SerializeField] private float _maxSpeed;
        public float MaxSpeed => _maxSpeed;

        [Tooltip("Max speed player can reach while in air.")]
        [SerializeField] private float _maxAirSpeed;
        public float MaxAirSpeed => _maxAirSpeed;

        [Tooltip("Force applied upwards when jumping.")]
        [SerializeField] private float _jumpForce;
        public float JumpForce => _jumpForce;

        [Tooltip("Movement speed multiplier while in air.")]
        [SerializeField] private float _airSpeedMultiplier;
        public float AirSpeedMultiplier => _airSpeedMultiplier;

        [Space]
        [Header("Drag")]
        [Tooltip("Rigidbody drag while grounded.")]
        [SerializeField] private float _groundDrag;
        public float GroundDrag => _groundDrag;

        [Tooltip("Rigidbody drag while in air.")]
        [SerializeField] private float _airDrag;
        public float AirDrag => _airDrag;

        [Space]
        [Header("Camera")]
        [Tooltip("Camera horizontal sensitivity.")]
        [SerializeField] private float _horizontalSensitivity;
        public float HorizontalSensitivity => _horizontalSensitivity;

        [Tooltip("Camera vertical sesnitivity.")]
        [SerializeField] private float _verticalSensitivity;
        public float VerticalSensitivity => _verticalSensitivity;

        [Space]
        [Header("Weapon")]
        [Tooltip("Max distance player can chain to target from.")]
        [SerializeField] private float _maxTargetDistance;
        public float MaxTargetDistance => _maxTargetDistance;

        [Space]
        [Header("Dash")]
        [Tooltip("Damage of dash ability.")]
        [SerializeField] private int _dashDamage;
        public int DashDamage => _dashDamage;

        [Tooltip("Speed while dashing to chained target.")]
        [SerializeField] private float _dashSpeed;
        public float DashSpeed => _dashSpeed;

        [Tooltip("Force applied upwards aftering killing chained target with dash.")]
        [SerializeField] private float _dashKillUpwardForce;
        public float DashKillUpwardForce => _dashKillUpwardForce;

        [Tooltip("Distance dash will kill chained enemy from.")]
        [SerializeField] private float _dashKillDistance;
        public float DashKillDistance => _dashKillDistance;

        [Space]
        [Header("Pull")]
        [Tooltip("Speed chained targets are pulled.")]
        [SerializeField] private float _pullSpeed;
        public float PullSpeed => _pullSpeed;

        [Tooltip("Distance where chained target will stop being pulled.")]
        [SerializeField] private float _pullStopDistance;
        public float PullStopDistance => _pullStopDistance;

        [Space]
        [Header("Melee")]
        [Tooltip("Damage of melee ability.")]
        [SerializeField] private int _meleeDamage;
        public int MeleeDamage => _meleeDamage;

        [Tooltip("Cooldown of melee ability.")]
        [SerializeField] private float _meleeCooldown;
        public float MeleeCooldown => _meleeCooldown;

        [Tooltip("Length of melee capsule hitbox.")]
        [SerializeField] private float _meleeLength;
        public float MeleeLength => _meleeLength;

        [Tooltip("Radius of melee capsule hitbox.")]
        [SerializeField] private float _meleeRadius;
        public float MeleeRadius => _meleeRadius;
    }
}
