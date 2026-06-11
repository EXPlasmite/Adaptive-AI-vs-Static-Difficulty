using UnityEngine;
using UnityEngine.UI;

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
            float maxHealth = enemyController.baseHealth * 
                (DifficultyManager.Instance != null ? 
                DifficultyManager.Instance.GetMultiplier() : 1f);
            healthBar.value = enemyController.health / maxHealth;
        }
    }
}