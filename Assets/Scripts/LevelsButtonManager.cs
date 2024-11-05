using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsButtonManager : MonoBehaviour
{
    private List<LevelsButton> _levelsButtons = new List<LevelsButton>();

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("max_level"))
            PlayerPrefs.SetInt("max_level", 0);
        
        _levelsButtons.AddRange(GetComponentsInChildren<LevelsButton>());
        SetupButtons();
    }

    private void SetupButtons()
    {
        int maxIndex = PlayerPrefs.GetInt("max_level");
        foreach (LevelsButton button  in _levelsButtons)
        {
            if(button.LevelIndex -1 <= maxIndex)
                button.ChangeButtonToEnabled();
            else
            {
                button.ChangeButtonToDisabled();
            }
        }
        
    }
}
