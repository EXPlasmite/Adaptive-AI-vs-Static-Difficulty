using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        string savedMode = PlayerPrefs.GetString("GameMode", "Static");
        mode = savedMode == "Adaptive" ? 
            DifficultyMode.Adaptive : DifficultyMode.Static;
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
            // Don't reset current health, only update damage and speed
            enemy.damage = enemy.baseDamage * difficultyMultiplier;

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null) ai.speed = ai.baseSpeed * difficultyMultiplier;
        }
    }
}