using UnityEngine;

public class FinishRaceTrigger : MonoBehaviour
{
    public RaceManager raceManager; 
    //5-6
    //9-2
    //13-1
    //18-1

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            raceManager.FinishRace();
        }
    }
}