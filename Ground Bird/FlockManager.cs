using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {

    public static FlockManager FM;
    public GameObject birdPrefab;
    public int numBird = 20;
    public GameObject[] allBird;
    public Vector3 airLimits = new Vector3(5.0f, 5.0f, 5.0f);
    public Vector3 goalPos;

    [Header("Bird Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;

    //public Vector3[] goalPositions;

    void Awake()
    {
        // If FM is null, initialize it to this instance
        if (FM == null)
        {
            FM = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of FlockManager found!");
        }
    }


    void Start() {

        allBird = new GameObject[numBird];
        
        for (int i = 0; i < numBird; ++i) {

            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-airLimits.x, airLimits.x),
                Random.Range(-airLimits.y, airLimits.y),
                Random.Range(-airLimits.z, airLimits.z));

            allBird[i] = Instantiate(birdPrefab, pos, Quaternion.identity);
        }//if to check if ground bird

        FM = this;
        goalPos = this.transform.position;
    }


    void Update() {

        if (Random.Range(0, 100) < 10) {

            goalPos = this.transform.position + new Vector3(
                Random.Range(-airLimits.x, airLimits.x),
                Random.Range(-airLimits.y, airLimits.y),
                Random.Range(-airLimits.z, airLimits.z));
        }//if check ground bird
    }
}