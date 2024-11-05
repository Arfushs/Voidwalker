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
    
    [Header("Level Selector Menu")]
    [Space]
    [SerializeField] private CanvasGroup _levelsCanvasGroup;
    [SerializeField] private float _levelsFadeDuration = 0.5f;
    
    private Animator _animator;  // Animator bileşenini referanslayın
    

    private void Awake()
    {
        AnimationFinished.OnAnimationFinished += StartAnim;
        _animator = GetComponent<Animator>();
        _mainMenuButtonsCanvasGroup.gameObject.SetActive(false);
    }
    
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            SkipAnimation();
        }
    }

    private void SkipAnimation()
    {
        // Tüm aktif DOTween animasyonlarını sonlandır
        DOTween.KillAll();

        // Animasyonun son hallerine geçiş yap
        _title.color = new Color(_title.color.r, _title.color.g, _title.color.b, 1f);
        _uiBackground.color = new Color(_uiBackground.color.r, _uiBackground.color.g, _uiBackground.color.b, .89f);
        _mainMenuButtonsCanvasGroup.alpha = 1f;
        _mainMenuButtonsCanvasGroup.interactable = true;

        if (_optionsCanvasGroup.alpha > 0f)
        {
            _optionsCanvasGroup.alpha = 1f;
            _optionsCanvasGroup.interactable = true;
            _optionsCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            _mainMenuButtonsCanvasGroup.alpha = 1f;
            _mainMenuButtonsCanvasGroup.interactable = true;
            _optionsCanvasGroup.alpha = 0f;
            _optionsCanvasGroup.interactable = false;
            _optionsCanvasGroup.blocksRaycasts = false;
        }

        // Animator animasyonunu son pozisyona ayarla
        if (_animator != null && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            _animator.Play(_animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 1f);
            _animator.Update(0f); // Anında geçiş yapar
        }
    }

    private void StartAnim(string s)
    {
        _mainMenuButtonsCanvasGroup.gameObject.SetActive(true);
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
            .OnComplete(()=>_mainMenuButtonsCanvasGroup.interactable = true);

    }

    public void OpenLevelSelectorMenu()
    {
        _mainMenuButtonsCanvasGroup.interactable = false;
        _levelsCanvasGroup.blocksRaycasts = true;
        DOTween.Sequence()
            .Append(_mainMenuButtonsCanvasGroup.DOFade(0f, _optionsFadeDuration))
            .AppendInterval(.2f)
            .Append(_levelsCanvasGroup.DOFade(1f, _levelsFadeDuration))
            .OnComplete(()=>_levelsCanvasGroup.interactable = true);
    }

    public void CloseLevelSelectorMenu()
    {
        _levelsCanvasGroup.interactable = false;
        _levelsCanvasGroup.blocksRaycasts = false;
        DOTween.Sequence()
            .Append(_levelsCanvasGroup.DOFade(0f, _levelsFadeDuration))
            .AppendInterval(.2f)
            .Append(_mainMenuButtonsCanvasGroup.DOFade(1f, _levelsFadeDuration))
            .OnComplete(()=>_mainMenuButtonsCanvasGroup.interactable = true);
    }


    private void OnDisable()
    {
        AnimationFinished.OnAnimationFinished -= StartAnim;
    }
}
