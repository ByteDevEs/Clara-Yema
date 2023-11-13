using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform target;          // El transform del objeto que seguir� la c�mara (en este caso, el objeto "Yema").
    public float distance = 10f;      // La distancia entre la c�mara y el objetivo.
    public float height = 5f;         // La altura de la c�mara por encima del objetivo.

    void LateUpdate()
    {
        if (target != null)
        {
            // Calcula la posici�n deseada de la c�mara.
            Vector3 desiredPosition = target.position - Vector3.forward * distance + Vector3.up * height;

            // Actualiza la posici�n de la c�mara.
            transform.position = desiredPosition;

            // Asegura que la c�mara siempre mire hacia el objetivo.
            transform.LookAt(target.position);
        }
    }
}
