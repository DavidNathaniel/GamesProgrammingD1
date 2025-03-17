using UnityEngine;

/// <summary>
/// This class is responsible for displaying the ghost data of the player during a run.
/// 
/// It gets passed the ghost data from the GhostManager and uses it to display the ghost run when the ghost is active, as requested by the GhostManager.
/// </summary>

public class GhostPlayer : MonoBehaviour {
    [Header("Ghost Settings")]
    public GameObject ghostPrefab; // Reference to the ghost prefab to instantiate
    public float ghostOpacity = 0.5f; // Opacity of the ghost object

    private GhostData ghostData; // Reference to the ghost data structure
    private bool isPlaying = false; // Flag to indicate if the ghost is currently playing
    private float playbackTime = 0f; // Timer to keep track of the ghost playback time
    private float timer = 0f; // Timer to keep track of the playback interval
    public float playbackInterval = 0.1f; // Interval at which to update the ghost playback, MUST BE THE SAME AS THE RECORD INTERVAL
    private int currentTransformIndex = 0; // Index of the current transform data in the ghost data structure
    private GameObject ghostObject; // Reference to the instantiated ghost object

    private void Start()
    {
        // Subscribe to the GhostManager events
        // TODO : Implement the GhostManager events

        // GhostManager.OnGhostStart += OnGhostStart;
        // GhostManager.OnGhostStop += OnGhostStop;   
    }

    public void LoadGhostData(GhostData data)
    {
        // Load the ghost data from the GhostManager
        ghostData = data;
    }

    public void StartPlayback() {
        // Check if the ghost is already playing
        if (isPlaying) {
            Debug.LogWarning("Ghost is already playing!");
            return;
        }
        // Check if the ghost data is valid
        if (ghostData == null || ghostData.transformData.Count == 0) {
            Debug.LogWarning("Ghost data is invalid!");
            return;
        }

        isPlaying = true; // Set the playing flag to true
        currentTransformIndex = 0; // Reset the transform index
        playbackTime = 0f; // Reset the playback timer

        // Set the inital postion/transform of the ghost object
        UpdateGhostTransform();

        // Set the ghost object to active
        ghostObject.SetActive(true);
    }

    public void StopPlayback() {
        // Check if the ghost is not currently playing
        if (!isPlaying) {
            Debug.LogWarning("Ghost is already stopped!");
            return;
        }

        isPlaying = false; // Set the playing flag to false

        // Set the ghost object to inactive
        ghostObject.SetActive(false);
    }

    /// <summary>
    /// Method to update the position and rotation of the ghost object based on the current playback index.
    /// </summary>
    private void UpdateGhostTransform() {
        // Check we're at the end of the ghost data
        if (currentTransformIndex >= ghostData.transformData.Count) {
            Debug.Log("End of ghost data!");
            // Stop the playback
            StopPlayback();
            return;
        }

        TransformData current = ghostData.transformData[currentTransformIndex]; // Get the current transform data
        TransformData next = ghostData.transformData[currentTransformIndex + 1]; // Get the next transform data

        // Maybe we should interpolate between the current and next transform data, but for now we'll just set the position and rotation directly
        ghostObject.transform.position = current.position;
        ghostObject.transform.rotation = current.rotation;
    }

    private void Update() {
        // Check if the ghost is currently playing
        if (!isPlaying) {
            // Bye bye
            return;
        }

        // Increment the playback timer
        playbackTime += Time.deltaTime;
        timer += Time.deltaTime;

        // Check if the playback timer has exceeded the playback interval
        if (playbackTime >= ghostData.TotalTime) {
            // Stop the playback
            StopPlayback();
            return;
        }

        // Check if we need to update the ghost transform
        if (playbackTime >= playbackInterval) {
            // Update the ghost transform
            UpdateGhostTransform();
            // Reset the interval timer
            timer = 0f;
            // Increment the transform index
            currentTransformIndex++;
        }
    }
}