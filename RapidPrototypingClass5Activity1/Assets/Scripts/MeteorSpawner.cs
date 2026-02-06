using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float spawnY = 6f;

    [Header("Meteor Properties")]
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float maxSize = 1.5f;
    [SerializeField] private float maxAngle = 30f; // Max angle deviation from straight down

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnMeteor();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnMeteor()
    {
        // Random spawn position at top of screen
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        // Create meteor
        GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        // Random size
        float size = Random.Range(minSize, maxSize);
        meteor.transform.localScale = Vector3.one * size;

        // Random speed
        float speed = Random.Range(minSpeed, maxSpeed);

        // Random direction (mostly downward with some angle variation)
        float angle = Random.Range(-maxAngle, maxAngle);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.down;
        direction.Normalize();

        // Calculate score based on size (smaller = more points)
        int scoreValue = Mathf.RoundToInt(100 / size);

        // Initialize meteor
        Meteor meteorScript = meteor.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            meteorScript.Initialize(speed, direction, scoreValue);
        }
    }

    // Optional: Adjust difficulty over time
    public void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.1f);
        maxSpeed += 0.5f;
    }
}