using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 4f;         // Max speed
    public float moveSpeedMultiplier = 1f;
    public float drag = 8f;              // How quickly you slow down

    private Rigidbody rb;
    private Vector3 inputDirection;
    private Camera mainCam;

    [SerializeField]
    PlayerInput playerInput;
    [SerializeField]
    Vector2 moveInput;


    [Header("animation")]
    [SerializeField]
    Animator animator;
    [SerializeField]
    string runningAnimationName = "Running";

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        

       
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
        mainCam = Camera.main;
    }

    public void onAttackCanelled(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            // Handle attack cancel logic here
            Debug.Log("Attack canceled");
        }
    }
    public void Onattack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Handle attack logic here
            Debug.Log("Attack performed");
        }
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

        if (animator != null)
        {
            animator.SetFloat(runningAnimationName, inputDirection.magnitude);
            if (inputDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    void FixedUpdate()
    {
        // Only set the horizontal velocity, keep y for gravity/knockback
        Vector3 velocity = rb.linearVelocity;
        Vector3 targetVelocity = inputDirection * moveSpeed * moveSpeedMultiplier;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        // Face movement direction if moving
        Vector3 lookDir = new Vector3(inputDirection.x, 0, inputDirection.z);
        if (lookDir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}