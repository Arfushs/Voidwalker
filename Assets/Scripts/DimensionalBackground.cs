using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class DimensionalBackground : MonoBehaviour
{
    [field:SerializeField] public DimensionType DimensionType { get; private set; }
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        _spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    public void ChangeMaskInteractionToOutside()
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }
    
    public void ChangeMaskInteractionToInside()
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
}
