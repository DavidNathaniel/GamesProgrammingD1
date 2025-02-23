using UnityEngine;


public class Fleeing : MonoBehaviour
{
    public GameObject player;
    GameObject[] agents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");//maybe change
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
        
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find player by tag
        if (player != null)
        {
            // Pass the player's position into DetectNewObstacle
            foreach (GameObject a in agents)
            {
                if (a != null)
                {
                    AIControl aiControl = a.GetComponent<AIControl>();
                    if (aiControl != null)
                    {
                        aiControl.DetectNewObstacle(player.transform.position);
                    }
                    else
                    {
                        //Debug.LogWarning("AIControl component is missing on " + a.name);
                    }
                }
                else
                {
                    //Debug.LogWarning("Agent in the array is null.");
                }
            }
        }
        else
        {
            //Debug.LogWarning("Player not found!");
        }

    }
}
