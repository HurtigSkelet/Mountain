using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Weapon : MonoBehaviour
{
    PlayerInput playerInput;
    public GameObject Hitbox;
    public float Damage = 10f;
    public float MinDamage = 3f; // Minimum damage tapping charge
    public float KnockbackForce = 5f;
    public float AttackCooldown = 1f; // Time in seconds between attacks
    private float lastAttackTime = 0f;
    private float currentCharge = 0f;
    public float MaxCharge = 0.5f; // Maximum charge time in seconds
    public AudioClip AttackSound;
    private AudioSource audioSource;
    public DecalProjector Decal;

    private bool isCharging = false;

    // InputAction reference (set up in Inspector or via code)
    public InputAction attackAction;

    public Player_Movement playerMovement; // Assign in Inspector

    void OnEnable()
    {
        if (attackAction != null)
        {
            attackAction.Enable();
            attackAction.started += OnAttackStarted;
            attackAction.canceled += OnAttackCanceled;
        }
    }

    void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.Disable();
            attackAction.started -= OnAttackStarted;
            attackAction.canceled -= OnAttackCanceled;
        }
    }

    void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (Time.time >= lastAttackTime + AttackCooldown)
        {
            Decal.enabled = true; //Placeholder, want to implement MaterialPropertyBlocks
            currentCharge = 0f;
            isCharging = true;
        }
    }

    void OnAttackCanceled(InputAction.CallbackContext context)
    {
        if (isCharging)
        {
            float ProposedDamage = 0;
            float charge = Mathf.Min(currentCharge, MaxCharge);

            if (charge >= MaxCharge)
            {
                ProposedDamage = Damage;
            }
            else
            {
                ProposedDamage = Mathf.Lerp(MinDamage, Damage * 0.5f, charge / MaxCharge);
            }
            // Reset charge and cooldown timer
            lastAttackTime = Time.time;
            isCharging = false;

            // Trigger attack logic
            Decal.enabled = false; //Placeholder, want to implement MaterialPropertyBlocks
            Debug.Log("You hit for: " + ProposedDamage);
        }
    }

    void Update()
    {
        // If attack is held and not already charging, start charging (if cooldown allows)
        if (attackAction != null && attackAction.ReadValue<float>() > 0.5f && !isCharging)
        {
            if (Time.time >= lastAttackTime + AttackCooldown)
            {
                OnAttackStarted(new InputAction.CallbackContext());
            }
        }

        if (isCharging)
        {
            currentCharge += Time.deltaTime;

            // Slow down movement as you charge
            if (playerMovement != null)
            {
                float chargeRatio = Mathf.Clamp01(currentCharge / MaxCharge);
                // At 0 charge: 1x speed, at full charge: 0.5x speed
                playerMovement.moveSpeedMultiplier = Mathf.Lerp(1f, 0.5f, chargeRatio);
            }
        }
        else
        {
            // Reset movement speed when not charging
            if (playerMovement != null)
            {
                playerMovement.moveSpeedMultiplier = 1f;
            }
        }
    }
}