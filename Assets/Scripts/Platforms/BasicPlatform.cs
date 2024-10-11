using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public class BasicPlatform : MonoBehaviour, IDimensional
{
    [field:SerializeField] public DimensionType DimensionType { get; private set;}
    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _visualHolo;

    private BoxCollider2D _boxCollider2D;
    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Show()
    {
        _boxCollider2D.enabled = true;
        _visual.gameObject.SetActive(true);
        _visualHolo.gameObject.SetActive(false);
    }

    public void Hide()
    {
        _boxCollider2D.enabled = false;
        _visual.gameObject.SetActive(false);
        _visualHolo.gameObject.SetActive(true);
    }

    public DimensionType GetDimensionType()
    {
        return DimensionType;
    }
}
