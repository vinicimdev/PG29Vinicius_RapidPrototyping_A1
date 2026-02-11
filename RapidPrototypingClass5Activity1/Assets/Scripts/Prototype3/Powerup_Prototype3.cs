using UnityEngine;

public class Powerup_Prototype3 : MonoBehaviour
{
    public enum PowerupType
    {
        BulletUpgrade,
        Invincibility
    }

    [SerializeField] private PowerupType powerupType = PowerupType.BulletUpgrade;
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
            PlayerController_Prototype3 player = other.GetComponent<PlayerController_Prototype3>();
            if (player != null)
            {
                switch (powerupType)
                {
                    case PowerupType.BulletUpgrade:
                        player.UpgradeBullets();
                        break;
                    case PowerupType.Invincibility:
                        player.ActivateInvincibility();
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