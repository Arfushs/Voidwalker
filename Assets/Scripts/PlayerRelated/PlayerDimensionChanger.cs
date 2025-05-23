using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDimensionChanger : MonoBehaviour
{
    [SerializeField] private Transform _spriteMask;
    [SerializeField] private float _dimensionChangeDuration = .5f;
    [SerializeField] private AudioClip _clip1;
    [SerializeField] private AudioClip _clip2;
    
    private AudioSource _audioSource;
    private int _audioIndex =0;
    private InputActions _inputActions;
    private bool _canChangeDimension = true;
    public static event Action OnDimensionChanged;
    public static event Action OnMaskTransitionComplete;
    private void Awake()
    {
        _inputActions = new InputActions();
        _audioSource = GetComponent<AudioSource>();
        
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.DimensionChanger.performed += ChangeDimension;
    }

    private void OnDisable()
    {
        _inputActions.Player.DimensionChanger.performed -= ChangeDimension;
        _inputActions.Disable();
    }

    private void ChangeDimension(InputAction.CallbackContext context)
    {
        if (_canChangeDimension)
        {
            Debug.Log("Change dimension");

            if (_audioIndex == 0)
            {
                _audioSource.PlayOneShot(_clip1);
                _audioIndex = 1;
            }
            else if (_audioIndex == 1)
            {
                _audioSource.PlayOneShot(_clip2);
                _audioIndex = 0;
            }
            
            _canChangeDimension = false;
            OnDimensionChanged?.Invoke();
            OpenSpriteMask();
        }
        
    }

    private void ResetDimensionCounter()
    {
        _canChangeDimension = true;
    }

    private void OpenSpriteMask()
    {
        Vector3 finalScale = new Vector3(50, 50, 50);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_spriteMask.DOScale(finalScale, _dimensionChangeDuration).SetEase(Ease.InQuad))
            .OnComplete(() => CloseSpriteMask());
    }


    private void CloseSpriteMask()
    {
        _spriteMask.transform.localScale = Vector3.zero;
        ResetDimensionCounter();
        OnMaskTransitionComplete?.Invoke();
    }

    
}
