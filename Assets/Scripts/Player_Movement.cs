using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 4f;         // Max speed
    public float drag = 8f;              // How quickly you slow down

    private Rigidbody rb;
    private Vector3 inputDirection;
    private Camera mainCam;

    [SerializeField]
    PlayerInput playerInput;
    [SerializeField]
    Vector2 moveInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
        mainCam = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    void Update()
    {
        // Get input
        float h = moveInput.x; // Horizontal input
        float v = moveInput.y; // Vertical input

        // Camera-relative movement
        if (mainCam != null)
        {
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            inputDirection = (camForward * v + camRight * h).normalized;
        }
        else
        {
            inputDirection = new Vector3(h, 0, v).normalized;
        }
    }

    void FixedUpdate()
    {
        // Only set the horizontal velocity, keep y for gravity/knockback
        Vector3 velocity = rb.linearVelocity;
        Vector3 targetVelocity = inputDirection * moveSpeed;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;
    }
}