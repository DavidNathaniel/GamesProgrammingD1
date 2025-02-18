using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    // UI elements
    [Header("Text Elements")]
    public TMP_Text timerText;

    // Race state
    [Header("Race State")]
    private bool raceStarted = false;
    private bool raceFinished = false;
    private float raceTime = 0f;
    public bool isTutorial = false;
    public float finishTime;

    // Array to store all checkpoint GameObjects
    [Header("Race Points")]
    public GameObject startLine;
    public GameObject[] checkpoints;
    public GameObject finishLine;

    // Checkpoints
    private int totalCheckpoints = 3; // hardcoded number of checkpoints
    private int checkpointsActivated = 0;


    void Start()
    {
        totalCheckpoints = checkpoints.Length; // determine the size of the actual checkpoints in the array
        Debug.Log("total number of checkpoints: "+totalCheckpoints);
        raceStarted = false;
        raceFinished = false;
        finishLine.SetActive(false);
        StopCheckpointParticleSystems();
    }

    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            // Update the race timer
            raceTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + raceTime.ToString("F3"); // Display time with 2 decimal places
        }
    }

    public void StartRace()
    {
        if (!raceStarted)
        {
            raceStarted = true;
            raceFinished = false;
            raceTime = 0f;
            checkpointsActivated = 0;
            
            //activate the finish line and each checkpoints particle systems
            StartCheckpointParticleSystems();// this should be in its own method.
            Debug.Log("Race started!");
        }
    }

    private void StopCheckpointParticleSystems()
    {
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(false); // Kill the checkpoint
            // Get the ParticleSystem component of the checkpoint
            ParticleSystem ps = checkpoint.GetComponent<ParticleSystem>();
            if (ps != null && !ps.isPlaying)
            {
                ps.Stop(); // Stop the particle system
            }
        }
    }

    private void StartCheckpointParticleSystems()
    {
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(true); // Reactivate the checkpoint
            // Get the ParticleSystem component of the checkpoint
            ParticleSystem ps = checkpoint.GetComponent<ParticleSystem>();
            if (ps != null && !ps.isPlaying)
            {
                ps.Play(); // Start the particle system
            }
        }
        ParticleSystem finishps = finishLine.GetComponent<ParticleSystem>();
        finishLine.SetActive(true);
        if (finishps != null && !finishps.isPlaying)
        {
            finishps.Play(); // Start the particle system
        }

        Debug.Log("Animations started!");
    }

    public void ActivateCheckpoint()
    {
        checkpointsActivated++;
        Debug.Log("Checkpoint activated! Total: " + checkpointsActivated);

        if (checkpointsActivated >= totalCheckpoints)
        {
            Debug.Log("All checkpoints activated! Proceed to finish line.");
        }
    }

    public void FinishRace()
    {
        if (raceStarted && checkpointsActivated >= totalCheckpoints)
        {
            if (raceTime <= finishTime)
            {
                raceFinished = true;
                Debug.Log("Race finished! Time: " + raceTime.ToString("F3"));

                //something in here about the game state needing to update - the colour of the tutorial goes green once you have completed that stage.
                //need to check if we are in the tutorial or not, or rather need to determine which scene we are in and dependant on that will depend on what happens next.
                if (isTutorial)
                {
                    // Save that the tutorial is completed
                    PlayerPrefs.SetInt("TutorialCompleted", 1);
                    PlayerPrefs.Save(); // Save the data is immediately
                    Debug.Log("Tutorial completed! Level 1 unlocked.");
                }

                ParticleSystem finishps = finishLine.GetComponent<ParticleSystem>();
                finishps.Stop(); // may have to do some wiggling here for repeat use of the race.
                                 //finishLine.SetActive(false); // kills it too fast
            }
            else
            {
                Debug.Log("Time was not fast enough! Race Reset...");
                ResetRace();
            }
        }
        else
        {
            Debug.Log("Cannot finish race. Not all checkpoints have been activated.");
        }
    }

    public void ResetRace()
    {
        // Reset race state
        raceStarted = false;
        raceFinished = false;
        raceTime = 0f;
        checkpointsActivated = 0;

        UpdateTimerDisplay(); //to be 0f

        // Reset all checkpoints
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(false); // Reactivate the checkpoint
            ParticleSystem ps = checkpoint.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop(); // Stop the particle system
            }
        }
        //reset startline 
        startLine.SetActive(true);
        ParticleSystem startps = startLine.GetComponent<ParticleSystem>();
        if (startps != null)
        {
            startps.Play(); // Restart the particle system
        }

        //reset finishline
        finishLine.SetActive(false);
        ParticleSystem finishps = finishLine.GetComponent<ParticleSystem>();
        if (finishps != null)
        {
            Debug.Log("play the finishline anim");
            //finishps.Stop();
            finishps.Stop(); // stop the particle system
        }
        Debug.Log("Race reset!");
    }
}