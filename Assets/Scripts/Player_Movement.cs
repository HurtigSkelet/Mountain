using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 3f;         // Max speed
    public float drag = 4f;              // How quickly you slow down

    private Rigidbody rb;
    private Vector3 inputDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
    }

    void Update()
    {
        // Get input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(h, 0, v).normalized;
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