using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    private PlayerInput playerInput;
    private InputAction walk;
    private InputAction jump;

    public float MoveSpeed = 5f;
    public float jumpHeight = 3;
    public float terminalVelocity = 150f;
    public float gravity = 15f;
    Vector3 velocity;
    public float inertiaDamping = 5f;
    private bool isGrounded = false;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        walk = playerInput.actions["Move"];
        jump = playerInput.actions["Jump"];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = walk.ReadValue<Vector2>();
        
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        bool movePressed = movement.sqrMagnitude > 0.001f;

        if (isGrounded && movePressed)
        {
            Vector3 moveDir = transform.right * movement.x + transform.forward * movement.y;
            Vector3 targetVelocity = moveDir * MoveSpeed;

            float t = 1f - Mathf.Exp(-inertiaDamping * Time.deltaTime);
            velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, t);
            velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, t);

        } else if (isGrounded && !movePressed)
        {
            // No input: smoothly decelerate horizontal velocity towards zero (braking)
            Vector3 targetVelocity = Vector3.zero;
            float brakingFactor = inertiaDamping * 2f; // stronger damping when stopping
            float t = 1f - Mathf.Exp(-brakingFactor * Time.deltaTime);
            velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, t);
            velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, t);

            // snap very small velocities to zero to avoid sliding
            if (Mathf.Abs(velocity.x) < 0.01f) velocity.x = 0f;
            if (Mathf.Abs(velocity.z) < 0.01f) velocity.z = 0f;
        } else
        {
            // In air: slight air control
            Vector3 moveDir = transform.right * movement.x + transform.forward * movement.y;
            Vector3 targetVelocity = moveDir * MoveSpeed * 0.9f; // reduced control in air
            float t = 1f - Mathf.Exp(-inertiaDamping * 0.1f * Time.deltaTime);
            velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, t);
            velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, t);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (jump.triggered && isGrounded)
        {   
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y = Mathf.Clamp(velocity.y + gravity * Time.deltaTime, -terminalVelocity, terminalVelocity);
        controller.Move(velocity * Time.deltaTime);



    }
}
