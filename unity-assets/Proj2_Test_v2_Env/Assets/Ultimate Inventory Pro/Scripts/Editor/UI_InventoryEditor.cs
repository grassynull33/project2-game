/*Ultimate Inventory 5 (2016)
This script is designed in order to help UInventory 5 users, 
You are not supposed to edit this script so there is no documentation about it,
however you are allowed to modify and extend this custom editor.
*/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Net;

[CustomEditor (typeof(UI_Inventory))]
public class UI_InventoryEditor : Editor
{
    private SerializedObject m_Object;
    private SerializedProperty m_Property;

    Texture2D uibanner;
    bool storeSettings = true;
    bool remLast = true;
    bool defaultEditor = false;
    string updateS = "On Enable";
    bool ask1 = false;
    bool neverask1 = false;
    void SaveSettings()
    {
        PlayerPrefs.SetString("save", storeSettings.ToString());
        if (storeSettings== true)
        {
            try
            {
                PlayerPrefs.SetString("viewStyle", tabView.ToString());
                if (remLast == true)
                    PlayerPrefs.SetInt("lastTab", selectedTab);
                PlayerPrefs.SetString("rem", remLast.ToString());
                PlayerPrefs.SetString("def", defaultEditor.ToString());
                PlayerPrefs.SetString("update", updateS);
                PlayerPrefs.SetString("nask1", neverask1.ToString());
                PlayerPrefs.Save();
            }
            catch { Debug.LogError("UInventory 5 - Editor settings can not be saved!"); }
        }
    }

    void LoadSettings()
    {
        storeSettings = bool.Parse(PlayerPrefs.GetString("save", "True"));
        try
        {
            tabView = bool.Parse(PlayerPrefs.GetString("viewStyle", "False"));
            if (tabView == false)
                styleC_index = 0;
            else
                styleC_index = 1;

            remLast = bool.Parse(PlayerPrefs.GetString("rem", "true"));

            if (remLast == true)
                selectedTab = PlayerPrefs.GetInt("lastTab", 0);
            else
                selectedTab = 0;

            updateS = PlayerPrefs.GetString("update", "Never");

            if (updateS == "Never")
                updateC_index = 1;
            else
                updateC_index = 0;

            defaultEditor = bool.Parse(PlayerPrefs.GetString("def", "False"));
            defaultEditorP = defaultEditor;

            neverask1 = bool.Parse(PlayerPrefs.GetString("nask1", "False"));
        }
        catch { Debug.LogError("UInventory 5 - Editor settings can not be loaded!." + System.Environment.NewLine + "Settings are now set up on default."); }
    }

    void OnEnable()
    {
        m_Object = new SerializedObject(target);
        uibanner = Resources.Load("banner") as Texture2D;
        LoadSettings();

        if (updateS != "Never")
            ScanForUpdates(true);
    }

    int selectedTab = 0;
    bool tabView = false;
    public override void OnInspectorGUI()
    {
        if (ask1 == false)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(uibanner,GUILayout.Width(450),GUILayout.Height(285));
            GUILayout.EndHorizontal();
            GUILayout.Label("");

            if (defaultEditor == false)
            {
                if (tabView == false)
                {
                    DrawCategory(0, "General Settings");
                    GeneralSettings();
                    DrawCategory(1, "Setup Settings");
                    SetupSettings();
                    DrawCategory(2, "Inventory Settings");
                    InventorySettings();
                    DrawCategory(3, "UI Settings");
                    UISettings();
                    DrawCategory(4, "Editor Settings");
                    EditorSettings();
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    DrawTab(0, "General Settings");
                    DrawTab(1, "Setup Settings");
                    DrawTab(2, "Inventory Settings");
                    DrawTab(3, "UI Settings");
                    DrawTab(4, "Editor Settings");
                    GUILayout.EndHorizontal();
                    GeneralSettings();
                    SetupSettings();
                    InventorySettings();
                    UISettings();
                    EditorSettings();
                }
            }
            else
            {
                DrawDefaultInspector();
                GUILayout.Label("");
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUILayout.Label("Default Editor :", GUILayout.Width(130));
                defaultEditor = GUILayout.Toggle(defaultEditor, "");
                defaultEditorP = defaultEditor;
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            DrawAsk1();
        }
    }
    bool na1 = false;
    void DrawAsk1()
    {
        EditorGUILayout.HelpBox("Are you sure you want to scan for updates ? If you proceed Unity Engine will access the internet !",MessageType.Warning);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Yes", EditorStyles.miniButton, GUILayout.Width(100)))
        {
            neverask1 = na1;
            SaveSettings();
            ask1 = false;
            ScanForUpdates();
        }
        if (GUILayout.Button("No", EditorStyles.miniButton, GUILayout.Width(100)))
        {
            neverask1 = na1;
            SaveSettings();
            ask1 = false;
        }
        na1 = GUILayout.Toggle(na1, "Never ask me again");
        GUILayout.EndHorizontal();

    }

