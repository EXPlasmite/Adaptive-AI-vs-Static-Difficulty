using UnityEngine;
using TMPro;

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
        if (sessionCompletePanel != null)
            sessionCompletePanel.SetActive(false);
    }

    void Update()
    {
        if (!sessionActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sessionCompletePanel.SetActive(false);
                RestartTimer();
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

            if (sessionCompletePanel != null)
                sessionCompletePanel.SetActive(true);

            Debug.Log("Session Complete!");
        }
    }

    public void RestartTimer()
    {
        timer = sessionDuration;
        sessionActive = true;
    }
}