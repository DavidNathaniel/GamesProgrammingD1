using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    public RaceManager raceManager; 
    public ParticleSystem ps; 

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
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            // Notify the RaceManager that this checkpoint has been activated
            raceManager.ActivateCheckpoint();

            // Stop the particle system
            if (ps != null)
            {
                ps.Stop();
            }

            // Disable the checkpoint GameObject (or just the collider and particle system)
            gameObject.SetActive(false); // Disables the entire GameObject
            // Alternatively, you can disable just the collider and particle system:
            // GetComponent<Collider>().enabled = false;
            // particleSystem.gameObject.SetActive(false);

            Debug.Log("Checkpoint activated and particle system stopped.");
        }
    }
}