using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControlAir : MonoBehaviour {

    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detectionRadius = 20.0f;
    float fleeRadius = 10.0f;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        //goalLocations = GameObject.FindGameObjectsWithTag("goal");
        //Debug.Log("Hello " + goalLocations);
        if(goalLocations == null || goalLocations.Length == 0){
            
        }
        else{
            int i = Random.Range(0, goalLocations.Length);
            agent.SetDestination(goalLocations[i].transform.position);
        }
        
       // anim = this.GetComponent<Animator>();
        //anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
        ResetAgent();
    }

    void ResetAgent() {

        speedMult = Random.Range(1.0f, 2.0f);
        //anim.SetFloat("speedMult", speedMult);
        agent.speed *= speedMult;
        //anim.SetTrigger("isWalking");//rename
        agent.angularSpeed = 120.0f;
        agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position) {

        if (Vector3.Distance(position, this.transform.position) < detectionRadius) {

            Vector3 fleeDirection = (this.transform.position - position).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid) {

                agent.SetDestination(path.corners[path.corners.Length - 1]);
                //anim.SetTrigger("isRunning");//rename
                agent.speed = 10.0f;
                agent.angularSpeed = 500.0f;
            }
        }
    }

    void Update() {
        //goalLocations = GameObject.FindGameObjectsWithTag("goal");
        /*if(goalLocations != null && goalLocations.Length > 0){
            if (agent.remainingDistance < 1.0f) {
                ResetAgent();
                int i = Random.Range(0, goalLocations.Length);
                agent.SetDestination(goalLocations[i].transform.position);
            }
        }*/
    }
}