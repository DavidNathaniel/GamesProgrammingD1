using UnityEngine;

public class PlayerCamerafps : MonoBehaviour
{
    //sensitivity
    public float senY;
    public float senX;

    public Transform orientation;

    public float xRotation; //these are set as the default player camera orientation.
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

        xRotation -= mouseY;
        yRotation += mouseX;
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //lock the camera so can only look up and down a maximum of 90 degrees.

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    //force player to look at a specified direction
    public void forcedOrientation(float xRot, float yRot)
    {
        xRotation = xRot;
        yRotation = yRot;

        //transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        //orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }

}
