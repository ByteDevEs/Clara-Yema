using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUM : MonoBehaviour
{
    public float explosionForce = 20f;     // Fuerza de la explosión.
    public float verticalForce = 10f;       // Fuerza vertical adicional.
    public float explosionRadius = 5f;     // Radio de la explosión.
    public LayerMask objectsToPush;        // Capas de los objetos que se lanzarán.

    void Update()
    {
        // Comprueba si se ha presionado el botón "X" del mando.
        if (Input.GetButtonDown("Fire1")) 
        {
            // Haz que el personaje explote.
            Explode();
        }
    }

    void Explode()
    {
        // Encuentra todos los objetos dentro del radio de la explosión.
        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, explosionRadius, objectsToPush);

        // Aplica una fuerza a todos los objetos dentro del radio.
        foreach (Collider obj in objectsInRadius)
        {
            if (obj.gameObject != gameObject)  // Evita afectar al propio personaje.
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calcula la dirección desde el personaje al objeto.
                    Vector3 direction = obj.transform.position - transform.position;

                    // Aplica la fuerza para lanzar el objeto con componente vertical adicional.
                    rb.AddForce(direction.normalized * explosionForce + Vector3.up * verticalForce, ForceMode.Impulse);
                }
            }
        }
    }
}
