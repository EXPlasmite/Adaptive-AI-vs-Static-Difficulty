using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public PlayerPerformanceTracker player;

    [Header("Settings")]
    public float checkInterval = 10f;
    private float timer;

    private float difficultyMultiplier = 1f;

    public float GetMultiplier()
    {
        return difficultyMultiplier;
    }

    public enum DifficultyMode
    {
        Static,
        Adaptive
    }

    public DifficultyMode mode;

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
        ApplyToAllEnemies();
    }

    void IncreaseDifficulty()
    {
        difficultyMultiplier = Mathf.Min(2f, difficultyMultiplier + 0.1f);
        ApplyToAllEnemies();
    }

    void ApplyToAllEnemies()
    {
        EnemyController[] enemies = 
            FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        
        foreach (EnemyController enemy in enemies)
        {
            enemy.health = enemy.baseHealth * difficultyMultiplier;
            enemy.damage = enemy.baseDamage * difficultyMultiplier;

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null) ai.speed = ai.baseSpeed * difficultyMultiplier;
        }
    }
}