using UnityEngine;
using UnityEngine.InputSystem;
using GemRush.Core;

namespace GemRush.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private InputActionReference moveAction;

        private Rigidbody _rb;
        private Vector2 _moveInput;
        private bool _inputEnabled = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
            MatchTimer.OnMatchEnded += DisableInput;
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
            MatchTimer.OnMatchEnded -= DisableInput;
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
            _rb.linearVelocity = velocity;
            ClampToArena();
        }

        private void ClampToArena()
        {
            Vector3 pos = _rb.position;
            pos.x = Mathf.Clamp(pos.x, -config.arenaHalfExtents.x, config.arenaHalfExtents.x);
            pos.z = Mathf.Clamp(pos.z, -config.arenaHalfExtents.y, config.arenaHalfExtents.y);
            _rb.position = pos;
        }

        private void DisableInput() => _inputEnabled = false;
    }
}