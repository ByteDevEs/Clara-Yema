using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class SplashController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool loadScene = false;
    public GameObject controlador1;
    public GameObject controlador2;

    void Update()
    {
        // Incrementar el tiempo transcurrido
        elapsedTime += Time.deltaTime;

        // Verificar si ha pasado más de 10 segundos
        if (elapsedTime >= 10f && !loadScene)
        {
            loadScene = true;
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    public void OnButton(InputAction.CallbackContext context)
    {
        Destroy(controlador1);
        Destroy(controlador2);

        SceneManager.LoadScene("MenuPrincipal");

    }

}
