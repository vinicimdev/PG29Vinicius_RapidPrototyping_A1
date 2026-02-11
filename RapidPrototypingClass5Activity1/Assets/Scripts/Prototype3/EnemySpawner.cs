using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnRangeX = 6f;
    [SerializeField] private float spawnY = 6f;

    [Header("Enemy Properties")]
    [SerializeField] private int baseScoreValue = 100;

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    void SpawnEnemy()
    {
        if (enemyPrefab == null)
            return;

        // Random spawn position at top of screen
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        // Create enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Set score value
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.scoreValue = baseScoreValue;
        }
    }

    // Optional: Adjust difficulty over time
    public void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(1f, spawnInterval - 0.2f);
    }
}
