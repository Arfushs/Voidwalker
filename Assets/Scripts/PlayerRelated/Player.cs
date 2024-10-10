using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Player : MonoBehaviour
{
    public DimensionType CurrentDimension { get; private set; }
    private Level _playerLevel;
    private float _score;
    
    private void Awake()
    {
        
    }

    public void SetupPlayer(Level level)
    {
        CurrentDimension = level.InitialDimension;
    }

    public void SetDimension(DimensionType dimension)
    {
        CurrentDimension = dimension;
    }
}
