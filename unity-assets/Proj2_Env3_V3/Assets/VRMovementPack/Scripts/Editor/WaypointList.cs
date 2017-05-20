using UnityEngine;
using UnityEditor;
using System;

public static class WaypointList {

    public static void Show(SerializedProperty list, WaypointListOption options = WaypointListOption.Default) {

        if (!list.isArray) {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }

        bool showListLabel = (options & WaypointListOption.ListLabel) != 0;
        bool showListSize = (options & WaypointListOption.ListSize) != 0;

        if (showListLabel) {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        
        if (!showListLabel || list.isExpanded) {
            if (showListSize) {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            ShowElements(list, options);
        }

        if (showListLabel) {
            EditorGUI.indentLevel -= 1;
        }
    }

    private static GUIContent
        moveButtonContent = new GUIContent("\u21b4", "Move Down"),
        duplicateButtonContent = new GUIContent("+", "Add Waypoint Manually"),
        deleteButtonContent = new GUIContent("-", "Delete Waypoint From List");
        //addButtonContent = new GUIContent("+", "Add Waypoint");

    private static void ShowElements(SerializedProperty list, WaypointListOption options) {
        bool showElementLabels = (options & WaypointListOption.ElementLabels) != 0;
        bool showButtons = (options & WaypointListOption.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++) {
            if (showButtons) {
                EditorGUILayout.BeginHorizontal();
            }

            if (showElementLabels) {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }
            if (showButtons) {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }

        /*
        // Show "Add" Button when array size is 0
        if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
            list.arraySize += 1;
        }
        */
        if (list.arraySize == 0) {
            //EditorGUILayout.TextArea("None");
            EditorGUILayout.LabelField("None");
        }

    }

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    private static void ShowButtons(SerializedProperty list, int index) {
        if (GUILayout.Button(moveButtonContent, miniButtonWidth)) {
            list.MoveArrayElement(index, index + 1);
        }
        
        if (GUILayout.Button(duplicateButtonContent, miniButtonWidth)) {
            //list.InsertArrayElementAtIndex(index);
            list.arraySize += 1;
            
        }
        
        if (GUILayout.Button(deleteButtonContent, miniButtonWidth)) {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize) {
                list.DeleteArrayElementAtIndex(index);
            }

        }
    }

    public static void AddArrayElement(SerializedProperty list) {
        //list.InsertArrayElementAtIndex(list.arraySize);
        list.arraySize += 1;
    }
}

[Flags]
public enum WaypointListOption {
    None = 0,
    ListSize = 1,
    ListLabel = 2,
    ElementLabels = 4,
    Buttons = 8,
    Default = ListSize | ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}