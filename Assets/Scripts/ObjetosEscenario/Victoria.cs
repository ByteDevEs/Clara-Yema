using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victoria : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public GameObject ClaraWin;
    public GameObject YemaWin;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el collider es uno de los personajes
        if (other.CompareTag("Player"))
        {

            if (other.name == "Clara")
            {
                ClaraWin.SetActive(true);
            }
            else if (other.name == "Yema")
            {
                YemaWin.SetActive(true);
            }

        }
        StartCoroutine(TeletransportarYDesactivarDespuesDeEspera());
    }

    IEnumerator TeletransportarYDesactivarDespuesDeEspera()
    {
        yield return new WaitForSeconds(5f); // Espera 5 segundos

        // Desactiva los objetos ganadores
        ClaraWin.SetActive(false);
        YemaWin.SetActive(false);

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

