using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDimensionChanger : MonoBehaviour
{
    private InputActions _inputActions;
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
        Debug.Log("Change dimension");
        OnDimensionChanged?.Invoke();
    }
}
