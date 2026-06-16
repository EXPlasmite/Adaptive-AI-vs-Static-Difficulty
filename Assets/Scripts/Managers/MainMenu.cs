using UnityEngine;
using UnityEngine.SceneManagement;

// Handles main menu button interactions for mode selection.
// Stores the selected mode in PlayerPrefs so DifficultyManager
// can read it on scene load and configure itself accordingly.
public class MainMenu : MonoBehaviour
{
    // Sets mode to Static and loads the game scene
    public void PlayStatic()
    {
        PlayerPrefs.SetString("GameMode", "Static");
        SceneManager.LoadScene("MainScene");
    }

    // Sets mode to Adaptive and loads the game scene
    public void PlayAdaptive()
    {
        PlayerPrefs.SetString("GameMode", "Adaptive");
        SceneManager.LoadScene("MainScene");
    }
}