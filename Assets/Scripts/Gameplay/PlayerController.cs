using UnityEngine;
using UnityEngine.InputSystem;
using GemRush.Core;
using GemRush.Core.Events;

namespace GemRush.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private InputActionReference moveAction;

        [Header("Listens To")]
        [SerializeField] private MatchStateEventChannelSO stateChannel;

        private Rigidbody _rb;
        private Vector2 _moveInput;
        private bool _inputEnabled = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
            stateChannel.Subscribe(HandleStateChanged);
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
            stateChannel.Unsubscribe(HandleStateChanged);
        }

        private void HandleStateChanged(MatchState state)
        {
            _inputEnabled = state == MatchState.Playing;
        }

        private void Update()
        {
            _moveInput = _inputEnabled
                ? moveAction.action.ReadValue<Vector2>()
                : Vector2.zero;
        }

        private void FixedUpdate()
        {
            Vector3 velocity = new(_moveInput.x * config.moveSpeed, _rb.linearVelocity.y, _moveInput.y * config.moveSpeed);
            _rb.linearVelocity = ClampVelocityToArena(velocity);

        }

        private Vector3 ClampVelocityToArena(Vector3 velocity)
        {
            Vector3 pos = _rb.position;
            Vector2 bounds = config.arenaHalfExtents;

            if ((pos.x >= bounds.x && velocity.x > 0f) || (pos.x <= -bounds.x && velocity.x < 0f))
                velocity.x = 0f;

            if ((pos.z >= bounds.y && velocity.z > 0f) || (pos.z <= -bounds.y && velocity.z < 0f))
                velocity.z = 0f;

            return velocity;
        }



        private void DisableInput() => _inputEnabled = false;
    }
}