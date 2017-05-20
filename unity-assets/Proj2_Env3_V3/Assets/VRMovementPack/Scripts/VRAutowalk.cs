using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class VRAutowalk : MonoBehaviour {
    // How fast to move
    public float speed = 3.0F;
    // Should I move forward or not
    private bool moveForward;
    // CharacterController script    
    private CharacterController myCC;
    // VR Main Camera
    private Transform vrCamera;

    // Use this for initialization
    void Start() {
        // Find the CharacterController
        myCC = GetComponent<CharacterController>();
        // Find the VR Head
        vrCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update() {
        // In the Google VR button, or the Gear VR touchpad is pressed
        if (Input.GetButtonDown("Fire1")) {
            // Change the state of moveForward
            moveForward = !moveForward;
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