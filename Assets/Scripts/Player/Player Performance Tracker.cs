using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerformanceTracker : MonoBehaviour
{
    [Header("Performance Metrics")]
    public float damageTaken;
    public int deaths;

    public void TakeDamage(float damage)
    {
        damageTaken += damage;

        Debug.Log("Damage Taken: " + damageTaken);
    }

    public void RegisterDeath()
    {
        deaths++;

        Debug.Log("Deaths: " + deaths);
    }

    public void ResetStats()
    {
        Debug.Log("Stats Reset");

        damageTaken = 0f;
        deaths = 0;
    }

    void Update()
    {
        // Temporary testing input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

        // Temporary death test
        if (Input.GetKeyDown(KeyCode.K))
        {
            RegisterDeath();
        }
    }
}