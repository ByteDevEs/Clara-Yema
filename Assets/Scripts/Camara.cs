using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform target;          // El transform del objeto que seguirá la cámara (en este caso, el objeto "Yema").
    public float distance = 10f;      // La distancia entre la cámara y el objetivo.
    public float height = 5f;         // La altura de la cámara por encima del objetivo.

    void LateUpdate()
    {
        if (target != null)
        {
            // Calcula la posición deseada de la cámara.
            Vector3 desiredPosition = target.position - Vector3.forward * distance + Vector3.up * height;

            // Actualiza la posición de la cámara.
            transform.position = desiredPosition;

            // Asegura que la cámara siempre mire hacia el objetivo.
            transform.LookAt(target.position);
        }
    }
}
