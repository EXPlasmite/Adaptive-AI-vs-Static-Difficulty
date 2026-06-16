using UnityEngine;

// Spawns enemies at fixed intervals up to a maximum count.
// Spawn rate and maximum enemy count are identical in both
// static and adaptive conditions - only enemy stats (damage,
// speed, health) are affected by the difficulty multiplier,
// not how many enemies appear or when.
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f; // Time in seconds between spawn attempts
    public int maxEnemies = 5;       // Maximum enemies active in the scene at once

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // Spawn only if interval has elapsed and enemy cap has not been reached
        if (timer >= spawnInterval &&
            GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Select a random spawn point from the available spawn points
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);

        // Notify DifficultyManager that the first enemy has spawned,
        // starting the evaluation timer in the adaptive condition
        if (DifficultyManager.Instance != null)
            DifficultyManager.Instance.OnEnemySpawned();
    }
}