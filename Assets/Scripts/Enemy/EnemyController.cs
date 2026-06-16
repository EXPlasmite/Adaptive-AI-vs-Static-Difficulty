using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy health, damage output and collision-based attacks.
// Stats are scaled by the difficulty multiplier at spawn via ResetStats().
public class EnemyController : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseHealth = 100f;
    public float baseDamage = 10f;

    [Header("Runtime Stats")]
    // These are set at spawn and updated by DifficultyManager.ApplyToAllEnemies()
    public float health;
    public float damage;

    public float attackCooldown = 1f;
    private float lastAttackTime;
    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        // Scale health and damage by current multiplier on spawn
        ResetStats();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Applies the current difficulty multiplier to health and damage.
    // Called on spawn - note that health is not retroactively updated
    // when the multiplier changes mid-session, only newly spawned
    // enemies receive the updated health value.
    public void ResetStats()
    {
        float multiplier = DifficultyManager.Instance != null ?
            DifficultyManager.Instance.GetMultiplier() : 1f;
        health = baseHealth * multiplier;
        damage = baseDamage * multiplier;
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
        DealDamage(collision);
    }

    // OnCollisionStay ensures damage continues to be dealt
    // while the enemy remains in contact with the player
    void OnCollisionStay2D(Collision2D collision)
    {
        DealDamage(collision);
    }

    // Deals damage to the player on contact, subject to attack cooldown
    void DealDamage(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerPerformanceTracker tracker =
                    collision.gameObject.GetComponent<PlayerPerformanceTracker>();

                if (tracker != null)
                {
                    tracker.TakeDamage(damage);
                    if (audioSource != null) audioSource.Play();
                    if (animator != null) animator.Play("attack");
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}