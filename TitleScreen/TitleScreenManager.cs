//using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Public variables to assign scene names in the Inspector
    public string tutorialSceneName;
    public string advancedLevelSceneName;
    public Button levelbutton;
    public GameObject tutorialText;

    void Start()
    {
        // Unlock and show the cursor when the TitleScreen loads
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Check if the tutorial is completed to enable access to different levels.
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            levelbutton.interactable = true; // Enable the Level1 button
            tutorialText.SetActive(false);
        }
        else
        {
            levelbutton.interactable = false; // Disable the Level1 button
        }
    }


    // Method to load the tutorial scene
    public void LoadTutorial()
    {
        Debug.Log("Loading Tutorial Scene: " + tutorialSceneName);
        SceneManager.LoadScene(tutorialSceneName, LoadSceneMode.Single);
    }

    // Method to load the advanced level scene
    public void LoadAdvancedLevel()
    {
        Debug.Log("Loading Level1 Scene: " + advancedLevelSceneName);
        SceneManager.LoadScene(advancedLevelSceneName, LoadSceneMode.Single);
    }

    // Method to exit game
    public void ExitApplication()
    {
        Debug.Log("Exit called...");
        Application.Quit();
    }
}