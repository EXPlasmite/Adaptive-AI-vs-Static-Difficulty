using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages dynamic difficulty adjustment for the adaptive condition.
// Evaluates player performance at fixed intervals and adjusts a difficulty
// multiplier applied to enemy damage and movement speed.
public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public PlayerPerformanceTracker player;

    [Header("Settings")]
    // Default 5f, overridden to 10 seconds in the Inspector for this study
    public float checkInterval = 5f;
    private float timer;
    // Prevents evaluation before the first enemy spawns
    private bool gameStarted = false;

    // Clamped between 0.5 (minimum) and 2.0 (maximum), starts at 1.0
    private float difficultyMultiplier = 1f;

    public float GetMultiplier()
    {
        return difficultyMultiplier;
    }

    public enum DifficultyMode
    {
        Static,   // Multiplier stays fixed at 1.0
        Adaptive  // Multiplier adjusted every checkInterval seconds
    }

    public DifficultyMode mode;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Read mode set from main menu via PlayerPrefs, defaults to Static
        string savedMode = PlayerPrefs.GetString("GameMode", "Static");
        mode = savedMode == "Adaptive" ?
            DifficultyMode.Adaptive : DifficultyMode.Static;
    }

    // Called by enemy spawner when first enemy spawns
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

    // Priority-ordered evaluation - only first matching condition applies.
    // Stats reset after each interval so each window is assessed independently.
    void EvaluatePlayer()
    {
        if (player.deaths > 0)
            DecreaseDifficulty(0.2f);       // Any death — strongest signal
        else if (player.damageTaken > 80f)
            DecreaseDifficulty(0.1f);       // High damage, no death
        else if (player.damageTaken == 0f)
            IncreaseDifficulty(0.2f);       // No damage — player performing strongly
        else if (player.damageTaken < 20f)
            IncreaseDifficulty(0.1f);       // Low damage — player performing well
        // 20-80 damage, no deaths = neutral, no change

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

    // Applies multiplier to damage and speed of all active enemies immediately.
    // Enemy health is only scaled at spawn, not retroactively updated.
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