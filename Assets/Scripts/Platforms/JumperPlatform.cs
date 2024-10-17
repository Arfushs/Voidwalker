using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public class JumperPlatform : MonoBehaviour, IDimensional
{
    [field: Header("Platform Infos")]
    [field: Space]
    [field: SerializeField] public DimensionType DimensionType { get; private set; }
    [SerializeField] protected Transform _visual;
    [SerializeField] protected Transform _visualHolo;
    [SerializeField] private Vector2 _jumpForce = new Vector2(0, 50f);  // Adjustable jump force
    [SerializeField] private float _animationScalePercent = 0.5f;  // How much to scale during the animation

    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    private Vector3 _originalScale;  // To store the original scale

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = _visual.GetComponent<Animator>();
        _originalScale = _visual.localScale;  // Store the original scale
    }

    private void OnEnable()
    {
        AnimationFinished.OnAnimationFinished += PlayEmpty;
    }

    private void OnDisable()
    {
        AnimationFinished.OnAnimationFinished -= PlayEmpty;
    }

    public void Show()
    {
        if (DimensionType == DimensionType.Both)
            return;
        _boxCollider2D.enabled = true;
        _visual.gameObject.SetActive(true);
        _visualHolo.gameObject.SetActive(false);
    }

    public void Hide()
    {
        if (DimensionType == DimensionType.Both)
            return;
        _boxCollider2D.enabled = false;
        _visual.gameObject.SetActive(false);
        _visualHolo.gameObject.SetActive(true);
    }

    public DimensionType GetDimensionType()
    {
        return DimensionType;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x + _jumpForce.x, _jumpForce.y);
                
                // Scale the platform based on _animationScalePercent
                ScalePlatform();
                
                // Play different animations based on the DimensionType
                if (DimensionType == DimensionType.Default)
                {
                    _animator.Play("jumperFire");
                }
                else if (DimensionType == DimensionType.Alter)
                {
                    _animator.Play("jumperAlterFire");
                }
                else if (DimensionType == DimensionType.Both)
                {
                    _animator.Play("jumperBothFire");
                }
            }
        }
    }

    private void ScalePlatform()
    {
        // Increase the scale of the _visual based on _animationScalePercent
        _visual.localScale = _originalScale * (1 + _animationScalePercent);
    }

    private void ResetScale()
    {
        // Reset the scale back to the original after the animation finishes
        _visual.localScale = _originalScale;
    }

    private void PlayEmpty(string s)
    {
        if (s == "jumperJumped")
        {
            _animator.Play("Empty");
            ResetScale();  // Reset the scale after the animation is finished
        }
    }
    
    private void OnDrawGizmos()
    {
        // Calculate the maximum height based on jumpForce
        float maxJumpHeight = _jumpForce.y * 0.1f;  // Adjust scaling if needed

        // Draw a line showing where the player would reach
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxJumpHeight);

        // Optionally, draw a sphere at the top to show the maximum height
        Gizmos.DrawSphere(transform.position + Vector3.up * maxJumpHeight, 0.1f);
    }
}