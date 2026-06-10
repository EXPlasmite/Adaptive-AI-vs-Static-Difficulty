using UnityEngine;

public class PlayerPerformanceTracker : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Performance Metrics")]
    public float damageTaken;
    public int deaths;

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
        transform.position = Vector3.zero;
    }

    public void ResetStats()
    {
        damageTaken = 0f;
    }
}