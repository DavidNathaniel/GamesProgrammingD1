using UnityEngine;

/// <summary>
/// This class is responsible for recording the ghost data of the player during a run.
/// 
/// It captures the player's transform data at regular intervals and stores it in the GhostData structure.
/// This object is attached to the player object, and can report the ghost data to the GhostManager.
/// </summary>
public class GhostRecorder : MonoBehaviour {
    [Header("Recording Settings")]
    public float recordInterval = 0.1f; // Interval at which to record the player's transform data

    private bool isRecording = false; // Flag to indicate if the recorder is currently recording
    private float timer = 0f; // Timer to keep track of the recording interval
    private GhostData currentGhostData = new GhostData(); // Ghost data structure to store the run positions

    private Transform playerTransform; // Reference to the player's transform component

    private void Awake() {
        // Get the player's transform component
        playerTransform = GetComponent<Transform>();
    }

    public void StartRecording() {
        // Check if the recorder is already recording
        if (isRecording) {
            Debug.LogWarning("Recorder is already recording!");
            return;
        }

        isRecording = true; // Set the recording flag to true
        timer = 0f; // Reset the timer
        currentGhostData = new GhostData(); // Create a fresh ghost data structure
        currentGhostData.startTime = Time.time; // Set the start time of the ghost run
        RecordTransformData(); // Record the initial position/transform data
        Debug.Log("Recording started!");
    }

    public void StopRecording() {
        // Check if the recorder is not currently recording
        if (!isRecording) {
            Debug.LogWarning("Recorder is already stopped!");
            return;
        }

        isRecording = false; // Set the recording flag to false
        currentGhostData.endTime = Time.time; // Set the end time of the ghost run
        // TODO: Send the ghost data to the GhostManager
        Debug.Log("Recording stopped!");
    }

    private void RecordTransformData() {
        // Store the player's transform data in the ghost data structure
        currentGhostData.transformData.Add(
            // Capture the player's transform data in a TransformData instance
            new TransformData(
                playerTransform.position, 
                playerTransform.rotation, 
                Time.time
            )
        );
    }

    private void Update() {
        // Check if the recorder is currently recording
        if (isRecording) {
            // Increment timer by the delta time
            timer += Time.deltaTime;
            // Check if the timer has exceeded the recording interval
            if (timer >= recordInterval) {
                // Record the player's transform data
                RecordTransformData();
                // Reset the timer
                timer = 0f;
            }
        }
        // Else, do fuck all because we're not recording    
    }
}