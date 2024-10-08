using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerJump : MonoBehaviour
{
    [Header("Components")] private Rigidbody2D _rigidbody;
    [SerializeField] private GroundChecker _onGroundChecker;
    private Vector2 _velocity;
    
    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 5.5f)]
    [Tooltip("Maximum jump height")]
    public float JumpHeight = 7.3f;

    [SerializeField, Range(0.2f, 1.25f)]
    [Tooltip("How long it takes to reach that height before coming back down")]
    public float TimeToJumpApex;

    [SerializeField, Range(0f, 5f)]
    [Tooltip("Gravity multiplier to apply when going up")]
    public float UpwardMovementMultiplier = 1f;

    [SerializeField, Range(1f, 10f)]
    [Tooltip("Gravity multiplier to apply when coming down")]
    public float DownwardMovementMultiplier = 6.17f;

    [SerializeField, Range(0, 1)]
    [Tooltip("How many times can you jump in the air?")]
    public int MaxAirJumps = 0;

    [Header("Options")]
    [Tooltip("Should the character drop when you let go of jump?")]
    public bool VariablejumpHeight;

    [SerializeField, Range(1f, 10f)]
    [Tooltip("Gravity multiplier when you let go of jump")]
    public float JumpCutOff;

    [SerializeField] [Tooltip("The fastest speed the character can fall")]
    public float SpeedLimit;

    [SerializeField, Range(0f, 0.3f)]
    [Tooltip("How long should coyote time last?")]
    public float CoyoteTime = 0.15f;

    [SerializeField, Range(0f, 0.3f)]
    [Tooltip("How far from ground should we cache your jump?")]
    public float JumpBuffer = 0.15f;

    [Header("Calculations")]
    public float JumpSpeed;

    private float _defaultGravityScale;

    public float GravMultiplier = 1f;

    private bool _desiredJump;
    private float _jumpBufferCounter;
    private int _jumpCounter = 0;
    private float _coyoteTimeCounter = 0;
    private bool _pressingJump;
    public bool OnGround;
    private bool _currentlyJumping;
    private bool _jumpCutOff;

    private float _currentGravityScale = 1f;

    private InputActions _inputActions;

    void Awake()
    {
        //Find the character's Rigidbody and ground detection and juice scripts

        _rigidbody = GetComponent<Rigidbody2D>();
        
        _defaultGravityScale = 1f;

        _inputActions = new InputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.Jump.started += OnJump;
        _inputActions.Player.Jump.canceled += OnJump;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //This function is called when one of the jump buttons (like space or the A button) is pressed.

        //When we press the jump button, tell the script that we desire a jump.
        //Also, use the started and canceled contexts to know if we're currently holding the button
        if (context.started)
        {
            _desiredJump = true;
            _pressingJump = true;
        }

        if (context.canceled)
        {
            _pressingJump = false;
        }
    }

    void Update()
    {
        //Check if we're on ground, using Kit's Ground script
        OnGround = _onGroundChecker.IsGrounded();

        //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
        if (JumpBuffer > 0)
        {
            //Instead of immediately turning off "desireJump", start counting up...
            //All the while, the DoAJump function will repeatedly be fired off
            if (_desiredJump)
            {
                _jumpBufferCounter += Time.deltaTime;

                if (_jumpBufferCounter > JumpBuffer)
                {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    _desiredJump = false;
                    _jumpBufferCounter = 0;
                }
            }
        }

        //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
        //So, start the coyote time counter...
        if (!_currentlyJumping && !OnGround)
        {
            _coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            //Reset it when we touch the ground, or jump
            _coyoteTimeCounter = 0;
        }
    }

    private void SetPhysics()
    {
        //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
        float newGravity = (2 * JumpHeight) / (TimeToJumpApex * TimeToJumpApex);

        float newGscale = (newGravity/ Physics2D.gravity.magnitude) * GravMultiplier;
        if (Math.Abs(newGscale - _currentGravityScale) > 0.001)
        {
            _rigidbody.gravityScale = newGscale;
            if (!_jumpCutOff) //preserve momentum 
            {
                var rate = _currentGravityScale / newGscale;
                var rbLocalVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
                rbLocalVelocity = new Vector2(rbLocalVelocity.x, rbLocalVelocity.y / Mathf.Sqrt(rate));

                _rigidbody.velocity = transform.TransformDirection(rbLocalVelocity);
            }
            _currentGravityScale = newGscale;
        }
    }

    private void FixedUpdate()
    {
        SetPhysics();
        //Get velocity from Kit's Rigidbody 
        _velocity = transform.InverseTransformDirection(_rigidbody.velocity);
        _jumpedThisFrame = false;
        //Keep trying to do a jump, for as long as desiredJump is true
        if (_desiredJump)
        {
            DoAJump();
            _rigidbody.velocity = transform.TransformDirection(_velocity);
        }

        CalculateGravityModifier();
    }

    private void CalculateGravityModifier()
    {
        //We change the character's gravity based on her Y direction
        _velocity = transform.InverseTransformDirection(_rigidbody.velocity);

        //If Kit is going up...
        _jumpCutOff = false;
        if (_velocity.y > 0.01f)
        {
            if (OnGround)
            {
                //Don't change it if Kit is stood on something (such as a moving platform)
                GravMultiplier = _defaultGravityScale;
            }
            else
            {
                //If we're using variable jump height...)
                if (VariablejumpHeight)
                {
                    //Apply upward multiplier if player is rising and holding jump
                    if (_pressingJump && _currentlyJumping)
                    {
                        GravMultiplier = UpwardMovementMultiplier;
                    }
                    //But apply a special downward multiplier if the player lets go of jump
                    else
                    {
                        GravMultiplier = JumpCutOff;
                        _jumpCutOff = true;
                    }
                }
                else
                {
                    GravMultiplier = UpwardMovementMultiplier;
                }
            }
        }

        //Else if going down...
        else if (_velocity.y < -0.01f)
        {
            if (OnGround)
                //Don't change it if Kit is stood on something (such as a moving platform)
            {
                GravMultiplier = _defaultGravityScale;
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                GravMultiplier = DownwardMovementMultiplier;
            }
        }
        //Else not moving vertically at all
        else
        {
            if (OnGround && !_jumpedThisFrame)
            {
                _currentlyJumping = false;
                _jumpCounter = 0;
            }

            GravMultiplier = _defaultGravityScale;
        }

        //Set the character's Rigidbody's velocity
        //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
        var clampedVelocity = new Vector3(_velocity.x, Mathf.Clamp(_velocity.y, -SpeedLimit, 100));
        _rigidbody.velocity = transform.TransformDirection(clampedVelocity);
    }

    private bool _jumpedThisFrame;

    private void DoAJump()
    {
        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (OnGround || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < CoyoteTime) || CanJumpAgain())
        {
            _desiredJump = false;
            _jumpBufferCounter = 0;
            _coyoteTimeCounter = 0;
            _jumpedThisFrame = true;
            _jumpCounter++;
            //If we have double jump on, allow us to jump again (but only once)

            //Determine the power of the jump, based on our gravity and stats
            JumpSpeed = Mathf.Sqrt(2f * Physics2D.gravity.magnitude * _rigidbody.gravityScale * JumpHeight);

            //If Kit is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
            //This will ensure the jump is the exact same strength, no matter your velocity.
            if (_velocity.y > 0f)
            {
                JumpSpeed = Mathf.Max(JumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                JumpSpeed += Mathf.Abs(_velocity.y);
            }

            //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
            _velocity.y += JumpSpeed;
            _currentlyJumping = true;
        }

        if (JumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            _desiredJump = false;
        }
    }

    bool CanJumpAgain()
    {
        return _jumpCounter <= MaxAirJumps;
    }

    public void BounceUp(float bounceAmount)
    {
        //Used by the springy pad
        _rigidbody.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }
}