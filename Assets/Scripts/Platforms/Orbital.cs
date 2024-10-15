using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class Orbital : MonoBehaviour, IDimensional
{
    [field: Header("Platform Infos")]
    [field: Space]
    [field: SerializeField] public DimensionType DimensionType { get; private set; }
    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _visualHolo;
    [SerializeField] private Transform _platformsContainer;

    [field: Header("Orbital Settings Infos")]
    [field: Space]
    [SerializeField] private Vector2 _radius = Vector2.one;
    [SerializeField] private float _rotationDuration = 0.5f;
    private List<IDimensional> _platforms = new List<IDimensional>();
    private float orbital_angle_offset;

    private void Awake()
    {
        _platforms.AddRange(_platformsContainer.GetComponentsInChildren<IDimensional>());
        
    }

    private void FixedUpdate()
    {
        orbital_angle_offset += 2* Mathf.PI * Time.deltaTime / _rotationDuration;
        orbital_angle_offset = Mathf.Repeat(orbital_angle_offset, 2 * Mathf.PI);
        RotatePlatforms();
    }

    public void Show()
    {
        if (DimensionType == DimensionType.Both)
            return;
        _visual.gameObject.SetActive(true);
        _visualHolo.gameObject.SetActive(false);
    }

    public void Hide()
    {
        if (DimensionType == DimensionType.Both)
            return;
        _visual.gameObject.SetActive(false);
        _visualHolo.gameObject.SetActive(true);
    }

    public DimensionType GetDimensionType()
    {
        return DimensionType;
    }
    

    private void RotatePlatforms()
    {
        if (_platforms.Count != 0)
        {
            float spacing = 2 * Mathf.PI / _platforms.Count;
            for(int i =0; i < _platforms.Count; i++)
            {
                Vector2 pos = new Vector2(Mathf.Cos(spacing*i + orbital_angle_offset)* _radius.x, Mathf.Sin(spacing * i + orbital_angle_offset) * _radius.y);
                ((MonoBehaviour)_platforms[i]).transform.localPosition = pos;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius.x);
    }
}
