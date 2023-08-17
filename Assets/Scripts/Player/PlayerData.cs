using UnityEngine;

namespace KillChain.Player
{
    [CreateAssetMenu(menuName = "KillChain/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Space]
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;

        [SerializeField] private float _maxSpeed;
        public float MaxSpeed => _maxSpeed;

        [SerializeField] private float _maxAirSpeed;
        public float MaxAirSpeed => _maxAirSpeed;

        [SerializeField] private float _jumpForce;
        public float JumpForce => _jumpForce;

        [SerializeField] private float _airSpeedMultiplier;
        public float AirSpeedMultiplier => _airSpeedMultiplier;

        [Space]
        [Header("Drag")]
        [SerializeField] private float _groundDrag;
        public float GroundDrag => _groundDrag;

        [SerializeField] private float _airDrag;
        public float AirDrag => _airDrag;

        [Space]
        [Header("Camera")]
        [SerializeField] private float _horizontalSensitivity;
        public float HorizontalSensitivity => _horizontalSensitivity;

        [SerializeField] private float _verticalSensitivity;
        public float VerticalSensitivity => _verticalSensitivity;

        [Space]
        [Header("Weapon")]
        [SerializeField] private float _maxTargetDistance;
        public float MaxTargetDistance => _maxTargetDistance;

        [Space]
        [Header("Dash")]
        [SerializeField] private int _dashDamage;
        public int DashDamage => _dashDamage;

        [SerializeField] private float _dashSpeed;
        public float DashSpeed => _dashSpeed;

        [SerializeField] private float _dashKillUpwardForce;
        public float DashKillUpwardForce => _dashKillUpwardForce;

        [SerializeField] private float _dashKillDistance;
        public float DashKillDistance => _dashKillDistance;

        [Space]
        [Header("Pull")]
        [SerializeField] private float _pullSpeed;
        public float PullSpeed => _pullSpeed;

        [SerializeField] private float _pullStopDistance;
        public float PullStopDistance => _pullStopDistance;
    }
}
