using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayStatic()
    {
        PlayerPrefs.SetString("GameMode", "Static");
        SceneManager.LoadScene("MainScene");
    }

    public void PlayAdaptive()
    {
        PlayerPrefs.SetString("GameMode", "Adaptive");
        SceneManager.LoadScene("MainScene");
    }
}