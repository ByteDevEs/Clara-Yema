using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private bool isMenuOpen = false;

    public GameObject pauseMenu;

    void Start()
    {
        // Nos aseguramos de desactivar el men� de pausa al inicio
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.started)
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
    }
    void Update()
    {
        

        // Controlar el men� si est� abierto
        if (isMenuOpen)
        {
            //Provisional
        }
    }

    void PauseGame()
    {
        isPaused = true;

        // Pausar el tiempo y mostrar el men�
        Time.timeScale = 0f;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            isMenuOpen = true;
        }
    }

    void ResumeGame()
    {
        isPaused = false;

        // Reanudar el tiempo y cerrar el men�
        Time.timeScale = 1f;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            isMenuOpen = false;
        }
    }
}
