using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victoria : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el collider es uno de los personajes
        if (other.CompareTag("Player"))
        {
            // Teletransporta a ambos personajes a sus respectivos spawnPoints
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if (player.name == "Clara")
                {
                    player.transform.position = spawnPoint1.position;
                }
                else if (player.name == "Yema")
                {
                    player.transform.position = spawnPoint2.position;
                }
            }
        }
    }
}
