using UnityEngine;

// Tracks player health and performance metrics throughout each session.
// Maintains two sets of counters: interval-level stats (damageTaken, deaths)
// which are reset by DifficultyManager after each evaluation window and
// session-level totals (totalDamageTaken, totalDeaths) which persist for
// the full session and are used by DataLogger for CSV output.
public class PlayerPerformanceTracker : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Performance Metrics")]
    // Reset every interval by DifficultyManager - used for difficulty evaluation
    public float damageTaken;
    public int deaths;

    // Never reset mid-session - used by DataLogger to record session totals
    public float totalDamageTaken;
    public int totalDeaths;

    [Header("Respawn")]
    public Transform[] respawnPoints;
    public float safeSpawnRadius = 5f; // Radius checked for nearby enemies on respawn

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        damageTaken += damage;         // Interval counter - reset each evaluation
        totalDamageTaken += damage;    // Session total - never reset

        // Death registered when health reaches zero - player respawns immediately
        // rather than ending the session, allowing play to continue for 150 seconds
        if (currentHealth <= 0f)
        {
            RegisterDeath();
            currentHealth = maxHealth;
        }
    }

    public void RegisterDeath()
    {
        deaths++;       // Interval counter - reset each evaluation
        totalDeaths++;  // Session total - never reset
        transform.position = GetSafeSpawnPosition();
    }

    // Finds a respawn point with no enemies within safeSpawnRadius.
    // Falls back to the first respawn point if none are clear.
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

    // Resets interval-level counters only.
    // Called by DifficultyManager after each evaluation and by DataLogger after logging.
    // totalDamageTaken and totalDeaths are intentionally NOT reset here.
    public void ResetStats()
    {
        damageTaken = 0f;
        deaths = 0;
    }
}