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

    public void OpenLevelSelectionMenu() => _mainMenuAnim.OpenLevelSelectorMenu();
    
    public void CloseLevelSelectionMenu() => _mainMenuAnim.CloseLevelSelectorMenu();

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

    public void StartGameWithIndex(int index)
    {
        PlayerPrefs.SetInt("last_level", index);
        SceneManager.LoadScene("GameScene");
    }
}
