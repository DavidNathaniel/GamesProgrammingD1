//using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public string tutorialSceneName;
    public string advancedLevelSceneName;
    public Button levelbutton;
    public Button toggleProgressButton;
    public GameObject tutorialText;
    public GameObject tutorialProgressionText;
    

    void Start()
    {
        //unlock and show the cursor when the Titlescreen loads
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Update the UI based on the current progress
        UpdateUI();

        // Add a listener to the toggle button
        toggleProgressButton.onClick.AddListener(ToggleProgress);
    }

    private void UpdateUI()
    {
        //check if the tutorial is completed to enable access to different levels.
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            levelbutton.interactable = true; //enable the Level1 button
            tutorialText.SetActive(false);
            tutorialProgressionText.SetActive(false);
        }
        else
        {
            levelbutton.interactable = false; //disable Level1 button
            tutorialText.SetActive(true);
            tutorialProgressionText.SetActive(true);
        }
    }

    //change the player preferences for demo purposes.
    public void ToggleProgress()
    {
        // Toggle the tutorial completion status
        int tutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0);
        if (tutorialCompleted == 1)
        {
            PlayerPrefs.SetInt("TutorialCompleted", 0); // Reset progress
            Debug.Log("Progress reset. Tutorial must be completed again.");
        }
        else
        {
            PlayerPrefs.SetInt("TutorialCompleted", 1); // Set progress to completed
            Debug.Log("Progress set to completed. Level 1 unlocked.");
        }

        // Save the changes
        PlayerPrefs.Save();

        // Update the UI to reflect the new progress state
        UpdateUI();
    }


    //method to load the tutorial scene
    public void LoadTutorial()
    {
        Debug.Log("Loading Tutorial Scene: " + tutorialSceneName);
        SceneManager.LoadScene(tutorialSceneName, LoadSceneMode.Single);
    }

    //method to load the advanced level (level1) scene
    public void LoadAdvancedLevel()
    {
        Debug.Log("Loading Level1 Scene: " + advancedLevelSceneName);
        SceneManager.LoadScene(advancedLevelSceneName, LoadSceneMode.Single);
    }

    //method to exit game
    public void ExitApplication()
    {
        Debug.Log("Exit called...");
        Application.Quit();
    }

    //method to reset player prefs (for demo/debugging)
    public void ResetPlayerPrefs()
    {
        Debug.Log("Exit called...");
        Application.Quit();
    }
}