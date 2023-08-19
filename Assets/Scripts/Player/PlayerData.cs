using UnityEngine;

namespace KillChain.Player
{
    [CreateAssetMenu(menuName = "KillChain/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        #region Movement
        [Space]
        [Header("Movement")]
        [Tooltip("Speed of basic movement.")]
        [SerializeField] private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;

        [Tooltip("Force applied upwards when jumping.")]
        [SerializeField] private float _jumpForce;
        public float JumpForce => _jumpForce;

        [Tooltip("Value multiplied by player velocity when going through boost")]
        [SerializeField] private float _boostSpeedMultiplier;
        public float BoostSpeedMultiplier => _boostSpeedMultiplier;

        [Tooltip("Movement speed multiplier while in air.")]
        [SerializeField] private float _airSpeedMultiplier;
        public float AirSpeedMultiplier => _airSpeedMultiplier;

        [SerializeField] private float _fallForce;
        public float FallForce => _fallForce;
        #endregion

        #region Drag
        [Space]
        [Header("Drag")]
        [Tooltip("Rigidbody drag while grounded.")]
        [SerializeField] private float _groundDrag;
        public float GroundDrag => _groundDrag;

        [Tooltip("Rigidbody drag while in air.")]
        [SerializeField] private float _airDrag;
        public float AirDrag => _airDrag;
        #endregion

        #region Camera
        [Space]
        [Header("Camera")]
        [Tooltip("Camera horizontal sensitivity.")]
        [SerializeField] private float _horizontalSensitivity;
        public float HorizontalSensitivity => _horizontalSensitivity;

        [Tooltip("Camera vertical sesnitivity.")]
        [SerializeField] private float _verticalSensitivity;
        public float VerticalSensitivity => _verticalSensitivity;
        #endregion

        #region Slide
        [Space]
        [Header("Slide")]
        [SerializeField] private float _slideSpeed;
        public float SlideSpeed => _slideSpeed;
        #endregion

        #region Slam
        [Space]
        [Header("Slam")]
        [Tooltip("Damage dealt by slam ability.")]
        [SerializeField] private int _slamDamage;
        public int SlamDamage => _slamDamage;

        [Tooltip("Speed player will move downwards when slamming.")]
        [SerializeField] private float _slamSpeed;
        public float SlamSpeed => _slamSpeed;

        [Tooltip("Size of the slam hitbox when impacting the ground.")]
        [SerializeField] private Vector3 _slamHitboxSize;
        public Vector3 SlamHitboxSize => _slamHitboxSize;
        #endregion

        #region Chain
        [Space]
        [Header("Chain")]
        [Tooltip("Max distance player can chain to target from.")]
        [SerializeField] private float _maxChainDistance;
        public float MaxChainDistance => _maxChainDistance;

        [Tooltip("The max time the chain takes to attach to a target.")]
        [SerializeField] private float _maxChainDelayTime;
        public float MaxChainDelayTime => _maxChainDelayTime;
        #endregion

        #region Dash
        [Space]
        [Header("Dash")]
        [Tooltip("Damage of dash ability.")]
        [SerializeField] private int _dashDamage;
        public int DashDamage => _dashDamage;

        [Tooltip("Speed while dashing to chained target.")]
        [SerializeField] private float _dashSpeed;
        public float DashSpeed => _dashSpeed;

        [Tooltip("Speed applied to player after reaching dash target. (Non-damageables)")]
        [SerializeField] private float _dashReboundSpeed;
        public float DashReboundSpeed => _dashReboundSpeed;

        [Tooltip("Upwards force applied to player after reaching dash target. (Non-damageables)")]
        [SerializeField] private float _dashReboundUpwardForce;
        public float DashReboundUpwardForce => _dashReboundUpwardForce;

        [Tooltip("Value multiplied by player velocity when doing dash damage.")]
        [SerializeField] private float _dashDamageSpeedMultiplier;
        public float DashDamageSpeedMultiplier => _dashDamageSpeedMultiplier;

        [Tooltip("Upwards force applied to player aftering damaging chained target with dash.")]
        [SerializeField] private float _dashDamageUpwardForce;
        public float DashDamageUpwardForce => _dashDamageUpwardForce;

        [Tooltip("Distance dash will damaged chained target from.")]
        [SerializeField] private float _dashStopDistance;
        public float DashStopDistance => _dashStopDistance;
        #endregion

        #region Pull
        [Space]
        [Header("Pull")]
        [Tooltip("Speed chained targets are pulled.")]
        [SerializeField] private float _pullSpeed;
        public float PullSpeed => _pullSpeed;

        [Tooltip("Distance where chained target will stop being pulled.")]
        [SerializeField] private float _pullStopDistance;
        public float PullStopDistance => _pullStopDistance;
        #endregion

        #region Melee
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
        #endregion

        #region Parry
        [Space]
        [Header("Parry")]
        [SerializeField] private float _parryTimeStopDuration;
        public float ParryTimeStopDuration => _parryTimeStopDuration;

        [SerializeField] private float _parryVelocityMultiplier;
        public float ParryVelocityMultiplier => _parryVelocityMultiplier;
        #endregion
    }
}
