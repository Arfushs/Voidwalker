using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("Game Objects")]
    [Space]
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineConfiner2D _confiner2D;
    
    private List<Level> _levels = new List<Level>();
    private int _currentLevelIndex = 0;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("last_level"))
        {
            _currentLevelIndex = PlayerPrefs.GetInt("last_level");
        }
        else
        {
            PlayerPrefs.SetInt("last_level", 0);
        }
        _levels.AddRange(GetComponentsInChildren<Level>());

    }

    private void Start()
    {
        StartLevel(_currentLevelIndex);
    }


    public void StartLevel(int levelIndex)
    {
        Level currentLevel = _levels[levelIndex];
        currentLevel.gameObject.SetActive(true);
        currentLevel.Init(_player, _confiner2D);
    }
}
