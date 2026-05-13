using System.Collections;
using System.Collections.Generic;
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

        Debug.Log("Damage Taken: " + damageTaken + " | Health: " + currentHealth);

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
        Debug.Log("Deaths: " + deaths);
    }

    public void ResetStats()
    {
        Debug.Log("Stats Reset");
        damageTaken = 0f;
        deaths = 0;
        currentHealth = maxHealth;
    }
}