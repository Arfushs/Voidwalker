using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAnim : MonoBehaviour
{
    [SerializeField] private Image _uiBackground;
    [SerializeField] private Image _title;
    [SerializeField] private float _fadeDuration = 0.5f;


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
        }
        
    }

    private void OnDisable()
    {
        AnimationFinished.OnAnimationFinished -= StartAnim;
    }
}
