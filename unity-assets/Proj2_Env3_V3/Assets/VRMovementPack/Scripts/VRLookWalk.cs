using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class VRLookWalk : MonoBehaviour {
    // Angle at which walk/stop will be triggered (X value of main camera)
    public float toggleAngle = 30.0f;
    // How fast to move
    public float speed = 3.0f;
    // Should I move forward or not
    private bool moveForward;
    // VR Main Camera
    private Transform vrCamera;
    // CharacterController script    
    private CharacterController myCC;

    // Use this for initialization
    void Start () {
        // Find the CharacterController
        myCC = GetComponent<CharacterController>();
        // Find the Main Camera
        vrCamera = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        // Check to see if the head has rotated down to the toggleangle, but not more than straight down 
        if (vrCamera.eulerAngles.x >= toggleAngle && vrCamera.eulerAngles.x < 90.0f) {
            // Move forward
            moveForward = true;
        }
        else {
            // Stop moving
            moveForward = false;
        }

        // Check to see if I should move
        if (moveForward) {
            // Find the forward direction
            Vector3 forward = vrCamera.TransformDirection(Vector3.forward);
            // Tell CharacterController to move forward
            myCC.SimpleMove(forward * speed);
        }
    }
}
