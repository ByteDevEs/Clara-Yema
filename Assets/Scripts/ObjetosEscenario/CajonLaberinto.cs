using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CajonLaberinto : MonoBehaviour
{
    //Cambia la escena al laberinto
    public void onTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  SceneManager.LoadScene(3);
    }
}
