using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuerteFueraRango : MonoBehaviour
{
    private PlayerController playerController;
    private Vector3 lastPosition; // Almacena la última posición válida del personaje.

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        lastPosition = transform.position; // Inicializa la última posición con la posición inicial del personaje.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutOfRange")) //Este es el tag del agua, si se cambia a otro, renombrarlo
        {
            // El personaje ha caído al agua, así que lo teleportamos a la última posición válida.
            transform.position = lastPosition;
            //Debug.Log("TP fuera del agua");
        }
    }

    void Update()
    {
        // Actualizamos la última posición válida en cada frame, pero evitamos actualizarla en el borde.
        if (playerController.Grounded())
        {
            if (Vector3.Distance(transform.position, lastPosition) > 1f)
            {
                lastPosition = transform.position;
                //Debug.Log("Última posición válida actualizada: " + lastPosition);
            }
        }

    }

}
