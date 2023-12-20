using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    PlayerController moveScript;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown = 1;
    public GameObject dashParticle; // Referencia al objeto DashParticle

    float timer;
    bool isDashing = false;

    private void Start()
    {
        moveScript = GetComponent<PlayerController>();
        dashParticle.SetActive(false); // Asegúrate de que el objeto DashParticle esté desactivado al inicio
    }

    public void CircleDash(InputAction.CallbackContext context)
    {
        if (timer >= dashCooldown && !isDashing)
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
        isDashing = true;
        float startTime = Time.time;
        dashParticle.SetActive(true); // Activa el objeto DashParticle al iniciar el dash

        while (Time.time < startTime + dashTime)
        {
            moveScript.controller.Move(moveScript.move * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        dashParticle.SetActive(false); // Desactiva el objeto DashParticle al finalizar el dash
    }
}