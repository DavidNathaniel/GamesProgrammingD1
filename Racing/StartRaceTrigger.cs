using UnityEngine;

public class StartRaceTrigger : MonoBehaviour
{
    public RaceManager raceManager;
    public ParticleSystem activationEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            raceManager.StartRace();

            // Stop the particle system after the player has started the race.
            if (activationEffect != null)
            {
                activationEffect.gameObject.SetActive(false);// activationEffect.Play();
                
            }
        }
    }
}