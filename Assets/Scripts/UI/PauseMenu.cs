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
        // Asegúrate de desactivar el menú de pausa al inicio
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
                // Si ya está en pausa, reanudar el juego
                ResumeGame();
            }
            else
            {
                // Si no está en pausa, pausar el juego y abrir el menú
                PauseGame();
            }
        }
    }
    void Update()
    {
        

        // Controlar el menú si está abierto
        if (isMenuOpen)
        {
            // Aquí puedes agregar lógica para controlar el menú con el jugador que pulsó "Start"
            // Puedes usar Input.GetAxis, Input.GetButton, etc., según tus necesidades
            // Por ejemplo:
            // float menuInput = Input.GetAxis("Vertical");
            // Mover el cursor del menú en consecuencia
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
