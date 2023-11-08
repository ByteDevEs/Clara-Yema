using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hundir : MonoBehaviour
{
    public float sinkSpeed = 1.0f; // Velocidad de hundimiento.
    public float riseSpeed = 1.0f; // Velocidad de ascenso.
    public float sinkDepth = -5.0f; // Profundidad a la que la plataforma se hunde.
    private Vector3 originalPosition; // Posición original de la plataforma.
    private bool isSinking = false; // Indica si la plataforma se está hundiendo.

    private void Start()
    {
        originalPosition = transform.position; // Almacena la posición original.
    }

    private void Update()
    {
        // Si la plataforma está hundiéndose y no ha alcanzado la profundidad deseada, sigue hundiéndose.
        if (isSinking && transform.position.y > sinkDepth)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= sinkSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
        // Si la plataforma no está hundiéndose y no ha alcanzado su posición original, sube poco a poco.
        else if (!isSinking && transform.position.y < originalPosition.y)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += riseSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando un personaje colisiona con la plataforma, inicia el hundimiento.
        if (other.CompareTag("Player"))
        {
            isSinking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando un personaje deja de colisionar con la plataforma, detiene el hundimiento.
        if (other.CompareTag("Player"))
        {
            isSinking = false;
        }
    }
}
