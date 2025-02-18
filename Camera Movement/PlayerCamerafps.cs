using UnityEngine;

public class PlayerCamerafps : MonoBehaviour
{
    //sensitivity
    public float senY;
    public float senX;

    public Transform orientation;

    public float xRotation;
    public float yRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //lock the camera so can only look up and down a maximum of 90 degrees.

        //maybe this is where i can hardcode the direciton we want the player to be looking on each level?
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);    

    }

}
