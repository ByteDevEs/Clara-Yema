using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atraer : MonoBehaviour
{
    public float shootingRange = 10f;      // Rango m�ximo de disparo.
    public float attractionForce = 5f;     // Fuerza de atracci�n.
    public float attractionDistance = 1f;  // Distancia a la que se desmarca el objeto.
    public LayerMask shootableLayer;       // Capa de objetos que se pueden disparar.
    public Color markedColor = Color.green; // Color cuando se marca el objeto.
    public string attractButton = "Fire1"; // Bot�n para atraer (cambia seg�n tu configuraci�n).

    private GameObject markedObject;       // Objeto actualmente marcado.
    private Color originalColor;           // Color original del objeto marcado.
    private bool isMarked = false;         // Estado de marcado.
    private bool isAttracting = false;     // Estado de atracci�n.

    void Update()
    {
        // Disparar un rayo desde la posici�n del personaje para detectar objetos en el rango de disparo.
        if (Input.GetButtonDown(attractButton))
        {
            // Si ya se ha marcado un objeto, activa o desactiva la atracci�n.
            if (isMarked)
            {
                ToggleAttraction();
            }
            else
            {
                Shoot();
            }
        }

        // Atraer el objeto marcado hacia el personaje si se activa la atracci�n.
        if (isAttracting && markedObject != null)
        {
            AttractObject();

            // Comprueba si el objeto est� lo suficientemente cerca para desmarcarlo.
            if (Vector3.Distance(transform.position, markedObject.transform.position) <= attractionDistance)
            {
                UnmarkObject();
            }
        }
    }

    void Shoot()
    {
        // Lanza un rayo desde la posici�n del personaje hacia adelante.
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Comprueba si el rayo golpea un objeto en la capa shootableLayer dentro del rango de disparo.
        if (Physics.Raycast(ray, out hit, shootingRange, shootableLayer))
        {
            MarkObject(hit.transform.gameObject);
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

    void UnmarkObject()
    {
        // Restaura el color original del objeto marcado.
        if (markedObject != null)
        {
            markedObject.GetComponent<Renderer>().material.color = originalColor;

            // Limpia la referencia al objeto marcado y desactiva el estado de marcado.
            markedObject = null;
            isMarked = false;
        }
    }

    void ToggleAttraction()
    {
        // Cambia el estado de atracci�n.
        isAttracting = !isAttracting;

        // Si se desactiva la atracci�n, restablece la velocidad del objeto marcado.
        if (!isAttracting && markedObject != null)
        {
            markedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void AttractObject()
    {
        if (markedObject != null)
        {
            // Calcula la direcci�n desde el objeto marcado hacia el personaje.
            Vector3 direction = transform.position - markedObject.transform.position;

            // Aplica una fuerza para atraer el objeto hacia el personaje.
            markedObject.GetComponent<Rigidbody>().AddForce(direction.normalized * attractionForce, ForceMode.Force);
        }
    }
}
