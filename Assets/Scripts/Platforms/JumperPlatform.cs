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
    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _visualHolo;
    [SerializeField] private SpriteRenderer _visualSpriteRenderer;
    [SerializeField] private SpriteRenderer _visualHoloSpriteRenderer;
    [SerializeField] private AudioSource _holoAudioSource;
    
    [SerializeField] private Vector2 _jumpForce = new Vector2(0, 50f);  // Adjustable jump force
    [SerializeField] private float _animationScalePercent = 0.5f;  // How much to scale during the animation

    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    private Vector3 _originalScale;  // To store the original scale
    private AudioSource _jumperAudioSource;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = _visual.GetComponent<Animator>();
        _originalScale = _visual.localScale;  // Store the original scale
        _jumperAudioSource = GetComponent<AudioSource>();
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
        _visualSpriteRenderer.enabled = true;
        _visualHoloSpriteRenderer.enabled = false;
        _holoAudioSource.enabled = false;
    }

    public void Hide()
    {
        if (DimensionType == DimensionType.Both)
            return;
        _boxCollider2D.enabled = false;
        _visualSpriteRenderer.enabled = false;
        _visualHoloSpriteRenderer.enabled = true;
        _holoAudioSource.enabled = true;
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
                PlaySpringSound();
                
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

    private void PlaySpringSound()
    {
        _jumperAudioSource.Play();
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
        // Calculate the jump direction and maximum height based on jumpForce
        Vector3 jumpDirection = new Vector3(_jumpForce.x, _jumpForce.y, 0) * 0.1f;  // Scale down for visualization
        Vector3 jumpEndPoint = transform.position + jumpDirection;  // Final position after jump

        // Draw a line showing the trajectory the player would take
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, jumpEndPoint);

        // Optionally, draw a sphere at the top to show the maximum height
        Gizmos.DrawSphere(jumpEndPoint, 0.1f);
    }
}