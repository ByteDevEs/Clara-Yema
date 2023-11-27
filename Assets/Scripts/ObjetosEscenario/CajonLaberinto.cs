using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CajonLaberinto : MonoBehaviour
{
    private int playersInside = 0; // Variable para contar los jugadores dentro del collider

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersInside++; // Incrementa el contador cuando un jugador entra
            Debug.Log("Player entered. Total players inside: " + playersInside);

            if (playersInside >= 2) // Verifica si ambos jugadores están dentro
            {
                SceneManager.LoadScene("SceneLaberinto"); // Carga la escena
            }
        }
    }

   
}