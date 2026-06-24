using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public bool doubleJumpEnabled;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool canDoubleJump;
    private float speedBoostMultiplier = 1f;
    private Coroutine speedBoostCoroutine;
    private Coroutine shieldCoroutine;
    private Coroutine doubleJumpCoroutine;
    private bool hasShield;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerTransform = transform;
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        Vector3 worldVelocity = transform.TransformDirection(direction) * moveSpeed * speedBoostMultiplier;
        rb.velocity = new Vector3(worldVelocity.x, rb.velocity.y, worldVelocity.z);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (doubleJumpEnabled && canDoubleJump)
            {
                canDoubleJump = false;
                Jump();
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            if (doubleJumpEnabled)
            {
                canDoubleJump = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collectible>(out Collectible collectible))
        {
            collectible.Collect();
            return;
        }

        if (other.TryGetComponent<PowerUp>(out PowerUp powerUp))
        {
            powerUp.OnPickedUp(gameObject);
            return;
        }
    }

    public void TakeDamage(int amount, float knockbackStrength, Vector3 sourcePosition)
    {
        if (hasShield)
        {
            hasShield = false;
            UIManager.Instance?.ShowMessage("Shield blocked the hit!");
            return;
        }

        GameManager.Instance?.Damage(amount);
        Vector3 knockbackDirection = (transform.position - sourcePosition).normalized;
        rb.AddForce((knockbackDirection + Vector3.up * 0.5f) * knockbackStrength, ForceMode.VelocityChange);
    }

    public void EnableSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        speedBoostMultiplier = multiplier;
        UIManager.Instance?.ShowMessage("Speed boost active!");
        yield return new WaitForSeconds(duration);
        speedBoostMultiplier = 1f;
        UIManager.Instance?.ShowMessage("Speed boost ended.");
    }

    public void EnableShield(float duration)
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        shieldCoroutine = StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        hasShield = true;
        UIManager.Instance?.ShowMessage("Shield activated!");
        yield return new WaitForSeconds(duration);
        hasShield = false;
        UIManager.Instance?.ShowMessage("Shield ended.");
    }

    public void EnableDoubleJump(float duration)
    {
        if (doubleJumpCoroutine != null)
        {
            StopCoroutine(doubleJumpCoroutine);
        }

        doubleJumpCoroutine = StartCoroutine(DoubleJumpRoutine(duration));
    }

    private IEnumerator DoubleJumpRoutine(float duration)
    {
        doubleJumpEnabled = true;
        canDoubleJump = true;
        UIManager.Instance?.ShowMessage("Double jump ready!");
        yield return new WaitForSeconds(duration);
        doubleJumpEnabled = false;
        canDoubleJump = false;
        UIManager.Instance?.ShowMessage("Double jump ended.");
    }
}
