using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private float powerupDropChance = 0.2f; // 20% chance

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move bullet upward
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meteor"))
        {
            // Destroy the meteor and add score
            Meteor meteor = other.GetComponent<Meteor>();
            if (meteor != null)
            {
                GameManager.Instance.AddScore(meteor.scoreValue);
            }

            // Chance to spawn powerup
            if (powerupPrefab != null && Random.value <= powerupDropChance)
            {
                Instantiate(powerupPrefab, other.transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}