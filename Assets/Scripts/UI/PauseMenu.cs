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
        // Aseg�rate de desactivar el men� de pausa al inicio
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
                // Si ya est� en pausa, reanudar el juego
                ResumeGame();
            }
            else
            {
                // Si no est� en pausa, pausar el juego y abrir el men�
                PauseGame();
            }
        }
    }
    void Update()
    {
        

        // Controlar el men� si est� abierto
        if (isMenuOpen)
        {
            // Aqu� puedes agregar l�gica para controlar el men� con el jugador que puls� "Start"
            // Puedes usar Input.GetAxis, Input.GetButton, etc., seg�n tus necesidades
            // Por ejemplo:
            // float menuInput = Input.GetAxis("Vertical");
            // Mover el cursor del men� en consecuencia
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
