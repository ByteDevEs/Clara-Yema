using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Dash : MonoBehaviour
{
    PlayerController moveScript;

    public float dashSpeed;
    public float dashTime;


    //Guarda en moveScript el script de PlayerController
    private void Start()
    {
        moveScript = GetComponent<PlayerController>();

    }

    //Cuando se le da al círculo comienza la subrutina de dash
    public void CircleDash(InputAction.CallbackContext context)
    {
        StartCoroutine(DashI());
    }
    private void Update()
    {

    }

    IEnumerator DashI()
    {
        float startTime = Time.time;

        //Mientras que el Tiempo del dash efectuado sea menor al que queremos que dure le aplica una fuerza al personaje en la dirección en la que se mueve
        while (Time.time < startTime + dashTime)
        {
            moveScript.controller.Move(moveScript.move * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
