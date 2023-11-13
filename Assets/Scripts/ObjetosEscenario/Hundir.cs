using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hundir : MonoBehaviour
{
    public float sinkSpeed = 1.0f; // Velocidad de hundimiento.
    public float riseSpeed = 1.0f; // Velocidad de ascenso.
    public float sinkDepth = -5.0f; // Profundidad a la que la plataforma se hunde.
    private Vector3 originalPosition; // Posición original de la plataforma.
    private int playerCount = 0; // Contador de jugadores en la plataforma.

    private void Start()
    {
        originalPosition = transform.position; // Almacena la posición original.
    }

    private void Update()
    {
        // Si la plataforma está hundiéndose y no ha alcanzado la profundidad deseada, sigue hundiéndose.
        if (playerCount > 0 && transform.position.y > sinkDepth)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= sinkSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
        // Si la plataforma no está hundiéndose y no ha alcanzado su posición original, sube poco a poco.
        else if (playerCount == 0 && transform.position.y < originalPosition.y)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += riseSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando un personaje colisiona con la plataforma, aumenta el contador.
        if (other.CompareTag("Player"))
        {
            playerCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando un personaje deja de colisionar con la plataforma, disminuye el contador.
        if (other.CompareTag("Player"))
        {
            playerCount--;
        }
    }
}
