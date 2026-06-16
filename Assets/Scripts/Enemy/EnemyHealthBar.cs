using UnityEngine;
using UnityEngine.UI;

// Displays a health bar for each enemy, scaled relative to their
// current maximum health (which varies with the difficulty multiplier).
// This means the health bar reflects scaled health values, making
// difficulty adjustments partially visible to the player.
public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        if (enemyController != null)
        {
            // Recalculate max health each frame to account for multiplier changes
            float maxHealth = enemyController.baseHealth *
                (DifficultyManager.Instance != null ?
                DifficultyManager.Instance.GetMultiplier() : 1f);

            // Set bar fill as a proportion of current health to scaled maximum
            healthBar.value = enemyController.health / maxHealth;
        }
    }
}