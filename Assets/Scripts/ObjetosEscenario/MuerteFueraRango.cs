using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (FindObjectOfType<SartenController>().state != SartenController.States.WaitingForPlayers)
        {
            //Move player to the further spawner which is not blocked by any collision
            Vector3[] spawners = GameObject.FindGameObjectsWithTag("Spawner").Select(x => x.transform.position).ToArray();
        
            //Remove the spawner which is blocked by a collision
            foreach (Vector3 spawner in spawners)
            {
                if (Physics.BoxCast(spawner, Vector3.one * 0.5f, Vector3.up, Quaternion.identity, 1f))
                {
                    spawners = spawners.Where(x => x != spawner).ToArray();
                }
            }
        
            //Get the furthest spawner
            Vector3 furthestSpawner = spawners.OrderByDescending(x => Vector3.Distance(x, transform.position)).First();
        
            //Move the player to the furthest spawner
            transform.position = furthestSpawner;
        }
        else
        {
            transform.position = lastPosition;
        }
        resetting = false;
    }
}
