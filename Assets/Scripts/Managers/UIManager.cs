using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public PlayerPerformanceTracker player;
    public DifficultyManager difficultyManager;
    public DataLogger dataLogger;

    [Header("UI Elements")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI multiplierText;

    void Update()
    {
        healthText.text = "Health: " + player.currentHealth.ToString("F0");
        deathText.text = "Deaths: " + player.deaths;
        modeText.text = "Mode: " + difficultyManager.mode.ToString();
        multiplierText.text = "Multiplier: " + 
            difficultyManager.GetMultiplier().ToString("F2");
    }

    public void SwitchMode()
    {
        if (difficultyManager.mode == DifficultyManager.DifficultyMode.Static)
            difficultyManager.mode = DifficultyManager.DifficultyMode.Adaptive;
        else
            difficultyManager.mode = DifficultyManager.DifficultyMode.Static;
    }

    public void EndSession()
    {
        dataLogger.LogSession();
        dataLogger.SaveLog();
    }
}