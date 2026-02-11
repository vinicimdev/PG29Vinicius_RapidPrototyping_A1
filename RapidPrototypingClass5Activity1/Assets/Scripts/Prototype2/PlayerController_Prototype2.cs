using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Prototype2 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float leftBound = -8f;
    [SerializeField] private float rightBound = 8f;
    [SerializeField] private float topBound = 4f;
    [SerializeField] private float bottomBound = -4f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.3f;

    private float nextFireTime = 0f;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool isShooting;

    // Powerup variables - 4 stages
    private int bulletStage = 1; // 1 = single, 2 = double, 3 = triple angled, 4 = quintuple
    [SerializeField] private float stage3Angle = 25f; // Angle for stage 3 side bullets
    [SerializeField] private float stage4Angle = 35f; // Wider angle for stage 4

    // Invincibility variables
    private bool isInvincible = false;
    private float invincibilityDuration = 5f;
    private float invincibilityEndTime = 0f;
    [SerializeField] private float blinkSpeed = 0.15f; // How fast to blink when invincible
    [SerializeField] private Color invincibilityColor = new Color(0.3f, 0.7f, 1f, 1f); // Cyan/blue color
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        CheckInvincibility();
    }

    void HandleMovement()
    {
        // Get input from new Input System (8 directions)
        movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                movementInput.y = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                movementInput.y = -1f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                movementInput.x = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                movementInput.x = 1f;
        }

        // Normalize diagonal movement so it's not faster
        movementInput.Normalize();

        // Move the player
        rb.linearVelocity = movementInput * moveSpeed;

        // Clamp position to screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, leftBound, rightBound);
        float clampedY = Mathf.Clamp(transform.position.y, bottomBound, topBound);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void HandleShooting()
    {
        isShooting = false;

        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
            isShooting = true;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            isShooting = true;

        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        switch (bulletStage)
        {
            case 1: // Single bullet straight
                CreateBullet(firePoint.position, 0f);
                break;

            case 2: // Two bullets straight
                CreateBullet(firePoint.position + Vector3.left * 0.15f, 0f);
                CreateBullet(firePoint.position + Vector3.right * 0.15f, 0f);
                break;

            case 3: // Three bullets: left angled, center straight, right angled
                CreateBullet(firePoint.position + Vector3.left * 0.3f, -stage3Angle);
                CreateBullet(firePoint.position, 0f);
                CreateBullet(firePoint.position + Vector3.right * 0.3f, stage3Angle);
                break;

            case 4: // Five bullets: wider angles + center straight bullet
                CreateBullet(firePoint.position + Vector3.left * 0.4f, -stage4Angle);
                CreateBullet(firePoint.position + Vector3.left * 0.2f, 0f);
                CreateBullet(firePoint.position, 0f);
                CreateBullet(firePoint.position + Vector3.right * 0.2f, 0f);
                CreateBullet(firePoint.position + Vector3.right * 0.4f, stage4Angle);
                break;
        }
    }

    void CreateBullet(Vector3 position, float angleOffset)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        // Rotate bullet based on angle
        if (angleOffset != 0f)
        {
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angleOffset);
        }
    }

    public void UpgradeBullets()
    {
        bulletStage++;
        bulletStage = Mathf.Clamp(bulletStage, 1, 4);
        Debug.Log("Bullet Stage: " + bulletStage);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meteor"))
        {
            if (!isInvincible)
            {
                GameManager.Instance.TakeDamage(20);
            }
            Destroy(other.gameObject);
        }
    }

    void CheckInvincibility()
    {
        if (isInvincible)
        {
            // Flash between original color and blue
            if (spriteRenderer != null)
            {
                float t = Mathf.PingPong(Time.time / blinkSpeed, 1f);
                spriteRenderer.color = Color.Lerp(originalColor, invincibilityColor, t);
            }

            // Check if invincibility expired
            if (Time.time >= invincibilityEndTime)
            {
                isInvincible = false;
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = originalColor;
                }
                Debug.Log("Invincibility ended!");
            }
        }
    }

    public void ActivateInvincibility()
    {
        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;
        Debug.Log("Invincibility activated!");
    }
}