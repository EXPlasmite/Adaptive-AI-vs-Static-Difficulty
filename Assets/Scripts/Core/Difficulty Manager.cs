using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public PlayerPerformanceTracker player;
    public EnemyController enemy;

    [Header("Settings")]
    public float checkInterval = 10f;
    private float timer;

    private float difficultyMultiplier = 1f;

    public enum DifficultyMode
    {
        Static,
        Adaptive
    }

    public DifficultyMode mode;

    void Start()
    {
        if (mode == DifficultyMode.Static)
        {
            ApplyStaticDifficulty(1f, 1f);
        }
    }

    void Update()
    {
        if (mode == DifficultyMode.Static)
            return;

        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            EvaluatePlayer();
            timer = 0f;
        }
    }

    void ApplyStaticDifficulty(float healthMultiplier, float damageMultiplier)
    {
        enemy.health = enemy.baseHealth * healthMultiplier;
        enemy.damage = enemy.baseDamage * damageMultiplier;
    }

    void EvaluatePlayer()
    {
        float score = player.damageTaken + (player.deaths * 50f);

        if (score > 100f)
        {
            DecreaseDifficulty();
        }
        else if (score < 40f)
        {
            IncreaseDifficulty();
        }

        player.ResetStats();
    }

    void DecreaseDifficulty()
    {
        difficultyMultiplier = Mathf.Max(0.5f, difficultyMultiplier - 0.1f);
        ApplyAdaptiveDifficulty();
    }

    void IncreaseDifficulty()
    {
        difficultyMultiplier = Mathf.Min(2f, difficultyMultiplier + 0.1f);
        ApplyAdaptiveDifficulty();
    }

    void ApplyAdaptiveDifficulty()
    {
        enemy.health = enemy.baseHealth * difficultyMultiplier;
        enemy.damage = enemy.baseDamage * difficultyMultiplier;
    }
}