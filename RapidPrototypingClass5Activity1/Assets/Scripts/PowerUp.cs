using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        DoubleShot
    }

    [SerializeField] private PowerupType powerupType = PowerupType.DoubleShot;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float lifetime = 8f;

    void Start()
    {
        // Destroy powerup after some time if not collected
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move powerup downward
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply powerup effect
            PlayerControllerBhvr player = other.GetComponent<PlayerControllerBhvr>();
            if (player != null)
            {
                switch (powerupType)
                {
                    case PowerupType.DoubleShot:
                        player.ActivateDoubleShot();
                        break;
                }
            }

            // Destroy the powerup
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        // Destroy powerup when it goes off screen
        Destroy(gameObject);
    }
}