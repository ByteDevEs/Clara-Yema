using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CajonLaberinto : MonoBehaviour
{
    //Cambia la escena al laberinto
    public void onTriggerEnter()
    {
        SceneManager.LoadScene(3);
    }
}
