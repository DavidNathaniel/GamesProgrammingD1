using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class RaceManager : MonoBehaviour
{
    // UI 
    [Header("Text Elements")]
    public TMP_Text timerText;

    // Race state
    [Header("Race State")]
    private bool raceStarted = false;
    private bool raceFinished = false;
    private float raceTime = 0f;
    public bool isTutorial = false;
    public float finishTime;

    //array to store all checkpoint GameObjects
    [Header("Race Points")]
    public GameObject startLine;
    public GameObject finishLine;
    public GameObject[] checkpoints;

    [Header("Audio Sources")]
    public AudioSource startLineAudio;
    public AudioSource finishLineAudio;

    // Checkpoints
    private int totalCheckpoints;
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

            //trigger sound effect
            if (startLineAudio != null && !startLineAudio.isPlaying)
            {
                startLineAudio.Play();
                StartCoroutine(DisableObjectAfterSound(startLineAudio.clip.length, startLine)); // disable gameobject AFTER sound has finished playing
            }

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
            finishps.Play(); //start the particle system
        }

        Debug.Log("Animations started!");
    }

    public void ActivateCheckpoint(GameObject checkpoint, AudioSource checkpointAudio)
    {
        checkpointsActivated++;
        Debug.Log("Checkpoint activated! Total: " + checkpointsActivated);

        if (checkpointsActivated >= totalCheckpoints)
        {
            Debug.Log("All checkpoints activated! Proceed to finish line.");
        }

        //trigger sound effect
        if (checkpointAudio != null && !checkpointAudio.isPlaying)
        {
            checkpointAudio.Play();
            StartCoroutine(DisableObjectAfterSound(checkpointAudio.clip.length, checkpoint)); // disable gameobject AFTER sound has finished playing
        }
    }

    public void FinishRace()
    {
        if (raceStarted && checkpointsActivated >= totalCheckpoints)
        {
            //trigger sound effect
            if (finishLineAudio != null && !finishLineAudio.isPlaying)
            {
                finishLineAudio.Play();
                StartCoroutine(DisableObjectAfterSound(finishLineAudio.clip.length, finishLine)); // disable gameobject AFTER sound has finished playing
            }

            if (raceTime <= finishTime)
            {
                raceFinished = true;
                Debug.Log("Race finished! Time: " + raceTime.ToString("F3"));

                // If in the tutorial, allow the player to access the next levels
                if (isTutorial)
                {
                    // Save that the tutorial is completed
                    PlayerPrefs.SetInt("TutorialCompleted", 1);
                    PlayerPrefs.Save(); // Save the data is immediately
                    Debug.Log("Tutorial completed! Level 1 unlocked.");
                }

                //stop the particle effects
                ParticleSystem finishps = finishLine.GetComponent<ParticleSystem>();
                finishps.Stop(); 
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

        //reset all checkpoints
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
        ParticleSystem finishps = finishLine.GetComponent<ParticleSystem>();
        if (finishps != null)
        {
            Debug.Log("play the finishline anim");
            finishps.Stop(); // stop the particle system
        }
        Debug.Log("Race reset!");
    }
    private IEnumerator DisableObjectAfterSound(float delay, GameObject gameObject)
    {
        //wait for the duration of the sound clip
        yield return new WaitForSeconds(delay);

        //Then disable the GameObject...
        gameObject.SetActive(false);
    }
}