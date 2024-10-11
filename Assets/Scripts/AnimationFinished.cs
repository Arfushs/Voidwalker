using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFinished : MonoBehaviour
{
    public static event Action<string> OnAnimationFinished;


    public void FireAnimationFinished(string stateName)
    {
        OnAnimationFinished?.Invoke(stateName);
    }
}
