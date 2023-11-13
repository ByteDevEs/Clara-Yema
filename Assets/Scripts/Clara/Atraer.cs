using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Atraer : MonoBehaviour
{
    public float shootingRange = 10f;      // Rango máximo de disparo.
    public float attractionForce = 5f;     // Fuerza de atracción.
    public float attractionDistance = 1f;  // Distancia a la que se desmarca el objeto.
    public LayerMask shootableLayer;       // Capa de objetos que se pueden disparar.
    public Color markedColor = Color.green; // Color cuando se marca el objeto.
    public float distanceStop = 1f;
    private GameObject markedObject;       // Objeto actualmente marcado.
    private Color originalColor;           // Color original del objeto marcado.
    private bool isMarked = false;         // Estado de marcado.
    private bool isAttracting = false;     // Estado de atracción.


    public void SkillSquare(InputAction.CallbackContext context)
    {
        if (isAttracting)
        {
            // Si se está atrayendo, detén la atracción y el objeto en seco.
            StopAttraction();
        }
        else if (isMarked)
        {
            // Si ya se ha marcado un objeto, activa la atracción.
            StartAttraction();
        }
        else
        {
            // Si no se ha marcado un objeto, dispara un rayo y marca el objeto más cercano en la dirección del rayo.
            Shoot();
        }
    }


    void Update()
    {
       
        // Atraer el objeto marcado hacia el personaje si se activa la atracción.
        if (isAttracting && markedObject != null)
        {
            AttractObject();

            // Comprueba si el objeto está lo suficientemente cerca para desmarcarlo y detenerlo en seco.
            if (Vector3.Distance(transform.position, markedObject.transform.position) <= attractionDistance)
            {
                StopAttraction();
            }
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootingRange, shootableLayer))
        {
            GameObject hitObject = hit.transform.gameObject;
            Rigidbody rb = hitObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                MarkObject(hitObject);
            }
        }
    }

    void MarkObject(GameObject objToMark)
    {
        // Almacena una referencia al objeto marcado.
        markedObject = objToMark;

        // Almacena el color original del objeto marcado.
        originalColor = markedObject.GetComponent<Renderer>().material.color;

        // Cambia el color del objeto marcado al color deseado (en este caso, verde).
        markedObject.GetComponent<Renderer>().material.color = markedColor;

        // Activa el estado de marcado.
        isMarked = true;
    }

    void StartAttraction()
    {
        isAttracting = true;
    }

    void StopAttraction()
{
    if (markedObject != null)
    {
        // Comprueba si el objeto está a una distancia 'distanceStop' del personaje antes de detenerlo en seco.
        float distanceToPlayer = Vector3.Distance(transform.position, markedObject.transform.position);
        if (distanceToPlayer <= distanceStop)
        {
            markedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // Restaura el color original del objeto marcado y desactiva el estado de marcado y atracción.
            markedObject.GetComponent<Renderer>().material.color = originalColor;
            markedObject = null;
            isMarked = false;
            isAttracting = false;
        }


    }
}

    void AttractObject()
    {
        if (markedObject != null)
        {
            // Calcula la dirección desde el objeto marcado hacia el personaje.
            Vector3 direction = transform.position - markedObject.transform.position;

            // Aplica la fuerza de atracción.
            markedObject.GetComponent<Rigidbody>().AddForce(direction.normalized * attractionForce, ForceMode.Force);
        }
    }
}


