using UnityEngine;

public class Bullet_Prototype3 : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    [Header("Powerup Drops")]
    [SerializeField] private GameObject bulletUpgradePrefab;
    [SerializeField] private GameObject invincibilityPrefab;
    [SerializeField] private float bulletUpgradeDropChance = 0.15f; // 15% chance
    [SerializeField] private float invincibilityDropChance = 0.10f; // 10% chance

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move bullet in the direction it's facing (handles angled shots)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Damage the enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);

                // Try to spawn powerups
                float randomValue = Random.value;

                // Check for bullet upgrade first
                if (bulletUpgradePrefab != null && randomValue <= bulletUpgradeDropChance)
                {
                    Instantiate(bulletUpgradePrefab, other.transform.position, Quaternion.identity);
                }
                // Check for invincibility (separate roll)
                else if (invincibilityPrefab != null && Random.value <= invincibilityDropChance)
                {
                    Instantiate(invincibilityPrefab, other.transform.position, Quaternion.identity);
                }
            }

            Destroy(gameObject);
        }
    }
}
