using UnityEngine;

public class FinishRaceTrigger : MonoBehaviour
{
    public RaceManager raceManager; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            raceManager.FinishRace();
        }
    }

}