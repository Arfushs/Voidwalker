using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Enums;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class Level : MonoBehaviour
{
    [Header("GameObjects")]
    [Space]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform allPlatformContainer;
    [SerializeField] private DimensionalBackground defaultBg, alterBg;
    [SerializeField] private AutoConfiner _confiner;
    [Header("Options")]
    [Space]
    [SerializeField] private int _levelIndex;
    
    [field:SerializeField] public DimensionType InitialDimension { get; private set; }
    
    private Player _player;

    private void Awake()
    {
        Debug.Log($"{gameObject.name} instantiated");
    }

    private void OnEnable()
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
        _player.SetDimension(InitialDimension);
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
        allPlatforms.AddRange(allPlatformContainer.GetComponentsInChildren<IDimensional>());
        
        if(allPlatforms.Count == 0)
            return;
        
        foreach (IDimensional platform in allPlatforms)
        {
            if (platform.GetDimensionType() == targetDimension)
            {
                platform.Show();
            }
            else
            {
                platform.Hide();
            }
        }
    }

    private void OnDisable()
    {
        PlayerDimensionChanger.OnDimensionChanged -= OnDimensionChanged;
        PlayerDimensionChanger.OnMaskTransitionComplete -= HandleBackgrounds;
    }

    private void OnDestroy()
    {
        Debug.Log($"{gameObject.name} destroyed");
    }
}