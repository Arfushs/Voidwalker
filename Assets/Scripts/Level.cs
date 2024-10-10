using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Enums;
using Interfaces;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("GameObjects")]
    [Space]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform defaultPlatformsContainer;
    [SerializeField] private Transform alterPlatformContainer;
    [SerializeField] private DimensionalBackground defaultBg, alterBg;
    [SerializeField] private AutoConfiner _confiner;
    [Header("Options")]
    [Space]
    [SerializeField] private int _levelIndex;

    public static event Action OnLevelComplete;
    
    [field:SerializeField] public DimensionType InitialDimension { get; private set; }
    
    private Player _player;

    private void Awake()
    {
        PlayerDimensionChanger.OnDimensionChanged += OnDimensionChanged;
        PlayerDimensionChanger.OnMaskTransitionComplete += HandleBackgrounds;
    }

    public void Init(Player player, CinemachineConfiner2D cinemachineConfiner)
    {
        _player = player;
        _player.SetupPlayer(this);
        _confiner.Init(cinemachineConfiner);
        _player.transform.position = _playerSpawnPoint.position;
        ChangeDimension(InitialDimension);
        HandleBackgrounds();
    }

    public void ResetLevel()
    {
        _player.transform.position = _playerSpawnPoint.position;
        ChangeDimension(InitialDimension);
        HandleBackgrounds();
    }

    private void HandleBackgrounds()
    {
        if (defaultBg.DimensionType == _player.CurrentDimension)
        {
            
            defaultBg.ChangeMaskInteractionToOutside();
            alterBg.ChangeMaskInteractionToInside();
        }
        else
        {
            alterBg.ChangeMaskInteractionToOutside();
            defaultBg.ChangeMaskInteractionToInside();
        }
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
        
        if(allPlatforms.Count == 0)
            return;
        
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
        PlayerDimensionChanger.OnMaskTransitionComplete -= HandleBackgrounds;
    }
}