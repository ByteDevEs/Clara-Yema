using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Explosion : MonoBehaviour
{
    public float explosionForce = 20f;     // Fuerza de la explosi�n.
    public float verticalForce = 10f;       // Fuerza vertical adicional.
    public float explosionRadius = 5f;     // Radio de la explosi�n.
    public LayerMask objectsToPush;        // Capas de los objetos que se lanzar�n.
    [SerializeField] protected Animator animator;
    void Update()
    {

    }

    public void SkillSquare(InputAction.CallbackContext context)
    {
        Explode();
    }

    void Explode()
    {
        animator.SetBool("Explode",true );
        // Encuentra todos los objetos dentro del radio de la explosi�n.
        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, explosionRadius, objectsToPush);

        // Aplica una fuerza a todos los objetos dentro del radio.
        foreach (Collider obj in objectsInRadius)
        {
            // Intenta encontrar el Rigidbody en el objeto o en su transformaci�n padre.
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = obj.transform.GetComponentInParent<Rigidbody>();
            }

            if (rb != null && rb.gameObject != gameObject)  // Evita afectar al propio personaje.
            {
                // Calcula la direcci�n desde el personaje al objeto.
                Vector3 direction = obj.transform.position - transform.position;

                // Aplica la fuerza para lanzar el objeto con componente vertical adicional.
                rb.AddForce(direction.normalized * explosionForce + Vector3.up * verticalForce, ForceMode.Impulse);
            }
        }
        Invoke("NoEx",1f);
        
    }
     IEnumerator NoEx()
    {
        animator.SetBool("Explode",false );
        return null;
    }
}

