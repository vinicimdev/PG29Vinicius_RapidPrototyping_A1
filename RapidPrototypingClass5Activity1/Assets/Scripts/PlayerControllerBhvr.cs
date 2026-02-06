using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerBhvr : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float leftBound = -8f;
    [SerializeField] private float rightBound = 8f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float bulletSpacing = 0.3f; // Distance between bullets in double shot

    private float nextFireTime = 0f;
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isShooting;

    // Powerup variables
    private bool doubleShotActive = false;
    private float doubleShotDuration = 10f;
    private float doubleShotEndTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        CheckPowerupTimer();
    }

    void HandleMovement()
    {
        // Get input from new Input System
        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontalInput = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontalInput = 1f;
        }

        // Move the player
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, 0f);
        rb.linearVelocity = movement;

        // Clamp position to screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, leftBound, rightBound);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
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
        if (bulletPrefab != null && firePoint != null)
        {
            if (doubleShotActive)
            {
                // Fire two bullets side by side
                Vector3 leftPosition = firePoint.position + Vector3.left * bulletSpacing;
                Vector3 rightPosition = firePoint.position + Vector3.right * bulletSpacing;

                Instantiate(bulletPrefab, leftPosition, Quaternion.identity);
                Instantiate(bulletPrefab, rightPosition, Quaternion.identity);
            }
            else
            {
                // Fire single bullet
                Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            }
        }
    }

    void CheckPowerupTimer()
    {
        // Check if double shot powerup has expired
        if (doubleShotActive && Time.time >= doubleShotEndTime)
        {
            doubleShotActive = false;
            Debug.Log("Double Shot ended!");
        }
    }

    public void ActivateDoubleShot()
    {
        doubleShotActive = true;
        doubleShotEndTime = Time.time + doubleShotDuration;
        Debug.Log("Double Shot activated!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meteor"))
        {
            GameManager.Instance.TakeDamage(20);
            Destroy(other.gameObject);
        }
    }
}