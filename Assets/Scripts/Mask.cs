using UnityEngine;

public class AreaDetector : MonoBehaviour
{
    public GameObject maskObject;
    public LayerMask characterLayer;
    public float detectionRadius = 5f;

    private void Update()
    {
        // Obtiene todos los colliders dentro del área
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        bool characterDetected = false;

        // Itera a través de los colliders encontrados
        foreach (Collider col in colliders)
        {
            // Comprueba si el collider pertenece a la capa del personaje
            if (col.gameObject.layer == characterLayer)
            {
                characterDetected = true;
                break;
            }
        }

        // Si se detecta un personaje, desactiva el objeto mask
        if (characterDetected)
        {
            maskObject.SetActive(false);
        }
        else
        {
            // Si no se detecta un personaje, activa el objeto mask
            maskObject.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja un gizmo para visualizar el área de detección en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
