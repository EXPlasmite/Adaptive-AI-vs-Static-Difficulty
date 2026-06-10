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
            healthBar.value = enemyController.health / enemyController.baseHealth;
    }
}