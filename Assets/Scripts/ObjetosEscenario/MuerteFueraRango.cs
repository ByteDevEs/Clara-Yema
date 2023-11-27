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

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        lastPosition = transform.position; // Inicializa la �ltima posici�n con la posici�n inicial del personaje.
    }
    
    bool resetting = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("InRange"))
        {
            lastPosition = transform.position;
        }
        else if (other.CompareTag("OutOfRange")) //Este es el tag del agua, si se cambia a otro, renombrarlo
        {
            if (resetting)
                return;
            resetting = true;
            // El personaje ha ca�do al agua, as� que lo teleportamos a la �ltima posici�n v�lida.
            Respawn();
            GetComponent<HealthController>().TakeDamage(1);
            //Debug.Log("TP fuera del agua");
        }
    }
    
    private void Respawn()
    {
        try
        {
            Camera cam = FindObjectOfType<SplitScreenEffect>().Screens.Find(x => x.Target == transform).Camera;
            GameObject gO = Instantiate(blackScreenGameObject, transform.position, Quaternion.identity);
            gO.GetComponent<DeathCamera>().canvas.renderMode = RenderMode.ScreenSpaceCamera;
            gO.GetComponent<DeathCamera>().canvas.worldCamera = cam;
            gO.GetComponent<DeathCamera>().muerteFueraRango = this;
        }
        catch(System.Exception e)
        {
            Invoke("ResetPosition", 0.25f);
        }
    }
    
    public void ResetPosition()
    {
        transform.position = lastPosition;
        resetting = false;
    }
}
