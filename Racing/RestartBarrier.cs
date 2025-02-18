using UnityEngine;

public class RestartBarrier : MonoBehaviour
{
    public Transform respawnPoint;
    public Transform parentPlayer;
    public RaceManager raceManager;
    public PlayerMovementRB pm;
    /*public Transform player;
    public Transform cameraDirection;
    public Transform cameraPostion;

    public Transform mainCameraObject;
    public Transform mainCameraPosition;*/

    private void OnTriggerEnter(Collider other) // "other" is the object triggering the collider and thus this script
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Reset the player's position to the respawn point
            parentPlayer.position = respawnPoint.position;
            //mainCameraPosition.position = respawnPoint.position;

            // Reset the player's velocity
            Rigidbody playerRigidbody = parentPlayer.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.linearVelocity = Vector3.zero;
                playerRigidbody.angularVelocity = Vector3.zero;
                pm.resetSpeed = true;
                //also set player's movespeed to 0 - resetting their wallrun speed or smth. //pm.resetWallRunSpeed //by overriding the mathf.lerp coroutine.
            }

            //restart the race
            if (raceManager != null)
            {
                Debug.Log("Race Restarted");
                raceManager.ResetRace();
            }

            //Restart the checkpoints for the race.

            Debug.Log("Player respawned at: " + respawnPoint.position);
        }
    }
}