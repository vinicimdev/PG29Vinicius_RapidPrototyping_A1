using UnityEngine;

public class Meteor : MonoBehaviour
{
    [HideInInspector] public int scoreValue;

    private float speed;
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set the meteor's velocity
        rb.linearVelocity = direction * speed;

        // Add random rotation
        rb.angularVelocity = Random.Range(-100f, 100f);

        // Destroy after falling off screen
        Destroy(gameObject, 10f);
    }

    public void Initialize(float meteorSpeed, Vector2 meteorDirection, int score)
    {
        speed = meteorSpeed;
        direction = meteorDirection;
        scoreValue = score;
    }

    void OnBecameInvisible()
    {
        // Destroy meteor when it goes off screen
        Destroy(gameObject, 1f);
    }
}