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

        [Tooltip("Downwards force constantly applied to player while on ground.")]
        [SerializeField] private float _downwardsForce;
        public float DownwardsForce => _downwardsForce;

        [Tooltip("Minimum angle of ground before angled forces are applied.")]
        [SerializeField] private float _minGroundAngle;
        public float MinGroundAngle => _minGroundAngle;

        [Tooltip("Max angle of ground where movement is allowed.")]
        [SerializeField] private float _maxGroundAngle;
        public float MaxGroundAngle => _maxGroundAngle;
        #endregion

        #region Jump
        [Space]
        [Header("Jump")]
        [Tooltip("Force applied upwards when jumping.")]
        [SerializeField] private float _jumpForce;
        public float JumpForce => _jumpForce;

        [Tooltip("The time a jump will still trigger after the player has pressed jump.")]
        [SerializeField] private float _jumpBufferTime;
        public float JumpBufferTime => _jumpBufferTime;

        [Tooltip("The time after jumping the player must wait to be grounded again.")]
        [SerializeField] private float _jumpWaitTime;
        public float JumpWaitTime => _jumpWaitTime;
        #endregion

        #region Air
        [Space]
        [Header("Air")]
        [Tooltip("Movement speed multiplier while in air.")]
        [SerializeField] private float _airSpeedMultiplier;
        public float AirSpeedMultiplier => _airSpeedMultiplier;

        [Tooltip("Force applied while falling.")]
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

        [SerializeField] private float _slideDownwardsForce;
        public float SlideDownwardsForce => _slideDownwardsForce;
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

        [Tooltip("Size of the slam HitBox when impacting the ground.")]
        [SerializeField] private Vector3 _slamGroundHitBoxSize;
        public Vector3 SlamGroundHitBoxSize => _slamGroundHitBoxSize;

        [Tooltip("Size of the slam HitBox while slamming down.")]
        [SerializeField] private Vector3 _slamAirHitBoxSize;
        public Vector3 SlamAirHitBoxSize => _slamAirHitBoxSize;

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

        [Tooltip("Value multiplied by player velocity when going through boost")]
        [SerializeField] private float _dashBoostSpeedMultiplier;
        public float DashBoostSpeedMultiplier => _dashBoostSpeedMultiplier;

        [Tooltip("Upwards force applied to the player when releasing from a dash.")]
        [SerializeField] private float _dashReleaseUpwardForce;
        public float DashReleaseUpwardForce => _dashReleaseUpwardForce;

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

        [Tooltip("Length of melee capsule HitBox.")]
        [SerializeField] private float _meleeLength;
        public float MeleeLength => _meleeLength;

        [Tooltip("Radius of melee capsule HitBox.")]
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
