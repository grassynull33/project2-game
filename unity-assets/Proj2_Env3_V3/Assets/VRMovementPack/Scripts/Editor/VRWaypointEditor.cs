using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VRWaypoint)), CanEditMultipleObjects]
public class VRWaypointEditor : Editor {

    // All the waypoints in the scene
    private static GameObject[] allWaypoints;

    // Serialized Object to hold data for the Waypoint List 
    //private SerializedObject myObject;

    //
    void OnEnable() {   
    }

    // 
    public override void OnInspectorGUI() {
        // Re-serialize the values
        serializedObject.Update();
        //EditorGUILayout.LabelField("Gaze Timer:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gazeTimer"), new GUIContent("Gaze Timer", "How long to look at waypoint before moving"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerSpeed"), new GUIContent("Player Speed", "Player's speed when moving to this waypoint"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerOffset"), new GUIContent("Player Offset", "Player's offset from center point when at waypoint"));
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(new GUIContent("Connected Waypoints:", "List of waypoints that are connected to this one"), EditorStyles.boldLabel);
        WaypointList.Show(serializedObject.FindProperty("connectedWaypoints"), WaypointListOption.Buttons);
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(new GUIContent("New Waypoint Prefix:", "Prefix for newly created waypoint"), EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("newWaypointPrefix"), GUIContent.none);
        // Apply modifications
        serializedObject.ApplyModifiedProperties();

        VRWaypoint myScript = (VRWaypoint)target;
        if (GUILayout.Button("Add Connected Waypoint")) {
            
            // Spawn a new waypoint Gameobject
            GameObject newWaypoint = null;
            // Try to find a source prefab object
            Object prefabRoot = PrefabUtility.GetPrefabParent(myScript.gameObject);
            // If there is a prefab
            if (prefabRoot != null) {
                // Spawn the prefab
                newWaypoint = PrefabUtility.InstantiatePrefab(prefabRoot) as GameObject;
            }
            // This gameobject is not a prefab
            else {
                // Just spawn this gameobject
                newWaypoint = Instantiate(myScript.gameObject) as GameObject;
            }

            // Set the Waypoint name
            newWaypoint.gameObject.name = myScript.newWaypointPrefix + CountAllGameobjectWithPrefix(myScript.newWaypointPrefix, 4);
            // Set the Waypoint Tag
            newWaypoint.gameObject.tag = "Waypoint";

            //VRWaypointEditor newWaypointEditor = newWaypoint .GetComponent<VRWaypointEditor>();


            // Find the VRWaypoint script on the new waypoint
            VRWaypoint newWaypointScript = newWaypoint.GetComponent<VRWaypoint>();

            // Reset the connected waypoints script on the new waypoint
            newWaypointScript.connectedWaypoints = null;
            // Set the Waypoints Prefix text
            newWaypointScript.waypointPrefix = myScript.newWaypointPrefix;
            // Set the New Waypoint Prefix text
            newWaypointScript.newWaypointPrefix = myScript.newWaypointPrefix;

            // Set position near existing waypoint
            newWaypoint.transform.position = myScript.gameObject.transform.position + new Vector3(1f, 0f, 0f);

            //------------------------------------------------
            // Connected Waypoints Array Management
            //------------------------------------------------
            // Add an array element to this gameobject
            WaypointList.AddArrayElement(serializedObject.FindProperty("connectedWaypoints"));            
            // Get the serializedObject from the new waypoint's VRWaypoint script
            SerializedObject newWaypointObject = new SerializedObject(newWaypointScript);
            // Add an array element to the new waypoint
            WaypointList.AddArrayElement(newWaypointObject.FindProperty("connectedWaypoints"));
            // Apply modifications to this object
            serializedObject.ApplyModifiedProperties();
            // Apply modifications to new waypoint
            newWaypointObject.ApplyModifiedProperties();

            // Set new waypoint's connection to me
            newWaypointScript.connectedWaypoints[0].gameObject = myScript.gameObject;
            // Set my connected waypoint to the new waypoint
            myScript.connectedWaypoints[myScript.connectedWaypoints.Length - 1].gameObject = newWaypoint;


            //------------------------------------------------
            // Set Editor Selection
            //------------------------------------------------
            // Create a new array of gameobjects, with a length of 1
            GameObject[] newWaypoints = new GameObject[1];
            // Set the new waypoint as the only value of the array
            newWaypoints[0] = newWaypoint;
            // Set Editor selection to the array of gameobjects (only the new waypoint)
            Selection.objects = newWaypoints;

        }
        
    }

    public void AddArrayElement() {
        Debug.Log("hi");
    }

    // string prefix: what is the gameobject's string prefix
    // numbersLength: is how many numbers are in waypoints name, for example: 1, 01, 001, 0001
    private string CountAllGameobjectWithPrefix(string prefix, int numbersLength) {
        // Will be the final number of waypoitns way passed prefix
        int finalNumber = 0;
        // A string of the final number, may have 0's in front of it, based on numbersLength variable
        string finalNumberString = "";
        // A string of 0's which will be put in front of the string final number
        string zerosInFrontOfNumber = "";

        // Find all the waypoints in the scene based on Tag
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        // Look at all of the waypoints
        foreach (GameObject i in allWaypoints) {
            // If the waypoint's prefix matches the one we are creating
            if (i.gameObject.GetComponent<VRWaypoint>().waypointPrefix == prefix) {
                // Add this waypoint to the count
                finalNumber++;
            }
        }
        // Get the final count to a string
        finalNumberString = finalNumber.ToString();

        // Make sure the user doesn't make over 1000 waypoints
        if (finalNumberString.Length > numbersLength) {
            // Just log a warning, waypoint numbers after 1000 will not have 0s in front of them
            Debug.LogWarning("You are creating way too many waypoints!!");
        }
        else {
            // Find out how many Zeros we need to add to the beginning
            int numberOfZeros = numbersLength - finalNumberString.Length;

            // For the number of zeros we should have
            for (int i = 0; i < numberOfZeros; i++) {
                // Add a '0' to the string
                zerosInFrontOfNumber += "0";
            }
            // Update the final number string with zeros in front
            finalNumberString = zerosInFrontOfNumber + finalNumberString;
        }
        // Return the value to where the function was called from
        return finalNumberString;
    }
}
