using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CajonLaberinto : MonoBehaviour
{
    private int playersInside = 0; // Variable para contar los jugadores dentro del collider
    public string sceneName = "SceneLaberinto";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersInside++; // Incrementa el contador cuando un jugador entra
            Debug.Log("Player entered. Total players inside: " + playersInside);

            if (playersInside >= 2) // Verifica si ambos jugadores están dentro
            {
                PlayerController.spawnPosition = new Dictionary<int, Vector3>();
                PlayerController[] players = FindObjectsOfType<PlayerController>();
                foreach (PlayerController player in players)
                {
                    PlayerController.spawnPosition.Add(player.myKey, new Vector3(-47, 61.91f, 25));
                }
                SceneManager.LoadScene(sceneName); // Carga la escena
            }
        }
    }

   
}