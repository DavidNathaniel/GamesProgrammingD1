using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check for Esc or M key press
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M))
        {
            LoadMainMenu();
        }
    }

    // Method to load the main menu scene
    void LoadMainMenu()
    {
        Debug.Log("Esc / M detected. Loading Scene: " + "TitleScene");
        // Unlock and show the cursor when the TitleScreen loads
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single); 
    }
}