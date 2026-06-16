using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Manages the session timer for each gameplay condition.
// When the timer reaches zero, the session ends automatically,
// data is logged and the session complete screen is displayed.
// The player can then press Enter to replay
// and begin the next condition.
public class SessionTimer : MonoBehaviour
{
    public float sessionDuration = 60f; // Set to 150 seconds in the Inspector
    private float timer;
    public TextMeshProUGUI timerText;
    public DataLogger dataLogger;
    public GameObject sessionCompletePanel;
    private bool sessionActive = true;

    void Start()
    {
        timer = sessionDuration;
        Time.timeScale = 1f;
        if (sessionCompletePanel != null)
            sessionCompletePanel.SetActive(false);
    }

    void Update()
    {
        // Session complete - wait for player to press Enter before continuing
        if (!sessionActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainScene");
            }
            return;
        }

        // Count down and update timer display
        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timer);

        // Session end - log data, pause game and show completion panel
        if (timer <= 0)
        {
            sessionActive = false;
            timerText.text = "Time: 0";
            dataLogger.LogSession();
            dataLogger.SaveLog();
            Time.timeScale = 0f; // Pause game while completion screen is shown

            if (sessionCompletePanel != null)
                sessionCompletePanel.SetActive(true);
        }
    }
}