using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(UIHealthController), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public Rigidbody rb;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public Vector3 move;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    
    float damageDelay = 0.5f;
    float damageDelayTimer = 0f;
    
    
    [Header("HealthSystem")]
    [SerializeField] protected HealthController healthController;
    
    [Header("Animations")]
    [SerializeField] protected Animator animator;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(cameraTransform == null)
            return;
        Vector3 input = context.ReadValue<Vector2>();
        //Get the forward direction of the camera on the x-z plane
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();
        
        //Change movement input to be relative to camera
        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();
        
        movementInput = new Vector2(input.x * right.x + input.y * forward.x, input.x * right.z + input.y * forward.z);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //jumped = context.ReadValue<bool>();
        jumped = context.action.triggered;
    }

    // Para el script de caer al agua o fuera del mapa
    public bool Grounded() { return groundedPlayer; }

    void Update()
    {
        if (damageDelayTimer > 0)
        {
            damageDelayTimer -= Time.deltaTime;
        }
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * (Time.deltaTime * playerSpeed));

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (groundedPlayer)
        {
            animator.SetBool("Jump", false);
        }
        
        if (groundedPlayer && jumped)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool("Jump", true);
        }
        
        animator.SetFloat("Speed", move.sqrMagnitude);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (invulnerable)
            return;
        if (other.gameObject.CompareTag("Damager"))
        {
            SartenController.States state = other.gameObject.GetComponentInParent<SartenController>().state;

            if ((state == SartenController.States.LaunchSpatula ||
                    state == SartenController.States.Smash ||
                    state == SartenController.States.Mix))
            {
                MakeInvulnerable(other);
                if(damageDelayTimer > 0)
                    return;
                healthController.TakeDamage(1);
                damageDelayTimer = damageDelay;
            }
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            if(other.gameObject.GetComponentInParent<SartenController>().state == SartenController.States.Dash || other.gameObject.GetComponentInParent<SartenController>().state == SartenController.States.Stomp)
            {
                MakeInvulnerable(other);
                if(damageDelayTimer > 0)
                    return;
                healthController.TakeDamage(1);
                damageDelayTimer = damageDelay;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            if(other.gameObject.GetComponentInParent<SartenController>().state == SartenController.States.Dash || other.gameObject.GetComponentInParent<SartenController>().state == SartenController.States.Stomp)
            {
                Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
                MakeInvulnerable(other.collider);
                if(damageDelayTimer > 0)
                    return;
                healthController.TakeDamage(1);
                damageDelayTimer = damageDelay;
            }
        }
    }

    private SkinnedMeshRenderer[] vulnerabilityMeshes;
    bool invulnerable = false;
    public void MakeInvulnerable(Collider other)
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
        
        vulnerabilityMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        invulnerable = true;
        int times = 10;
        for (int i = 0; i < times; i++)
        {
            Invoke("MakeInvisible", 0.2f * i);
            Invoke("MakeVisible", 0.2f * i + 0.1f);
        }
        
        Invoke("EndInvulnerability", 0.1f * times);
        
        Physics.IgnoreCollision(other, GetComponent<Collider>(), false);
    }
    
    void MakeVisible()
    {
        foreach (SkinnedMeshRenderer mesh in vulnerabilityMeshes)
        {
            mesh.enabled = true;
        }
    }
    
    void MakeInvisible()
    {
        foreach (SkinnedMeshRenderer mesh in vulnerabilityMeshes)
        {
            mesh.enabled = false;
        }
    }
    
    void EndInvulnerability()
    {
        invulnerable = false;
    }
}
