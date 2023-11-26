using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CajonLaberinto : MonoBehaviour
{
    //Cambia la escena al laberinto
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { SceneManager.LoadScene(3); }
    }
}
