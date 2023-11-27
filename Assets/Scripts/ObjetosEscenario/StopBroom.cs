using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBroom : MonoBehaviour
{
    // Referencia al objeto cuchar�n.
    public GameObject cucharon;

    private bool fijado = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si ha colisionado con el objeto escoba y no est� fijado ya.
        if (collision.gameObject.CompareTag("Broom") && !fijado)
        {
            // Desactiva el Rigidbody del cuchar�n para que se quede fijo.
            Rigidbody rb = cucharon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Hace que el objeto no sea afectado por fuerzas f�sicas.
            }

            // Fija la posici�n del cuchar�n a la posici�n de la colisi�n.
            cucharon.transform.position = collision.contacts[0].point;

            // Marca el cuchar�n como fijado.
            fijado = true;
        }
    }
}
