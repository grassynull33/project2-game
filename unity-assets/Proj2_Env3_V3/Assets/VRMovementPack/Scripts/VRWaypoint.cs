using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VRWaypoint : MonoBehaviour {
    // ****************************************************************
    // Variables
    // ****************************************************************
    // How long to look at waypoint before moving
    [SerializeField]
    public float gazeTimer = 2f;

    // Player's speed when moving to this waypoint
    [SerializeField]
    public float playerSpeed = 3f;

    // Player's offset from this object's center point when moving here
    [SerializeField]
    public Vector3 playerOffset = new Vector3(0f, -1f, 0f);

    // This waypoint's prefix text
    [HideInInspector]
    public string waypointPrefix;
    
    // What the user wants a newly created waypoint to be called
    [SerializeField]
    public string newWaypointPrefix = "Waypoint";

    // List of waypoints that are connected to this one
    public Waypoint[] connectedWaypoints;

    // Animation Controller for playing 'Gazed at' animation
    private Animator myAnim;
    // Bool to track if waypoint has animation
    private bool hasAnimation;
    // Timer
    private float timer;
    // PointerEnter bool: If the gaze has entered this object
    private bool pointerEnter;
    // Player's root gameobject (VRMain)
    private GameObject player;
    // EventTrigger for PointerEnter, Exit, Click events
    private EventTrigger myTrigger;

    // ****************************************************************
    // Monobehaviour Functions
    // ****************************************************************
    // Use this for initialization
    void Start () {
        // Set timer's initial value
        timer = gazeTimer;
        // Find the player
        player = GameObject.FindGameObjectWithTag("Player");
        // Log an error if there is no object tagged "player"
        if (player == null) Debug.LogError("Please assign Tag 'Player' to the Main VR Player gameobject.");

        // Try to find Animator on myself
        if (myAnim = GetComponent<Animator>()) {
            // Set to use animations
            hasAnimation = true;
        }
        // Try to find Animator in children
        else if (myAnim = GetComponentInChildren<Animator>()) {
            // Set to use animations
            hasAnimation = true;
        }
        // There is no Animator, anywhere
        else {
            // Set to use no animations
            hasAnimation = false;
        }

        // Try to Find an EventTrigger Script on this GameObject
        myTrigger = gameObject.GetComponent<EventTrigger>();
        // If a script does not exist..
        if (myTrigger == null) {
            // .. then create one.
            myTrigger = gameObject.AddComponent<EventTrigger>();

            // Register the Event for "Pointer Enter" (cursor goes Over button)
            EventTrigger.Entry entryOver = new EventTrigger.Entry();
            entryOver.eventID = EventTriggerType.PointerEnter;
            entryOver.callback.AddListener((eventData) => { OnPointerEnter(); });
            myTrigger.triggers.Add(entryOver);

            // Register the Event for "Pointer Exit" (cursor goes Out of button)
            EventTrigger.Entry entryOut = new EventTrigger.Entry();
            entryOut.eventID = EventTriggerType.PointerExit;
            entryOut.callback.AddListener((eventData) => { OnPointerExit(); });
            myTrigger.triggers.Add(entryOut);

            // Register the Event for "Pointer Click" (physical button has been pressed down and back up)
            EventTrigger.Entry entryClick = new EventTrigger.Entry();
            entryClick.eventID = EventTriggerType.PointerClick;
            entryClick.callback.AddListener((eventData) => { OnPointerClick(); });
            myTrigger.triggers.Add(entryClick);

            // Register the Event for "Pointer Up" (physical button has been released)
            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((eventData) => { OnPointerUp(); });
            myTrigger.triggers.Add(entryUp);

            // Register the Event for "Pointer Down" (physical button has been pushed down)
            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((eventData) => { OnPointerDown(); });
            myTrigger.triggers.Add(entryDown);
        }
    }

    // Update is called once per frame
    void Update() {
        // Check if user is looking
        if (pointerEnter) {
            // Tell Animator we are being looked at
            if (hasAnimation) myAnim.SetBool("PointerEnter", true);
            // We won't go to the way point for X amount of time, so take delta time away from that value till we get to 0
            timer -= Time.deltaTime;
            // Timer up, move to waypoint
            if (timer < 0) {
                // Not being looked at anymore
                pointerEnter = false;
                // Reset the teleport timer
                timer = gazeTimer;
                // Disable visual componets of the waypoint
                DisableVisuals();
                // Move the player to this waypoint
                MovePlayerTo(transform.position, playerSpeed);
            }
        }
    }

    // ****************************************************************
    // Custom Functions
    // ****************************************************************
    // Move player to given Vector3 coordinates with a delay
    public void MovePlayerTo(Vector3 coords, float speed) {
        // Determine travel time
        float travelTime = (Vector3.Distance(player.transform.position, coords)) / speed;
        // Use itween to move player
        iTween.MoveTo(player, iTween.Hash("position", coords + playerOffset, "time", travelTime, "easetype", "linear"));
        // Enable waypoints
        StartCoroutine(EnableConnectedWaypoints(1f));
    }

    // Enable nearby waypoints
    public IEnumerator EnableConnectedWaypoints(float waypointEnableDelay) {
        // Waiting for x seconds
        yield return new WaitForSeconds(waypointEnableDelay);

        // Find all of the active waypoints
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        // For each active waypoint
        foreach (GameObject i in allWaypoints) {
            // Disable the gameobject
            i.SetActive(false);
        }
        
        // Enable all nearby waypoints
        foreach (Waypoint i in connectedWaypoints) {
            // Enable this waypoint
            if (i.gameObject != null) {
                i.gameObject.SetActive(true);
            }
            
            // *** Uncomment the following 2 lines to make a nice fade in ***
            // Set my material to clear
            //i.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.clear);
            // Tween back to Normal color
            //iTween.ColorTo(i.gameObject.transform.GetChild(0).gameObject, Color.white, 2f);
        }
        EnableVisuals();
        gameObject.SetActive(false);
    }

    // Enable Visual componets of this waypoint
    private void EnableVisuals() {
        gameObject.GetComponent<Collider>().enabled = (true);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    // Disable Visual componets of this waypoint
    private void DisableVisuals() {
        gameObject.GetComponent<Collider>().enabled = (false);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Gaze has entered this object
    public void PointerEnter() {
        // Pointer has entered
        pointerEnter = true;
    }

    // Gaze has Exited from this object
    public void PointerExit() {
        // Pointer has exited
        pointerEnter = false;
        // Disable animation
        if (hasAnimation && myAnim.isActiveAndEnabled) myAnim.SetBool("PointerEnter", false);
        // Reset timer
        timer = gazeTimer;
    }

    // ****************************************************************
    // Editor Gizmos
    // ****************************************************************
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        for (int i = 0; i < connectedWaypoints.Length; i++) {
            if (connectedWaypoints[i].gameObject != null) {
                Gizmos.DrawLine(transform.position, connectedWaypoints[i].gameObject.transform.position);
            }
        }
    }

    // ****************************************************************
    // Event Trigger Events, Added Automatically
    // ****************************************************************
    // Pointer Enter Event
    public void OnPointerEnter() {
        PointerEnter();
    }

    // Pointer Exit Event
    public void OnPointerExit() {
        PointerExit();
    }

    // Pointer Click Event
    public void OnPointerClick() {
    }

    // Pointer Up Event
    public void OnPointerUp() {
    }

    // Pointer Down Event
    public void OnPointerDown() {
        // Move instantly if a button is pressed
        timer -= 2f;
    }
}