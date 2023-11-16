using System.Collections;
using System.Collections.Generic;
using ProjectDawn.SplitScreen;
using UnityEngine;

public class MuerteFueraRango : MonoBehaviour
{
    [SerializeField]
    private GameObject blackScreenGameObject;
    
    private PlayerController playerController;
    private Vector3 lastPosition; // Almacena la �ltima posici�n v�lida del personaje.
    private bool dead;

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
            Respawn();
            //Debug.Log("TP fuera del agua");
        }
    }
    
    private void Respawn()
    {
        dead = true;
        GameObject gO = Instantiate(blackScreenGameObject, transform.position, Quaternion.identity);
        gO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        gO.GetComponent<Canvas>().worldCamera = FindObjectOfType<SplitScreenEffect>().Screens.Find(x => x.Target == transform).Camera;
        gO.GetComponent<DeathCamera>().muerteFueraRango = this;
    }
    
    public void ResetPosition()
    {
        transform.position = lastPosition;
        dead = false;
    }

    void Update()
    {
        // Actualizamos la �ltima posici�n v�lida en cada frame, pero evitamos actualizarla en el borde.
        if (playerController.Grounded() && !dead)
        {
            if (Vector3.Distance(transform.position, lastPosition) > 1f)
            {
                lastPosition = transform.position;
                //Debug.Log("�ltima posici�n v�lida actualizada: " + lastPosition);
            }
        }

    }

}
