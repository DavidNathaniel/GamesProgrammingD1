using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    public RaceManager raceManager; 
    public ParticleSystem ps;
    public AudioSource audioSource;

    private void Start()
    {
        // Ensure the particle system is playing at the start
        if (ps != null && !ps.isPlaying)
        {
            ps.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the particle system
            if (ps != null)
            {
                ps.Stop();
            }

            //notify the RaceManager that this checkpoint has been activated
            // and send this gameobject and its audio listener for the cancellation effect.
            raceManager.ActivateCheckpoint(gameObject, audioSource);
            Debug.Log("Checkpoint activated and particle system stopped.");
        }
    }
}