using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Atraer : MonoBehaviour
{
    public float shootingRange = 10f;      // Rango m�ximo de disparo.
    public float attractionForce = 5f;     // Fuerza de atracci�n.
    public float attractionDistance = 1f;  // Distancia a la que se desmarca el objeto.
    public LayerMask shootableLayer;       // Capa de objetos que se pueden disparar.
    public Color markedColor = Color.green; // Color cuando se marca el objeto.
    public float distanceStop = 1f;
    private GameObject markedObject;       // Objeto actualmente marcado.
    private Color originalColor;           // Color original del objeto marcado.
    private bool isMarked = false;         // Estado de marcado.
    private bool isAttracting = false;     // Estado de atracci�n.
    public LayerMask objectsToAttract;        // Capas de los objetos que se lanzar�n.
    [SerializeField] protected Animator animator;
    private Ray ray;
    public void SkillSquare(InputAction.CallbackContext context)
    {
        Shoot();
    }

    void Update()
    {
        // Atraer el objeto marcado hacia el personaje si se activa la atracci�n.
        if (isAttracting && markedObject != null)
        {
            AttractObject();

        }
    }

    IEnumerator NotGrab()
    {
        animator.SetBool("Grab",false );
        return null;
    }
    void Shoot()
    {
        animator.SetBool("Grab", true);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, shootingRange, objectsToAttract))
        {
            GameObject hitObject = hit.transform.gameObject;
            Rigidbody rb = hitObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                MarkObject(hitObject);
            }
        }else{Invoke("NotGrab", 1f);}

    }

    IEnumerator StartAttraction()
    {
        isAttracting = true;
        return null;
    }

    void MarkObject(GameObject objToMark)
    {
        // Almacena una referencia al objeto marcado.
        markedObject = objToMark;

        // Activa el estado de marcado.
        isMarked = true;
        Invoke("StartAttraction",1f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == markedObject)
        {
            StopAttraction();
        }
    }

    void StopAttraction()
    {

        // Comprueba si el objeto est� a una distancia 'distanceStop' del personaje antes de detenerlo en seco.
        isAttracting = false;
        Rigidbody rb = markedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        isMarked = false;
        animator.SetBool("Grab",false );

    }

    void AttractObject()
    {
        if (markedObject != null)
        {
            // Calcula la direcci�n desde el objeto marcado hacia el personaje.
            Vector3 direction = transform.position - markedObject.transform.position;

            // Aplica la fuerza de atracci�n.
            markedObject.GetComponent<Rigidbody>().AddForce(direction.normalized * attractionForce, ForceMode.Force);

        }
    }
}
