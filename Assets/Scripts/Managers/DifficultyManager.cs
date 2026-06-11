using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public PlayerPerformanceTracker player;

    [Header("Settings")]
    public float checkInterval = 5f;
    private float timer;
    private bool gameStarted = false;

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

    public void OnEnemySpawned()
    {
        gameStarted = true;
    }

    void Update()
    {
        if (mode == DifficultyMode.Static || !gameStarted)
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

        if (score > 150f)
            DecreaseDifficulty(0.1f);
        else if (score > 80f)
            DecreaseDifficulty(0.05f);
        else if (score < 30f)
            IncreaseDifficulty(0.1f);
        else if (score < 60f)
            IncreaseDifficulty(0.05f);

        player.ResetStats();
    }

    void DecreaseDifficulty(float amount)
    {
        difficultyMultiplier = Mathf.Max(0.5f, difficultyMultiplier - amount);
        ApplyToAllEnemies();
    }

    void IncreaseDifficulty(float amount)
    {
        difficultyMultiplier = Mathf.Min(2f, difficultyMultiplier + amount);
        ApplyToAllEnemies();
    }

    void ApplyToAllEnemies()
    {
        EnemyController[] enemies = 
            FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        
        foreach (EnemyController enemy in enemies)
        {
            enemy.damage = enemy.baseDamage * difficultyMultiplier;

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null) ai.speed = ai.baseSpeed * difficultyMultiplier;
        }
    }
}