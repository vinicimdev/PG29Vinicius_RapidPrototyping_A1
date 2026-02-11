using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Prototype3 : MonoBehaviour
{
    [Header("Mouse Follow Settings")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float leftBound = -8f;
    [SerializeField] private float rightBound = 8f;
    [SerializeField] private float topBound = 4f;
    [SerializeField] private float bottomBound = -4f;

    [Header("Auto Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.3f;

    private float nextFireTime = 0f;
    private Camera mainCamera;
    private Vector3 targetPosition;

    // Powerup variables - 4 stages
    private int bulletStage = 1;
    [SerializeField] private float stage3Angle = 25f;
    [SerializeField] private float stage4Angle = 35f;

    // Invincibility variables
    private bool isInvincible = false;
    private float invincibilityDuration = 5f;
    private float invincibilityEndTime = 0f;
    [SerializeField] private float blinkSpeed = 0.15f;
    [SerializeField] private Color invincibilityColor = new Color(0.3f, 0.7f, 1f, 1f);
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleMouseFollow();
        AutoShoot();
        CheckInvincibility();
    }

    void HandleMouseFollow()
    {
        if (Mouse.current != null && mainCamera != null)
        {
            // Get mouse position in world space
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.nearClipPlane));

            // Set target position (keep Z the same)
            targetPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);

            // Clamp target position to screen bounds
            targetPosition.x = Mathf.Clamp(targetPosition.x, leftBound, rightBound);
            targetPosition.y = Mathf.Clamp(targetPosition.y, bottomBound, topBound);

            // Smoothly move towards target
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    void AutoShoot()
    {
        // Automatically shoot at fixed intervals
        if (Time.time >= nextFireTime)
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

            case 4: // Five bullets: wider angles + center straight bullets
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            if (!isInvincible)
            {
                GameManager.Instance.TakeDamage(20);
            }
            Destroy(other.gameObject);
        }
    }
}
