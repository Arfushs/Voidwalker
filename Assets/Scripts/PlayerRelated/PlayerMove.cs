using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed = 10f;

    [SerializeField, Range(0f, 100f), Tooltip("How fast to reach max speed")]
    private float _maxAcceleration = 52f;

    [SerializeField, Range(0f, 100f), Tooltip("How fast to stop after letting go")]
    private float _maxDeceleration = 52f;

    [SerializeField, Range(0f, 100f), Tooltip("How fast to stop when changing direction")]
    private float _maxTurnSpeed = 80f;

    [SerializeField, Range(0f, 100f), Tooltip("How fast to reach max speed when in mid-air")]
    private float _maxAirAcceleration = 30f;

    [SerializeField, Range(0f, 100f), Tooltip("How fast to stop in mid-air when no direction is used")]
    private float _maxAirDeceleration = 30f;

    [SerializeField, Range(0f, 100f), Tooltip("How fast to stop when changing direction when in mid-air")]
    private float _maxAirTurnSpeed = 60f;

    [SerializeField, Tooltip("Friction to apply against movement on stick")]
    private float _friction = 0.2f;

    [Header("Options")]
    [SerializeField] private bool _useAcceleration = true; // If true, use acceleration/deceleration, if false, use instant speed.

    private float _directionX; // Input direction (-1 for left, 1 for right, 0 for no input)
    private Vector2 _velocity;
    private float _maxSpeedChange;

    [Header("Components")]
    private Rigidbody2D _rb;
    [SerializeField] private GroundChecker _groundChecker;

    private InputActions _inputActions;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _inputActions = new InputActions();
        
        
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Movement.performed += OnMove;
        _inputActions.Player.Movement.canceled += OnMove;
    }

    private void OnDisable()
    {
        // Cleanup input actions to avoid memory leaks
        _inputActions.Player.Movement.performed -= OnMove;
        _inputActions.Player.Movement.canceled -= OnMove;
        _inputActions.Disable();
        _directionX = 0;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _directionX = context.ReadValue<float>(); // Get the movement input (-1, 0, 1)
    }

    private void Update()
    {
        Vector3 currentScale = transform.localScale; // Mevcut scale'i kaydet
        if (_directionX != 0) // Hareket varsa
        {
            // Eğer _directionX pozitifse scale pozitif, negatifse scale negatif olacak şekilde ayarla
            currentScale.x = Mathf.Abs(currentScale.x) * (_directionX > 0 ? 1 : -1);

            // Scale'i güncelle
            transform.localScale = currentScale;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        bool isGrounded = _groundChecker.IsGrounded();
        float targetSpeed = _directionX * _maxSpeed; // Apply directionX to target speed

        float acceleration = isGrounded ? _maxAcceleration : _maxAirAcceleration;
        float deceleration = isGrounded ? _maxDeceleration : _maxAirDeceleration;
        float turnSpeed = isGrounded ? _maxTurnSpeed : _maxAirTurnSpeed;

        // If moving in opposite direction, use turn speed for deceleration
        if (!Mathf.Approximately(Mathf.Sign(targetSpeed), Mathf.Sign(_rb.velocity.x)))
        {
            acceleration = turnSpeed;
        }
        
        _maxSpeedChange = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        if (_useAcceleration)
        {
            // Apply friction when grounded and not moving
            if (isGrounded && Mathf.Abs(_directionX) < 0.01f)
            {
                _maxSpeedChange *= (1 - _friction);
            }

            // Smooth movement with acceleration/deceleration
            _velocity.x = Mathf.MoveTowards(_rb.velocity.x, targetSpeed, _maxSpeedChange * Time.fixedDeltaTime);
        }
        else
        {
            // Instant movement without acceleration (directly set the target speed)
            _velocity.x = targetSpeed;
        }

        _rb.velocity = new Vector2(_velocity.x, _rb.velocity.y);
    }
}