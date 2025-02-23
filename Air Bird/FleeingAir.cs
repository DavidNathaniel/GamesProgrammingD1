using UnityEngine;


public class FleeingAir : MonoBehaviour
{
    public GameObject obstacle;
    GameObject[] agents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");//maybe change
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){//change to even
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)){
                Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                foreach(GameObject a in agents){
                    a.GetComponent<AIControl>().DetectNewObstacle(hitInfo.point);
                }
            }
        }
    }*/
}
