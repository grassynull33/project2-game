using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Waypoint))]
public class WaypointDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        int oldIndentLevel = EditorGUI.indentLevel;
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        contentPosition.width *= 1f;
        EditorGUI.indentLevel = 0;
        //EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("name"), GUIContent.none);
        //contentPosition.x += contentPosition.width;
        //contentPosition.width *= 2f;
        //EditorGUIUtility.labelWidth = 14f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("gameObject"), GUIContent.none);
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }
}