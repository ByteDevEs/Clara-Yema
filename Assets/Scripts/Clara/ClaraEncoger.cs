using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ClaraEncoger : MonoBehaviour
{
    // Factor de escala para el collider vertical
    public float verticalScale = 0.5f;

    // Referencia al collider del personaje
    private CapsuleCollider characterCollider;

    // Referencia al transform del modelo del personaje
    private Transform characterModelTransform;

    PlayerController moveScript;

    [SerializeField] protected Animator animator;

    private bool isShrinking = false;

    void Start()
    {

        // Obtener el collider del personaje (aseg�rate de que el GameObject tenga un characterCollider adjunto)
        characterCollider = GetComponent<CapsuleCollider>();
        // Obtener el transform del modelo del personaje
        characterModelTransform = transform.GetChild(0);

        moveScript = GetComponent<PlayerController>();

        if (characterCollider == null)
        {
            Debug.LogError("Este script requiere un BoxCollider2D en el GameObject.");
        }

        if (characterModelTransform == null)
        {
            Debug.LogError("No se encontr� el modelo del personaje. Aseg�rate de que el modelo sea el primer hijo del GameObject.");
        }
    }
    public void OnCircle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Comenzar la rutina de encogimiento cuando el bot�n se presiona
            StartCoroutine(Shrink());
        }
        else if (context.canceled)
        {
            // Detener el encogimiento cuando el bot�n se suelta
            StopCoroutine(Shrink());
            isShrinking = false;

            // Volver al tama�o normal
            ResetSize();
        }
    }
    IEnumerator Shrink()
    {
        // Evitar que se inicie m�ltiples veces mientras el bot�n sigue presionado
        if (!isShrinking)
        {
            isShrinking = true;

            animator.SetBool("Duck", true);

            // Encoger
            Vector3 originalScale = characterCollider.transform.localScale;
            characterCollider.transform.localScale = new Vector3(originalScale.x, originalScale.y * verticalScale, originalScale.z);

            Vector3 scale = characterModelTransform.localScale;
            scale.y *= verticalScale;
            characterModelTransform.localScale = scale;
        }

        yield return null;

    }

    void ResetSize()
    {
        // Restablecer al tama�o normal
        animator.SetBool("Duck", false);
        //Vector3 originalScale = characterCollider.transform.localScale;
        //characterCollider.transform.localScale = new Vector3(originalScale.x, originalScale.y / verticalScale, originalScale.z);

        //Vector3 scale = characterModelTransform.localScale;
        //scale.y /= verticalScale;
        //characterModelTransform.localScale = scale;
    }
}
