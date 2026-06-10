using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SessionTimer : MonoBehaviour
{
    public float sessionDuration = 60f;
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
        if (!sessionActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainScene");
            }
            return;
        }

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timer);

        if (timer <= 0)
        {
            sessionActive = false;
            timerText.text = "Time: 0";
            dataLogger.LogSession();
            dataLogger.SaveLog();
            Time.timeScale = 0f;

            if (sessionCompletePanel != null)
                sessionCompletePanel.SetActive(true);
        }
    }
}