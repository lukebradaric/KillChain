using KillChain.Core.Managers;
using KillChain.Input;
using KillChain.Player.States;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace KillChain.Player
{
    public class Player : SerializedMonoBehaviour
    {
        [PropertySpace]
        [Title("Managers")]
        [OdinSerialize] public TimeManager TimeManager { get; private set; }

        [PropertySpace]
        [Title("Settings")]
        [OdinSerialize] public PlayerData Data { get; private set; }
        [OdinSerialize] public GameInput GameInput { get; private set; }

        [PropertySpace]
        [Title("Components")]
        [OdinSerialize] public PlayerStateMachine StateMachine { get; private set; }
        [OdinSerialize] public PlayerChainStateMachine ChainStateMachine { get; private set; }
        [OdinSerialize] public PlayerChain Chain { get; private set; }
        [OdinSerialize] public PlayerMelee Melee { get; private set; }
        [OdinSerialize] public PlayerGroundCheck GroundCheck { get; private set; }
        [OdinSerialize] public PlayerJumpBuffer JumpBuffer { get; private set; }
        [OdinSerialize] public PlayerAudio Audio { get; private set; }

        [PropertySpace]
        [Title("Unity Components")]
        [OdinSerialize] public Rigidbody Rigidbody { get; private set; }
        [OdinSerialize] public Transform LookTransform { get; private set; }
        [OdinSerialize] public Transform CameraTransform { get; private set; }
        [OdinSerialize] public Transform MeleeHitBoxTransform { get; private set; }
        [OdinSerialize] public Transform SlamHitBoxTransform { get; private set; }
        [OdinSerialize] public Transform ChainStartTransform { get; private set; }
        [OdinSerialize] public LineRenderer ChainLineRenderer { get; private set; }
    }
}
