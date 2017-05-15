/*Ultimate Inventory 5 (2016)
This script is designed in order to help UInventory 5 users, 
You are not supposed to edit this script so there is no documentation about it,
however you are allowed to modify and extend this custom editor.
*/
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(UI_Container))]
public class UI_ContainerEditor : Editor {


    private SerializedObject m_Object;
    private SerializedProperty m_Property;

    void OnEnable()
    {
        m_Object = new SerializedObject(target);
    }

    int selectedTab = 0;

    void DrawGeneral()
    {
        m_Property = m_Object.FindProperty("toggleKey");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Toggle Key :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("maxOpenRange");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Max Open Range :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("autoClose");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Auto Close :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("defaultEmpty");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Empty Slot Icon :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("autoOpenInv");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Auto Toggle Inventory :"), true);
        m_Object.ApplyModifiedProperties();

    }

    void DrawAnimation()
    {
        m_Property = m_Object.FindProperty("animated");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Is Animated :"), true);
        m_Object.ApplyModifiedProperties();
        if (m_Object.FindProperty("animated").boolValue == true)
        {
            m_Property = m_Object.FindProperty("anims");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Animation Gameobject :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("openAnim");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Open Animation :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("useCloseAnim");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Use Close Animation :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("useCloseAnim").boolValue == true)
            {
                m_Property = m_Object.FindProperty("closeAnim");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Close Animation :"), true);
                m_Object.ApplyModifiedProperties();
            }
        }
    }

    void DrawAudio()
    {
        m_Property = m_Object.FindProperty("useSounds");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Use Audio :"), true);
        m_Object.ApplyModifiedProperties();

        if (m_Object.FindProperty("useSounds").boolValue == true)
        {
            m_Property = m_Object.FindProperty("sounds");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Audio Source :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("openSound");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Open Clip :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("useCloseAnim").boolValue == true)
            {
                m_Property = m_Object.FindProperty("closeSound");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Close Clip :"), true);
                m_Object.ApplyModifiedProperties();
            }
        }
    }

    void DrawSystem()
    {
        m_Property = m_Object.FindProperty("player");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Player Gameobject :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("containerUI");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Container UI :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("containerHolder");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Slots Holder :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("inventoryGO");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Inventory GO :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("playerEquipGO");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("PlayerEquipemnt GO :"), true);
        m_Object.ApplyModifiedProperties();
    }

    void DrawContainer()
    {
        m_Property = m_Object.FindProperty("autoAddSlots");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Auto Add Slots :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("autoAdjustSlots");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Auto Adjust Slots :"), true);
        m_Object.ApplyModifiedProperties();
        if (m_Object.FindProperty("autoAddSlots").boolValue == false)
        {
            m_Property = m_Object.FindProperty("containerSlots");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Container Slots :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("stackSlots");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Slots Stack :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("slotsGO");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Slot Gameobjects :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("allDur");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Slots Durability :"), true);
            m_Object.ApplyModifiedProperties();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("If you are not an Advanced User, check 'Auto Add Slots'", MessageType.Warning);
        }
        if (m_Object.FindProperty("autoAdjustSlots").boolValue == false)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("If you are not an Advanced User, check 'Auto Adjust Slots'", MessageType.Warning);
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Ultimate Inventory 2016 - Container");
        EditorGUILayout.LabelField("");

        GUILayout.BeginHorizontal();

        if (GUILayout.Toggle(selectedTab == 0, "General Settings", EditorStyles.toolbarButton, GUILayout.Width(130)))
            selectedTab = 0;

        if (GUILayout.Toggle(selectedTab == 1, "Animation Settings", EditorStyles.toolbarButton, GUILayout.Width(130)))
            selectedTab = 1;

        if (GUILayout.Toggle(selectedTab == 2, "Audio Settings", EditorStyles.toolbarButton, GUILayout.Width(130)))
            selectedTab = 2;


        if (GUILayout.Toggle(selectedTab == 3, "System Settings", EditorStyles.toolbarButton, GUILayout.Width(130)))
            selectedTab = 3;


        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Toggle(selectedTab == 4, "Container Slots", EditorStyles.toolbarButton, GUILayout.Width(130)))
            selectedTab = 4;

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");

        if (selectedTab == 0)
            DrawGeneral();
        else if (selectedTab == 1)
            DrawAnimation();
        else if (selectedTab == 2)
            DrawAudio();
        else if (selectedTab == 3)
            DrawSystem();
        else if (selectedTab == 4)
            DrawContainer();
    }
}
