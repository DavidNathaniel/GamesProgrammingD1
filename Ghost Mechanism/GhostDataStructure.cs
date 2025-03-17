using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// This file defines the data structures used to store the data of the ghost mechanism.
/// </summary>


/// <summary>
/// Class defining an instance of data used to capture the transform of the player at a given time.
/// </summary>
[Serializable]
public class TransformData {
    public Vector3 position; // Current position of the player
    public Quaternion rotation; // Current rotation of the player
    public float time; // Time at which the data was captured

    public TransformData(Vector3 position, Quaternion rotation, float time) {
        this.position = position;
        this.rotation = rotation;
        this.time = time;
    }
}

/// <summary>
/// Class defining the data structure used to store a ghost run.
/// </summary>
[Serializable]
public class GhostData {
    public List<TransformData> transformData; // List of transform data instances representing an entire ghost run
    public float startTime; // Start time of the ghost run
    public float endTime; // End time of the ghost run
    public float TotalTime { get { return endTime - startTime; } } // Total time taken for the ghost run
    public string levelName; // Name of the level where the ghost run was recorded
    public string playerName; // Name of the player who recorded the ghost run
}