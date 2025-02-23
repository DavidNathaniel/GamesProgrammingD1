using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManagerAir : MonoBehaviour {

    public static FlockManagerAir FM;
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

    public GameObject[] goalLocations;

    void Start() {

        allBird = new GameObject[numBird];
        float yOffset = 25f; 
        
        for (int i = 0; i < numBird; ++i) {

            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-airLimits.x, airLimits.x),
                0 + yOffset,
                Random.Range(-airLimits.z, airLimits.z));

            allBird[i] = Instantiate(birdPrefab, pos, Quaternion.identity);
        }//if to check if ground bird

        FM = this;
        goalPos = this.transform.position;
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        if(goalLocations != null && goalLocations.Length > 0){
            int i = Random.Range(0, goalLocations.Length);
            goalPos = goalLocations[i].transform.position;
        }
        
    }


    void Update() {
        if (Random.Range(0, 100) < 10) {
            goalPos = this.transform.position + new Vector3(
                Random.Range(-airLimits.x, airLimits.x),
                Random.Range(-airLimits.y, airLimits.y) + 25f,
                Random.Range(-airLimits.z, airLimits.z));    
        }
    }
}