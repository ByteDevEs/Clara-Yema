using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BUM : MonoBehaviour
{
    public float explosionForce = 20f;     // Fuerza de la explosi�n.
    public float verticalForce = 10f;       // Fuerza vertical adicional.
    public float explosionRadius = 5f;     // Radio de la explosi�n.
    public LayerMask objectsToPush;        // Capas de los objetos que se lanzar�n.

    void Update()
    {

    }

    public void SkillSquare(InputAction.CallbackContext context)
    {
        Explode();
    }

    void Explode()
    {
        // Encuentra todos los objetos dentro del radio de la explosi�n.
        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, explosionRadius, objectsToPush);

        // Aplica una fuerza a todos los objetos dentro del radio.
        foreach (Collider obj in objectsInRadius)
        {
            if (obj.gameObject != gameObject)  // Evita afectar al propio personaje.
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calcula la direcci�n desde el personaje al objeto.
                    Vector3 direction = obj.transform.position - transform.position;

                    // Aplica la fuerza para lanzar el objeto con componente vertical adicional.
                    rb.AddForce(direction.normalized * explosionForce + Vector3.up * verticalForce, ForceMode.Impulse);
                }
            }
        }
    }
}
