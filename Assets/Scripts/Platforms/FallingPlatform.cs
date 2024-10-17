using System;
using System.Collections;
using UnityEngine;
using Enums;
using Interfaces;
using DG.Tweening;

public class FallingPlatform : MonoBehaviour, IDimensional
{
    [field: Header("Platform Infos")]
    [field: Space]
    [field: SerializeField] public DimensionType DimensionType { get; private set; }
    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _visualHolo;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fallDelay = 1.0f; // Total time before falling
    [SerializeField] private float _returnDelay = 1.0f; // Time before returning to original position
    [SerializeField] private float _fallDistance = 2f; // Distance to fall down
    [SerializeField] private float _fallDuration = 0.5f; // Time to reach fall position
    [SerializeField] private float _returnDuration = 0.5f; // Time to return to original position

    [field: Header("Tween Settings")]
    [field: Space]
    [SerializeField] private float _shakeStrength = 0.1f; // Intensity of the shake
    [Range(0f, 1f)]
    [SerializeField] private float _shakeRatio = 0.8f; // Ratio to split between normal and stronger shakes

    private Vector3 _initialPosition;
    private Vector3 _initialPositionHolo;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _initialPosition = _visual.position; // Store the initial position of the platform
        _initialPositionHolo = _visualHolo.position; // Store the initial position of the holo platform

        switch (DimensionType)
        {
            case DimensionType.Alter:
                _animator.Play("fallingPlatformAlterIdle");
                break;
            case DimensionType.Default :
                _animator.Play("fallingPlatformIdle");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HandleFallingPlatform());
        }
    }

    private IEnumerator HandleFallingPlatform()
    {
        // Calculate shake durations based on _fallDelay and _shakeRatio
        float initialShakeDuration = _fallDelay * _shakeRatio; // Normal shake duration
        float finalShakeDuration = _fallDelay * (1 - _shakeRatio); // Stronger shake duration

        // Initial shake phase
        _visual.DOShakePosition(initialShakeDuration, _shakeStrength).SetEase(Ease.InOutQuad);
        _visualHolo.DOShakePosition(initialShakeDuration, _shakeStrength).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(initialShakeDuration);

        // Stronger shake before fall
        _visual.DOShakePosition(finalShakeDuration, _shakeStrength * 2).SetEase(Ease.InOutQuad);
        _visualHolo.DOShakePosition(finalShakeDuration, _shakeStrength * 2).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(finalShakeDuration);
        
        // Start falling after the shake is done
        transform.DOMoveY(_initialPosition.y - _fallDistance, _fallDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(_fallDuration + _returnDelay);

        // Move the platform back to the original position
        transform.DOMoveY(_initialPosition.y, _returnDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(_returnDuration);
        
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
}
