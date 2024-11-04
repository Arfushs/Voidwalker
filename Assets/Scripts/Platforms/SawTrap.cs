using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class SawTrap : MonoBehaviour, IDimensional
{
    [field: Header("Platform Infos")]
    [field: Space]
    [field:SerializeField] public DimensionType DimensionType { get; private set;}
    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _visualHolo;
    [SerializeField] private float _rotationSpeed;
    private DeadZone _deadZone;

    private void Awake()
    {
        _deadZone = GetComponent<DeadZone>();
    }

    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 360), _rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    
    public void Show()
    {
        if(DimensionType == DimensionType.Both)
            return;
        _visual.gameObject.SetActive(true);
        _visualHolo.gameObject.SetActive(false);
        _deadZone.IsActive = true;
    }

    public void Hide()
    {
        if(DimensionType == DimensionType.Both)
            return;
        _visual.gameObject.SetActive(false);
        _visualHolo.gameObject.SetActive(true);
        _deadZone.IsActive = false;
    }

    public DimensionType GetDimensionType()
    {
        return DimensionType;
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
