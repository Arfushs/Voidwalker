using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeoutCanvas : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeoutDuration;
    [SerializeField] private float fadeinDuration;

    public static FadeoutCanvas Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }
    }

    public void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(1, fadeoutDuration / 2));
        sequence.AppendInterval(fadeinDuration);
        sequence.Append(image.DOFade(0, fadeoutDuration / 2));
    }
}
