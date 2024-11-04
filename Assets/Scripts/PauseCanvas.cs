using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour
{
    private InputActions _inputActions;
    [SerializeField] private GameObject _pausePanel;
    
    private bool isPaused = false;
    private void Awake()
    {
        _inputActions = new InputActions();
        
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Pause.performed += PauseGame;
    }

    private void OnDisable()
    {
        _inputActions.Player.Pause.performed -= PauseGame;
        _inputActions.Disable();
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        isPaused = !isPaused;
        _pausePanel.SetActive(isPaused);
        
        if (isPaused)
            Time.timeScale = 0;
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        _pausePanel.SetActive(isPaused);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
