using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VRWalkableSurface : MonoBehaviour {

    // Player Gameobject, VRMain
    private GameObject player;

    // EventTrigger for PointerEnter, Exit, Click events
    private EventTrigger myTrigger;
    
    // NavMesh agent, should be on VR Player, gameobject with tag "Player"
    private UnityEngine.AI.NavMeshAgent navmeshAgent;

    // NavMesh hit, similar to raycast hit
    private UnityEngine.AI.NavMeshHit navmeshHit;
    
    // If the path is blocked
    private bool blocked = false;

    // Use this for initialization
    void Start() {
        // Find VRMain by Player Tag
        player = GameObject.FindGameObjectWithTag("Player");
        // Find NavMesh Agent on Player
        navmeshAgent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();

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

    // Pointer Enter Event
    public void OnPointerEnter() {
    }

    // Pointer Exit Event
    public void OnPointerExit() {
    }

    // Pointer Click Event
    public void OnPointerClick() {
    }

    // Pointer Up Event
    public void OnPointerUp() {
    }

    // Pointer Down Event
    public void OnPointerDown() {
        // Variable to hold information about what is hit
        RaycastHit hit;
        // RAY HAS A MAX DISTANCE OF 100! (Should automatically match reticle max distance)
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f)) {
            // Check to see if the path is blocked
            blocked = UnityEngine.AI.NavMesh.Raycast(navmeshAgent.gameObject.transform.position, hit.point, out navmeshHit, UnityEngine.AI.NavMesh.AllAreas);
            // Draw a line in the editor, green if not blocked, red if blocked
            Debug.DrawLine(navmeshAgent.gameObject.transform.position, hit.point, blocked ? Color.red : Color.green, 10f);
            // The path is blocked
            if (blocked) {
                // Draw a vertical line in the editor at the position the block occurred 
                Debug.DrawRay(navmeshHit.position, Vector3.up, Color.red, 10f);
                // Set the navmesh destination to this point- the point where the block occurred
                navmeshAgent.SetDestination(navmeshHit.position);
            }
            // The path is not blocked
            else {
                // Update navmesh destination
                navmeshAgent.SetDestination(hit.point);
            }
        }
    }
}