using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform defaultPlatformsContainer;
    [SerializeField] private Transform alterPlatformContainer;
    [SerializeField] private DimensionalBackground defaultBg, alterBg;
    public Transform PlayerSpawnPoint { get; private set; }
    public DimensionType InitialDimension { get; private set; }
    
    private Player _player;

    private void Awake()
    {
        PlayerDimensionChanger.OnDimensionChanged += OnDimensionChanged;
    }

    private void OnDimensionChanged()
    {
        DimensionType targetDimension = _player.CurrentDimension == DimensionType.Default ?
            DimensionType.Alter : DimensionType.Default;
        
        _player.SetDimension(targetDimension);
        ChangeDimension(targetDimension);
    }

    private void ChangeDimension(DimensionType targetDimension)
    {
        List<IDimensional> allPlatforms = new List<IDimensional>();
        allPlatforms.AddRange(defaultPlatformsContainer.GetComponentsInChildren<IDimensional>());
        allPlatforms.AddRange(alterPlatformContainer.GetComponentsInChildren<IDimensional>());
        
        foreach (IDimensional platform in allPlatforms)
        {
            if (platform.DimensionType == targetDimension)
            {
                platform.Show();
            }
            else
            {
                platform.Hide();
            }
        }
    }

    private void OnDestroy()
    {
        PlayerDimensionChanger.OnDimensionChanged -= OnDimensionChanged;
    }
}