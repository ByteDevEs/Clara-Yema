using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Dash : MonoBehaviour
{
    PlayerController moveScript;

    public float dashSpeed;
    public float dashTime;
    public float dashCooldown = 1;
    float timer;

    //Guarda en moveScript el script de PlayerController
    private void Start()
    {
        moveScript = GetComponent<PlayerController>();

    }

    //Cuando se le da al c�rculo comienza la subrutina de dash
    public void CircleDash(InputAction.CallbackContext context)
    {
        if(timer >= dashCooldown)
        {
            StartCoroutine(DashI());
            timer = 0;
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }

    IEnumerator DashI()
    {
        float startTime = Time.time;

        //Mientras que el Tiempo del dash efectuado sea menor al que queremos que dure le aplica una fuerza al personaje en la direcci�n en la que se mueve
        while (Time.time < startTime + dashTime)
        {
            moveScript.controller.Move(moveScript.move * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
