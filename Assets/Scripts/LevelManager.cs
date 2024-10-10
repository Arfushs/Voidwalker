using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Game Objects")]
    [Space]
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineConfiner2D _confiner2D;
    
    public static LevelManager Instance;
    private List<Level> _levels = new List<Level>(); 
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        _levels.AddRange(GetComponentsInChildren<Level>());

    }

    private void Start()
    {
        StartLevel(0);
    }


    public void StartLevel(int levelIndex)
    {
        Level currentLevel = _levels[levelIndex];
        currentLevel.gameObject.SetActive(true);
        currentLevel.Init(_player, _confiner2D);
    }
}
