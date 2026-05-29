using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    public PlayerPerformanceTracker player;
    public DifficultyManager difficultyManager;

    private List<string> log = new List<string>();
    private float sessionTimer;
    private int sessionNumber = 1;

    void Start()
    {
        log.Add("Session,Mode,Deaths,DamageTaken,TimeSeconds,FinalMultiplier");
    }

    void Update()
    {
        sessionTimer += Time.deltaTime;
    }

    public void LogSession()
    {
        string mode = difficultyManager.mode.ToString();
        string entry = $"{sessionNumber},{mode},{player.deaths}," +
                      $"{player.damageTaken},{sessionTimer:F1}," +
                      $"{difficultyManager.GetMultiplier()}";
        log.Add(entry);
        player.ResetStats();
        sessionNumber++;
        sessionTimer = 0f;
    }

    public void EndSession()
    {
        LogSession();
        SaveLog();
    }

    public void SaveLog()
    {
        string path = Application.persistentDataPath + "/session_log.csv";
        File.WriteAllLines(path, log);
        Debug.Log("Data saved to: " + path);
    }
}