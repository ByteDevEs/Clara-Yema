using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moverse : MonoBehaviour
{
    public float speed;         // Velocidad del personaje
    public float rotationSpeed; // Velocidad de giro
    public Rigidbody rb;        // El Rigidbody del personaje
    public bool isGrounded = true; // Si está en el suelo

    void Start()
    {
        // Obtener el componente Rigidbody del GameObject
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movimiento
        float horizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");

        // Crear un vector de dirección a partir de las entradas de movimiento
        Vector3 moveDirection = new Vector3(horizontalMove, 0, VerticalMove);
        moveDirection.Normalize(); // Normalizar para evitar movimiento más rápido en diagonal
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World); // Mover el personaje

        if (moveDirection != Vector3.up)
        {
            // Calcular la rotación para mirar en la dirección de movimiento
            Quaternion toRotate = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }

        // Salto solo si está en el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Aplicar una fuerza de impulso hacia arriba para el salto
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            isGrounded = false; // Indicar que el personaje ya no está en el suelo
        }
    }

    // Método que se llama cuando el personaje colisiona con otro objeto
    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.name == "Floor") // Comprobar si la colisión es con el objeto "Floor"
        {
            isGrounded = true; // El personaje está en el suelo
        }
    }
}