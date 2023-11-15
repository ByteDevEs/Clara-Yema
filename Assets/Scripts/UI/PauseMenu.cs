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
        // Nos aseguramos de desactivar el menú de pausa al inicio
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
        

        // Controlar el menú si está abierto
        if (isMenuOpen)
        {
            //Provisional
        }
    }

    void PauseGame()
    {
        isPaused = true;

        // Pausar el tiempo y mostrar el menú
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

        // Reanudar el tiempo y cerrar el menú
        Time.timeScale = 1f;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            isMenuOpen = false;
        }
    }
}
