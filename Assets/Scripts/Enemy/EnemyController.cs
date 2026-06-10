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

    public float attackCooldown = 1f;
    private float lastAttackTime;
    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        ResetStats();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

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

    void OnCollisionStay2D(Collision2D collision)
    {
        DealDamage(collision);
    }

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