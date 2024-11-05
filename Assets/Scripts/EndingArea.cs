using System;
using System.Collections;
using System.Collections.Generic;

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

    private void ChangeNextLevel()
    {
        StartCoroutine("ChangeNextLevelCoroutine");
    }

    private IEnumerator ChangeNextLevelCoroutine()
    {
        FadeoutCanvas.Instance.FadeOut();
        yield return new WaitForSeconds(0.25f);
        LevelManager.Instance.LoadNextLevel();
    }
}
