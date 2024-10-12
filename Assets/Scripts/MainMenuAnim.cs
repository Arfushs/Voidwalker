using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAnim : MonoBehaviour
{
    [Header("Main Menu")]
    [Space]
    [SerializeField] private Image _uiBackground;
    [SerializeField] private Image _title;
    [SerializeField] private CanvasGroup _mainMenuButtonsCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;
    
    [Header("Options Menu")]
    [Space]
    [SerializeField] private CanvasGroup _optionsCanvasGroup;
    [SerializeField] private float _optionsFadeDuration = 0.5f;
    

    private void Awake()
    {
        AnimationFinished.OnAnimationFinished += StartAnim;
    }

    private void StartAnim(string s)
    {
        if (s == "main_menu")
        {
            _title.DOFade(1f, _fadeDuration);
            _uiBackground.DOFade(.89f, _fadeDuration);
            _mainMenuButtonsCanvasGroup.DOFade(1f, _fadeDuration);
        }
    }

    public void OpenOptionsMenu()
    {
        _mainMenuButtonsCanvasGroup.interactable = false;
        _optionsCanvasGroup.blocksRaycasts = true;
        DOTween.Sequence()
            .Append(_mainMenuButtonsCanvasGroup.DOFade(0f, _optionsFadeDuration))
            .AppendInterval(.2f)
            .Append(_optionsCanvasGroup.DOFade(1f, _optionsFadeDuration))
            .OnComplete(()=>_optionsCanvasGroup.interactable = true);
    }

    public void CloseOptionsMenu()
    {
        _optionsCanvasGroup.interactable = false;
        _optionsCanvasGroup.blocksRaycasts = false;
        DOTween.Sequence()
            .Append(_optionsCanvasGroup.DOFade(0f, _optionsFadeDuration))
            .AppendInterval(.2f)
            .Append(_mainMenuButtonsCanvasGroup.DOFade(1f, _optionsFadeDuration))
            .OnComplete(()=>_mainMenuButtonsCanvasGroup.interactable = true);;

    }


    private void OnDisable()
    {
        AnimationFinished.OnAnimationFinished -= StartAnim;
    }
}
