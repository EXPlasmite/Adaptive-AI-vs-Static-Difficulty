using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseHealth = 100f;
    public float baseDamage = 10f;

    [Header("Runtime Stats")]
    public float health;
    public float damage;

    void Start()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        health = baseHealth;
        damage = baseDamage;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPerformanceTracker tracker =
                collision.gameObject.GetComponent<PlayerPerformanceTracker>();

            if (tracker != null)
            {
                tracker.TakeDamage(damage);
            }
        }
    }
}