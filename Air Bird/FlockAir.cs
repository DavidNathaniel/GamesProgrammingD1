using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAir : MonoBehaviour {

    public new float speed;
    bool turning = false;

    void Start() {

        speed = Random.Range(FlockManagerAir.FM.minSpeed, FlockManagerAir.FM.maxSpeed);
        
    }


    void Update() {
        

        Bounds b = new Bounds(FlockManagerAir.FM.transform.position, FlockManagerAir.FM.airLimits * 2.0f);
        if (!b.Contains(transform.position)) {

            turning = true;
        } else {

            turning = false;
        }

        if (turning) {

            Vector3 direction = FlockManagerAir.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                FlockManagerAir.FM.rotationSpeed * Time.deltaTime);
        } else {


            if (Random.Range(0, 100) < 10) {

                speed = Random.Range(FlockManagerAir.FM.minSpeed, FlockManagerAir.FM.maxSpeed);
            }


            if (Random.Range(0, 100) < 10) {
                ApplyRules();
            }
        }

        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
    }

    private void ApplyRules() {

        GameObject[] gos;
        gos = FlockManagerAir.FM.allBird;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;

        float gSpeed = 0.01f;
        float mDistance;
        int groupSize = 0;

        foreach (GameObject go in gos) {

            if (go != this.gameObject) {

                mDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (mDistance <= FlockManagerAir.FM.neighbourDistance) {

                    vCentre += go.transform.position;
                    groupSize++;

                    if (mDistance < 1.0f) {

                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    FlockAir anotherFlock = go.GetComponent<FlockAir>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0) {

            vCentre = vCentre / groupSize + (FlockManagerAir.FM.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            if (speed > FlockManagerAir.FM.maxSpeed) {

                speed = FlockManagerAir.FM.maxSpeed;
            }

            Vector3 direction = (vCentre + vAvoid) - transform.position;
            if (direction != Vector3.zero) {

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    FlockManagerAir.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }
}