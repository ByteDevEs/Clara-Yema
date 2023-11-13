using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuerteFueraRango : MonoBehaviour
{
    private PlayerController playerController;
    private Vector3 lastPosition; // Almacena la �ltima posici�n v�lida del personaje.

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        lastPosition = transform.position; // Inicializa la �ltima posici�n con la posici�n inicial del personaje.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutOfRange")) //Este es el tag del agua, si se cambia a otro, renombrarlo
        {
            // El personaje ha ca�do al agua, as� que lo teleportamos a la �ltima posici�n v�lida.
            transform.position = lastPosition;
            //Debug.Log("TP fuera del agua");
        }
    }

    void Update()
    {
        // Actualizamos la �ltima posici�n v�lida en cada frame, pero evitamos actualizarla en el borde.
        if (playerController.Grounded())
        {
            if (Vector3.Distance(transform.position, lastPosition) > 1f)
            {
                lastPosition = transform.position;
                //Debug.Log("�ltima posici�n v�lida actualizada: " + lastPosition);
            }
        }

    }

}