    void DrawCategory(int tab,string tabName)
    {
        if (selectedTab == tab)
            GUI.enabled = false;
        else
            GUI.enabled = true;
        if (GUILayout.Button(tabName, GUILayout.Height(25)))
        {
            selectedTab = tab;
            SaveSettings();
        }
        GUI.enabled = true;
    }

    void DrawTab(int tab, string tabName)
    {
        if (GUILayout.Toggle(selectedTab == tab, tabName, EditorStyles.toolbarButton, GUILayout.Width(130)))
        {
            selectedTab = tab;
            SaveSettings();
        }
    }

    void GeneralSettings()
    {
        if (selectedTab == 0)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("");
            m_Property = m_Object.FindProperty("autoAddSlots");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Auto add slots :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("autoAddSlots").boolValue == false)
            {
                EditorGUILayout.HelpBox("Warning : It is not recommended to use manual settings !", MessageType.Warning);
            }
             m_Property = m_Object.FindProperty("autoAdjustSlotsID");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Auto adjust slots ID :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("autoAdjustSlotsID").boolValue == false)
            {
                EditorGUILayout.HelpBox("Warning : It is not recommended to use manual settings !", MessageType.Warning);
            }
            m_Property = m_Object.FindProperty("dragIconFactor");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Dragged icon resize factor :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("autoAddSlots").boolValue == false)
            {
                GUI.enabled = true;
            }
            else
                GUI.enabled = false;
            m_Property = m_Object.FindProperty("allSlots");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("All items on inventory :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("allSlotsStack");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Stack of all items on inventory :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("allDur");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Durability of all items on inventory :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("slotGOs");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Auto add inventory slots GO :"), true);
            m_Object.ApplyModifiedProperties();
            GUI.enabled = true;
            GUILayout.EndVertical();
        }
    }

    void SetupSettings()
    {
        if (selectedTab == 1)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.HelpBox("Warning: Please do not modify any of these values if you are not an advanced user!", MessageType.Warning);
            GUILayout.Label("");
            m_Property = m_Object.FindProperty("player");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Player gameObject :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("dropPos");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Drop item gameObject :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.BeginHorizontal();
            m_Property = m_Object.FindProperty("eventManager");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Event manager system :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.Label("Note: This feature is not fully supported yet!");
            GUILayout.EndHorizontal();
            m_Property = m_Object.FindProperty("cam");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Main camera :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.EndVertical();
        }
    }
    void InventorySettings()
    {
        if (selectedTab == 2)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("");
            GUILayout.Label("Key Settings :");
            GUILayout.BeginVertical(EditorStyles.helpBox);
            m_Property = m_Object.FindProperty("toggleInv");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Inventory toggle key :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("raycastOnMouse");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Pick up with left click :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("raycastOnMouse").boolValue == false)
                GUI.enabled = true;
            else
                GUI.enabled = false;
            m_Property = m_Object.FindProperty("pickupKey");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Pick up item key :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("countWeight");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Enable inventory weight system :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("countWeight").boolValue == true)
                GUI.enabled = true;
            else
                GUI.enabled = false;
            m_Property = m_Object.FindProperty("speedFactor");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Speed factor when the max weight is reached :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("totalWeight");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Max weight player can carry :"), true);
            m_Object.ApplyModifiedProperties();
            GUI.enabled = true;
            GUILayout.EndVertical();

            GUILayout.Label("Inventory Panels & Features :");
            GUILayout.BeginVertical(EditorStyles.helpBox);
            m_Property = m_Object.FindProperty("allowDragDrop");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Drag and drop :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("dropWhenDragOutOfInv");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Drop item when is dragged out of any slot :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("allowContextMenu");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Right click menu :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("allowPreviewWin");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Preview panel :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("hideCursor");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Hide cursor on toggle :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.EndVertical();
            GUILayout.Label("");
            m_Property = m_Object.FindProperty("pickupRange");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Pickup range :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.Label("");
            m_Property = m_Object.FindProperty("disableComponentOnToggle");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Disable components on inventory toggle :"), true);
            m_Object.ApplyModifiedProperties();
            if (m_Object.FindProperty("disableComponentOnToggle").boolValue == true)
                GUI.enabled = true;
            else
                GUI.enabled = false;
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.HelpBox("Add here all the components that needs to be disabled when you toggle the inventory !", MessageType.Info);
            m_Property = m_Object.FindProperty("components");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Components to disable :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.EndVertical();
            GUI.enabled = true;
            m_Property = m_Object.FindProperty("fullSound");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Inventory full sound :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("pickupItemSound");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Pickup item sound :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.EndVertical();
        }
    }

    void UISettings()
    {
        if (selectedTab == 3)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Color Settings :");
            m_Property = m_Object.FindProperty("fullDur");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Full durability color :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("halfDur");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Half durability color :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("lowDur");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Low durability color :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.EndVertical();
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Gameobject & other Settings :");
            m_Property = m_Object.FindProperty("defaultEmpty");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Default icon for empty slots :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("contextMenu");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Context Menu GO :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("playerEquipmentGO");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Player equipment GO :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("itemName");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Item name label :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("itemIcon");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Item icon GO :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("previewPanel");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Preview panel :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("durBar");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Durability bar GO :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("ecButton");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Consume button GO :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.EndVertical();
        }
    }

    string[] styleC = new[] { "Categories", "Tabview" };
    int styleC_index = 0;
    string[] updateC = new[] { "On Enable", "Never" };
    int updateC_index = 0;
    bool defaultEditorP = false;
    void EditorSettings()
    {
        if (selectedTab == 4)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Editor Style :", GUILayout.Width(160));
            styleC_index = EditorGUILayout.Popup(styleC_index, styleC, EditorStyles.toolbarPopup);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Save Editor Settings :", GUILayout.Width(160));
            storeSettings = GUILayout.Toggle(storeSettings, "", EditorStyles.toggle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (tabView == true)
                GUILayout.Label("Remember Last Tab :", GUILayout.Width(160));
            else
                GUILayout.Label("Remember Last Category :", GUILayout.Width(160));
            remLast = GUILayout.Toggle(remLast, "", EditorStyles.toggle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Default Editor :", GUILayout.Width(160));
            defaultEditorP = GUILayout.Toggle(defaultEditorP, "", EditorStyles.toggle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Check For Updates :", GUILayout.Width(160));
            updateC_index = EditorGUILayout.Popup(updateC_index, updateC, EditorStyles.toolbarPopup);
            GUILayout.EndHorizontal();
            GUILayout.Label("");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Installed Version :", GUILayout.Width(160));
            GUILayout.Label(UI_Inventory.currentVersion);
            GUILayout.EndHorizontal();
            GUILayout.Label("");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply", EditorStyles.miniButton,GUILayout.Width(150)))
            {
                if (styleC_index == 0)
                    tabView = false;
                else
                    tabView = true;
                updateS = updateC[updateC_index];
                defaultEditor = defaultEditorP;
                SaveSettings(); 
            }
            if (GUILayout.Button("Check For Updates", EditorStyles.miniButton, GUILayout.Width(150)))
            {
                if (neverask1 == false)
                    ask1 = true;
                else
                    ScanForUpdates();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }

    void ScanForUpdates(bool isForced = false)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                }
                catch { }
                string s = client.DownloadString("http://greekstudios.url.ph/uinv5_ver.html");
                if (s != UI_Inventory.currentVersion)
                {
                    UI_UpdateWindow.ShowWindow("A new version of Ultimate Inventory 5 is available." + System.Environment.NewLine + "You may visit Unity Asset Store in order to update UInventory 5" + System.Environment.NewLine + System.Environment.NewLine + "From Version : " + UI_Inventory.currentVersion + " to " + s);
                }
                else
                {
                  if (isForced== false)
                        UI_UpdateWindow.ShowWindow("You are running the latest Ultimate Inventory 5 Version.");
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning("Ultimate Inventory 5 can't check for updates! Details on the next log.");
            Debug.LogException(ex);
        }
    }
}
