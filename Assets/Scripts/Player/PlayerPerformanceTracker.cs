using UnityEngine;

public class PlayerPerformanceTracker : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Performance Metrics")]
    public float damageTaken;
    public int deaths;
    public int totalDeaths;

    [Header("Respawn")]
    public Transform[] respawnPoints;
    public float safeSpawnRadius = 5f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        damageTaken += damage;

        if (currentHealth <= 0f)
        {
            RegisterDeath();
            currentHealth = maxHealth;
        }
    }

    public void RegisterDeath()
    {
        deaths++;
        totalDeaths++;
        transform.position = GetSafeSpawnPosition();
    }

    Vector3 GetSafeSpawnPosition()
    {
        if (respawnPoints == null || respawnPoints.Length == 0)
            return Vector3.zero;

        int enemyLayer = LayerMask.GetMask("Enemy");

        foreach (Transform point in respawnPoints)
        {
            if (Physics2D.OverlapCircleAll(point.position, safeSpawnRadius, enemyLayer).Length == 0)
                return point.position;
        }

        return respawnPoints[0].position;
    }

    public void ResetStats()
    {
        damageTaken = 0f;
        deaths = 0;
    }
}