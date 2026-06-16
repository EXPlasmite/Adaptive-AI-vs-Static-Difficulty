using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Logs gameplay data to a CSV file at the end of each session.
// Records one row per condition (Static or Adaptive) per participant.
// CSV is saved to Application.persistentDataPath/session_log.csv
public class DataLogger : MonoBehaviour
{
    public PlayerPerformanceTracker player;
    public DifficultyManager difficultyManager;

    private List<string> log = new List<string>();
    private float sessionTimer;
    private int sessionNumber = 1;

    void Start()
    {
        // Write CSV header row on scene load
        log.Add("Session,Mode,Deaths,DamageTaken,TimeSeconds,FinalMultiplier");
    }

    void Update()
    {
        // Track session duration continuously
        sessionTimer += Time.deltaTime;
    }

    // Called when the session ends (via UI button).
    // Logs totalDeaths and totalDamageTaken - these persist across
    // DifficultyManager's per-interval ResetStats() calls, ensuring
    // the full session totals are recorded rather than only the
    // final interval's values.
    public void LogSession()
    {
        string mode = difficultyManager.mode.ToString();
        string entry = $"{sessionNumber},{mode},{player.totalDeaths}," +
                    $"{player.totalDamageTaken},{sessionTimer:F1}," +
                    $"{difficultyManager.GetMultiplier()}";
        log.Add(entry);
        player.ResetStats();
        sessionNumber++;
        sessionTimer = 0f;
    }

    // Logs the session, saves to CSV and returns to the main menu
    public void EndSession()
    {
        LogSession();
        SaveLog();
        SceneManager.LoadScene("MainMenu");
    }

    // Appends to existing CSV if one exists, otherwise creates a new file.
    // Skips the header row when appending to avoid duplicates.
    public void SaveLog()
    {
        string path = Application.persistentDataPath + "/session_log.csv";
        if (!File.Exists(path))
            File.WriteAllLines(path, log);
        else
            File.AppendAllLines(path, log.GetRange(1, log.Count - 1));
    }
}