using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    private bool isPaused = false;

    public void OnStart(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pausa el tiempo del juego
        pauseMenuCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        pauseMenuCanvas.SetActive(false);
    }
}
