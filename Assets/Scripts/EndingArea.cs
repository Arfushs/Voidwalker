using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EndingArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ChangeNextLevel();
        }
    }

    private async void ChangeNextLevel()
    {
        FadeoutCanvas.Instance.FadeOut();
        await Task.Delay(250);
        LevelManager.Instance.LoadNextLevel();
    }
}
