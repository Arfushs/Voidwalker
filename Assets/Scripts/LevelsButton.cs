using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TMP_FontAsset _fontAsset;
    [SerializeField] private TMP_FontAsset _fontAssetDisabled;
    [SerializeField ]private Button _button;

    [field:SerializeField] public int LevelIndex { get; private set; }
    

    public void ChangeButtonToDisabled()
    {
        _levelText.font = _fontAssetDisabled;
        _button.interactable = false;
    }

    public void ChangeButtonToEnabled()
    {
        _levelText.font = _fontAsset;
        _button.interactable = true;
    }
    
}
