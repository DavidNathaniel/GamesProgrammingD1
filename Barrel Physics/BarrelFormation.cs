using UnityEngine;

public class BarrelFormation : MonoBehaviour
{
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;
    private Transform[] barrels;

    void Start()
    {
        //store initial local positions and rotations
        int childCount = transform.childCount;
        initialPositions = new Vector3[childCount];
        initialRotations = new Quaternion[childCount];
        barrels = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            barrels[i] = transform.GetChild(i);
            initialPositions[i] = barrels[i].localPosition;
            initialRotations[i] = barrels[i].localRotation;
        }
    }

    public void ResetBarrels()
    {
        //reset all barrels to their original local positions
        for (int i = 0; i < barrels.Length; i++)
        {
            barrels[i].localPosition = initialPositions[i];
            barrels[i].localRotation = initialRotations[i];

            //reset each barrels' physics
            Rigidbody rb = barrels[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
