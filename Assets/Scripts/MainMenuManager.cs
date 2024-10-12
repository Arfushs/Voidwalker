using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenuAnim _mainMenuAnim;

    public void OpenOptionsMenu() => _mainMenuAnim.OpenOptionsMenu();
    public void CloseOptionsMenu() => _mainMenuAnim.CloseOptionsMenu();

    private void Awake()
    {
        Cursor.visible = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); // Build edilen oyunu kapat
#endif
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
