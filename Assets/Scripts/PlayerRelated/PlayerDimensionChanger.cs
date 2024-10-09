using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDimensionChanger : MonoBehaviour
{
    [SerializeField] private Transform _spriteMask;
    [SerializeField] private float _dimensionChangeDuration = .5f;
    
    private InputActions _inputActions;
    private bool _canChangeDimension = true;
    public static event Action OnDimensionChanged;
    private void Awake()
    {
        _inputActions = new InputActions();
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
            _canChangeDimension = false;
            OnDimensionChanged?.Invoke();
            Invoke("ResetDimensionCounter", _dimensionChangeDuration);
        }
        
    }

    private void ResetDimensionCounter()
    {
        _canChangeDimension = true;
    }

    
}
