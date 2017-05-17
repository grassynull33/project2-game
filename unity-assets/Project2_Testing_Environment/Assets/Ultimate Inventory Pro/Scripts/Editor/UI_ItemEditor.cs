using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UI_Item))]
public class UI_ItemEditor : Editor
{

    private SerializedObject m_Object;
    private SerializedProperty m_Property;

    int selectedTab = 0;
    void OnEnable()
    {
        m_Object = new SerializedObject(target);

        if (m_Object.FindProperty("itemCat").stringValue == "Normal")
            itemC_index = 0;
        else if (m_Object.FindProperty("itemCat").stringValue == "Consumable")
            itemC_index = 1;
        else
            itemC_index = 2;

        if (m_Object.FindProperty("equipmentCat").stringValue == "Head")
            equipC_index = 0;
        else if (m_Object.FindProperty("equipmentCat").stringValue == "Chest")
            equipC_index = 1;
        else if (m_Object.FindProperty("equipmentCat").stringValue == "Hands")
            equipC_index = 2;
        else if (m_Object.FindProperty("equipmentCat").stringValue == "Leggies")
            equipC_index = 3;
        else if (m_Object.FindProperty("equipmentCat").stringValue == "Feet")
            equipC_index = 4;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("");
        GUILayout.BeginHorizontal();
        DrawTab(0, "Item Settings");
        DrawTab(1, "Crafting Settings");
        GUILayout.EndHorizontal();
        DrawItemSettings();
        DrawCraftingSettings();
    }

    void DrawTab(int tab, string tabName)
    {
        if (GUILayout.Toggle(selectedTab == tab, tabName, EditorStyles.toolbarButton, GUILayout.Width(130)))
        {
            selectedTab = tab;
        }
    }
    string[] itemC = new[] { "Normal", "Consumable", "Equipment" };
    int itemC_index = 0;
    string[] equipC = new[] { "Head", "Chest", "Hands", "Leggies", "Feet" };
    int equipC_index = 0;
    void DrawItemSettings()
    {
        if (selectedTab == 0)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            m_Property = m_Object.FindProperty("itemID");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Item ID :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("name");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Item Name :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("itemPreview");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Item Image Preview :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("desc");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Item Description :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("maxStack");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Max Stack :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("itemWeight");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Item Weight :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("hasDurability");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Enable Durability :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("hasDurability").boolValue == true)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                m_Property = m_Object.FindProperty("itemDurability");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Item Durability :"), true);
                m_Object.ApplyModifiedProperties();
                m_Property = m_Object.FindProperty("useDurability");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Decrease Durability After Use :"), true);
                m_Object.ApplyModifiedProperties();
                GUILayout.EndVertical();
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Item Category :", GUILayout.Width(204));
            itemC_index = EditorGUILayout.Popup(itemC_index, itemC, EditorStyles.toolbarPopup);
            if (itemC_index == 0)
                m_Object.FindProperty("itemCat").stringValue = "Normal";
            else if (itemC_index == 1)
                m_Object.FindProperty("itemCat").stringValue = "Consumable";
            else
                m_Object.FindProperty("itemCat").stringValue = "Equipment";
            m_Object.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
            if (m_Object.FindProperty("itemCat").stringValue == "Consumable")
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                m_Property = m_Object.FindProperty("statsToEditOnConsume");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Stats To Edit When Used :"), true);
                m_Object.ApplyModifiedProperties();
                GUILayout.EndVertical();
            }
            else if (m_Object.FindProperty("itemCat").stringValue == "Equipment")
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Equipment Category :", GUILayout.Width(200));
                equipC_index = EditorGUILayout.Popup(equipC_index, equipC, EditorStyles.toolbarPopup);
                if (equipC_index == 0)
                    m_Object.FindProperty("equipmentCat").stringValue = "Head";
                else if (equipC_index == 1)
                    m_Object.FindProperty("equipmentCat").stringValue = "Chest";
                else if (equipC_index == 2)
                    m_Object.FindProperty("equipmentCat").stringValue = "Hands";
                else if (equipC_index == 3)
                    m_Object.FindProperty("equipmentCat").stringValue = "Leggies";
                else if (equipC_index == 4)
                    m_Object.FindProperty("equipmentCat").stringValue = "Feet";
                m_Object.ApplyModifiedProperties();
                GUILayout.EndHorizontal();
                m_Property = m_Object.FindProperty("objectsToEquip");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Objects To Enable When Equipped :"), true);
                m_Object.ApplyModifiedProperties();
                GUILayout.EndVertical();
            }

            m_Property = m_Object.FindProperty("destroyOnUse");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Destroy On Use :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("pickUpWhenOver");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Pick Up Item When Player Is Over :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.EndVertical();
        }
    }
    void DrawCraftingSettings()
    {
        if (selectedTab == 1)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            m_Property = m_Object.FindProperty("isCraftable");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Is Craftable :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("isCraftable").boolValue == true)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                m_Property = m_Object.FindProperty("craftingTime");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Crafting Time :"), true);
                m_Object.ApplyModifiedProperties();
                EditorGUILayout.HelpBox("Use the ID of the item !", MessageType.Info);
                m_Property = m_Object.FindProperty("resourcesID");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Resources Required For Crafting :"), true);
                m_Object.ApplyModifiedProperties();
                m_Object.FindProperty("resourcesAmount").arraySize = m_Object.FindProperty("resourcesID").arraySize;
                m_Property = m_Object.FindProperty("resourcesAmount");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Amount Of Resources Required :"), true);
                m_Object.ApplyModifiedProperties();
                m_Object.FindProperty("resourcesID").arraySize = m_Object.FindProperty("resourcesAmount").arraySize;
                GUILayout.EndVertical();
            }
            m_Property = m_Object.FindProperty("isBlueprint");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Is BluePrint :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("isBlueprint").boolValue == true)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.HelpBox("Assign the crafting UI from the Canvas", MessageType.Info);
                m_Property = m_Object.FindProperty("blueprintUnlock");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("Item To Unlock :"), true);
                m_Object.ApplyModifiedProperties();
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
    }

}
