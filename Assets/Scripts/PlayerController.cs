using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(HealthController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public Vector3 move;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    
    [Header("HealthSystem")]
    [SerializeField] protected HealthController healthController;
    
    [Header("Animations")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip idleClip;
    [SerializeField] protected AnimationClip andarClip;
    [SerializeField] protected AnimationClip correrClip;
    [SerializeField] protected AnimationClip saltarClip;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //jumped = context.ReadValue<bool>();
        jumped = context.action.triggered;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (!groundedPlayer)
        {
            
            animator.Play(saltarClip.name);
        }
        else if (jumped && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        else if (move != Vector3.zero && !jumped)
        {
            if (move.magnitude > 0.5f)
            {
                animator.Play(correrClip.name);
            }
            else
            {
                animator.Play(andarClip.name);
            }
        }
        else
        {
            animator.Play(idleClip.name);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Damager"))
        {
            print("TriggerStay");
            healthController.TakeDamage(Time.deltaTime);
        }
    }
}
