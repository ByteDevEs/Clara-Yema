using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private bool isMenuOpen = false;
    public PlayerController playerController1;
    public PlayerController playerController2;
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
        // Desactivar el script de movimiento del personaje
        if (playerController1 != null && playerController2 != null)
        {
            playerController1.enabled = false;
            playerController2.enabled = false;
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

        if (playerController1 != null && playerController1 != null)
        {
            playerController1.enabled = true;
            playerController2.enabled = true;
        }
    }
}
