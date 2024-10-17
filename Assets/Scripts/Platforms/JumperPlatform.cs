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
    [SerializeField] private float jumpForce = 10f;  // Adjustable jump force

    private BoxCollider2D _boxCollider2D;
    private Animator _animator;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = _visual.GetComponent<Animator>();
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
                // Apply upward force to the player using AddForce
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                
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
}