using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBroom : MonoBehaviour
{
    // Referencia al objeto cucharón.
    public GameObject cucharon;

    private bool fijado = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si ha colisionado con el objeto escoba y no está fijado ya.
        if (collision.gameObject.CompareTag("Broom") && !fijado)
        {
            // Desactiva el Rigidbody del cucharón para que se quede fijo.
            Rigidbody rb = cucharon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Hace que el objeto no sea afectado por fuerzas físicas.
            }

            // Fija la posición del cucharón a la posición de la colisión.
            cucharon.transform.position = collision.contacts[0].point;

            // Marca el cucharón como fijado.
            fijado = true;
        }
    }
}
