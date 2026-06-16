using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Manages in-game UI display and session control buttons.
// Displays live player stats and the current difficulty multiplier,
// making the adaptive system's adjustments visible to the player
// during the session.
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
    public TextMeshProUGUI multiplierText; // Displays current multiplier value live

    // Update UI every frame with current player stats and difficulty state
    void Update()
    {
        healthText.text = "Health: " + player.currentHealth.ToString("F0");
        deathText.text = "Deaths: " + player.totalDeaths;
        modeText.text = "Mode: " + difficultyManager.mode.ToString();
        multiplierText.text = "Multiplier: " +
            difficultyManager.GetMultiplier().ToString("F2");
    }

    // Toggles between Static and Adaptive mode during a session.
    // Used for testing purposes during development.
    public void SwitchMode()
    {
        if (difficultyManager.mode == DifficultyManager.DifficultyMode.Static)
            difficultyManager.mode = DifficultyManager.DifficultyMode.Adaptive;
        else
            difficultyManager.mode = DifficultyManager.DifficultyMode.Static;
    }

    // Logs session data and returns to main menu -
    // called manually via a UI button at the end of a condition
    public void EndSession()
    {
        dataLogger.LogSession();
        dataLogger.SaveLog();
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}