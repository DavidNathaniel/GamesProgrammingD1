using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathControl : MonoBehaviour {

    public GameObject[] goalLocations;
    NavMeshAgent agent;
    

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("walk goal");
        int i = Random.Range(0, goalLocations.Length);
        agent.SetDestination(goalLocations[i].transform.position);
        float sm = Random.Range(0.5f, 2.0f);
        agent.speed *= sm;

    }


    void Update() {

        if (agent.remainingDistance < 1.0f) {

            int i = Random.Range(0, goalLocations.Length);
            agent.SetDestination(goalLocations[i].transform.position);
        }
    }
}