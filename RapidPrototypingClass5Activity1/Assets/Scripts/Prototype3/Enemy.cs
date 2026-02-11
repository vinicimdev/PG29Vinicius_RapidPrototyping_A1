using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 3f; // How far to move left/right
    [SerializeField] private bool moveHorizontally = true;
    [SerializeField] private float downwardSpeed = 0.5f; // Speed moving down toward player

    [Header("Shooting")]
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float shootDelay = 1f; // Delay before first shot

    [Header("Stats")]
    [SerializeField] private int health = 1;
    [HideInInspector] public int scoreValue = 100;

    private Vector3 startPosition;
    private float moveDirection = 1f;
    private float nextShootTime;
    private bool canShoot = false;

    void Start()
    {
        startPosition = transform.position;
        nextShootTime = Time.time + shootDelay;

        // Start shooting after delay
        Invoke(nameof(EnableShooting), shootDelay);
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        // Move down slowly
        transform.Translate(Vector2.down * downwardSpeed * Time.deltaTime);

        if (!moveHorizontally)
            return;

        // Move left and right
        transform.Translate(Vector2.right * moveDirection * moveSpeed * Time.deltaTime);

        // Check if moved too far from start position
        float distanceFromStart = transform.position.x - startPosition.x;

        if (Mathf.Abs(distanceFromStart) >= moveDistance)
        {
            moveDirection *= -1f; // Reverse direction
        }
    }

    void HandleShooting()
    {
        if (!canShoot || enemyBulletPrefab == null || firePoint == null)
            return;

        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootInterval;
        }
    }

    void Shoot()
    {
        Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
    }

    void EnableShooting()
    {
        canShoot = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        // Destroy enemy if it goes off screen
        Destroy(gameObject, 2f);
    }
}