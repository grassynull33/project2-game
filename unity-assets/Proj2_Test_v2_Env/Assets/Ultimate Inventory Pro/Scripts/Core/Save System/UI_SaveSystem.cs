using UnityEngine;
using System;
using UInventory;
using System.Collections.Generic;

public class UI_SaveSystem : MonoBehaviour {

    public bool testGUI = false;

    public string encryptPassword = "TestPasswordChangeIt_256bit_AES_encryption";
    public string saveName = "UI_Save";
    public bool forceVer = false;

    UI_Inventory inv;
    UI_ItemsList itemsList;
    UI_Hotbar htbar;
    public UI_PlayerEquipment equip;

    List<GameObject> containers = new List<GameObject>();

    void Awake()
    {
        inv = GetComponent<UI_Inventory>();
        itemsList = GetComponent<UI_ItemsList>();
        htbar = GetComponent<UI_Hotbar>();
        GameObject[] allGO = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allGO)
        {
            if (obj.GetComponent<UI_Container>() != null)
            {
                containers.Add(obj);
            }
        }
    }

    void OnGUI()
    {
        if (testGUI)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Inventory"))
            {
                SaveInventory();
            }
            if (GUILayout.Button("Load Inventory"))
            {
                LoadInventory();
            }
            GUILayout.EndHorizontal();
        }
    }

    public void SaveInventory()
    {
        try
        {
            string saveData = "[ver]" + UI_Inventory.currentVersion + "[/ver]" + Environment.NewLine
                + "[force_ver]" + forceVer.ToString() + "[/force_ver]" + Environment.NewLine + "[items]" + Environment.NewLine;

            for (int i=0; i<inv.allSlots.Count; i++)
            {
                if (inv.allSlots[i] != null)
                {
                    saveData += inv.allSlots[i].GetComponent<UI_Item>().itemID + Environment.NewLine;
                }
                else
                {
                    saveData += "-1" + Environment.NewLine;
                }
            }
            saveData += "[/items]" + Environment.NewLine + "[stack]" + Environment.NewLine;

            for (int i=0; i<inv.allSlotsStack.Count; i++)
            {
                if (inv.allSlots[i] != null)
                {
                    saveData += inv.allSlotsStack[i] + Environment.NewLine;
                }
                else
                {
                    saveData += "-1" + Environment.NewLine;
                }
            }
            saveData += "[/stack]" + Environment.NewLine + "[dur]" + Environment.NewLine;

            for (int i=0; i<inv.allDur.Count; i++)
            {
                if (inv.allSlots[i] != null)
                {
                    saveData += inv.allDur[i] + Environment.NewLine;
                }
                else
                {
                    saveData += "-1" + Environment.NewLine;
                }
            }
            saveData += "[/dur]" + Environment.NewLine;

            string encrypted = USecurity.EncryptString(saveData, encryptPassword);
            saveData = "0";

            PlayerPrefs.SetString(saveName, encrypted);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
        SaveHotbar();
        SaveEquipment();
    }

    public void LoadInventory() //this will replace the current items
    {
        try
        {
            string loadData = USecurity.DecryptString(PlayerPrefs.GetString(saveName), encryptPassword);

            string Lver = USecurity.ExtractBetween(loadData, "[ver]", "[/ver]", false);
            bool Lforce_ver = bool.Parse(USecurity.ExtractBetween(loadData, "[force_ver]", "[/force_ver]", false));
            string itemsString = USecurity.ExtractBetween(loadData, "[items]", "[/items]",false);
            string stackString = USecurity.ExtractBetween(loadData, "[stack]", "[/stack]", false);
            string durString = USecurity.ExtractBetween(loadData, "[dur]", "[/dur]", false);
            loadData = "0";
            if (Lforce_ver == false)
            {
                if (Lver == UI_Inventory.currentVersion)
                {
                    LoadItems(itemsString, stackString, durString);
                }
                else
                {
                    Debug.LogError("Save version is incompatible!");
                }
            }
            else
            {
                Debug.Log("Save file's version is forced!");
                LoadItems(itemsString, stackString, durString);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        LoadHotbar();
        LoadEquipment();
    }


    //Don't call this functions seperate, use the SaveInventory / Load Inventory !

    private void SaveHotbar()
    {
        try
        {
            string saveData = "[items]" + Environment.NewLine;

            for (int i=0; i<htbar.hotSlotsGO.Count; i++)
            {
                if (htbar.hotbarSlots[i] != null)
                {
                    saveData += htbar.hotbarSlots[i].GetComponent<UI_Item>().itemID + Environment.NewLine;
                }
                else
                {
                    saveData += "-1" + Environment.NewLine;
                }
            }

            saveData += "[/items]" + Environment.NewLine + "[stack]" + Environment.NewLine;

            for (int i=0; i<htbar.hotSlotsGO.Count; i++)
            {
                if (htbar.hotbarSlots[i] != null)
                {
                    saveData += htbar.hotbarStack[i] + Environment.NewLine;
                }
                else
                {
                    saveData += "-1" + Environment.NewLine;
                }
            }

            saveData += "[/stack]" + Environment.NewLine + "[dur]" + Environment.NewLine;

            for (int i = 0; i < htbar.hotSlotsGO.Count; i++)
            {
                if (htbar.hotbarSlots[i] != null)
                {
                    saveData += htbar.hotDurability[i] + Environment.NewLine;
                }
                else
                {
                    saveData += "-1" + Environment.NewLine;
                }
            }
            saveData += "[/dur]";

            string encrypted = USecurity.EncryptString(saveData, encryptPassword);
            saveData = "0";

            PlayerPrefs.SetString(saveName + ".htb", encrypted);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void LoadHotbar() //this will replace the current items
    {
        try
        {
            string loadData = USecurity.DecryptString(PlayerPrefs.GetString(saveName + ".htb"), encryptPassword);

            string itemsString = USecurity.ExtractBetween(loadData,"[items]", "[/items]", false);
            string stackString = USecurity.ExtractBetween(loadData, "[stack]", "[/stack]", false);
            string durString = USecurity.ExtractBetween(loadData, "[dur]", "[/dur]", false);
            loadData = "0";

            htbar.hotbarSlots.Clear();
            htbar.hotbarStack.Clear();
            htbar.hotDurability.Clear();

            string[] itemLines = itemsString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i=0; i<itemLines.Length; i++)
            {
                if (itemLines[i] != "")
                {
                    if (itemLines[i] != "-1")
                    {
                        htbar.hotbarSlots.Add(itemsList.allItems[int.Parse(itemLines[i])]);
                    }
                    else
                    {
                        htbar.hotbarSlots.Add(null);
                    }
                }
            }

            string[] stackLines = stackString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i=0; i<stackLines.Length; i++)
            {
                if (stackLines[i] != "")
                {
                    if (stackLines[i] != "-1")
                    {
                        htbar.hotbarStack.Add(int.Parse(stackLines[i]));
                    }
                    else
                    {
                        htbar.hotbarStack.Add(0);
                    }
                }
            }

            string[] durLines = durString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i=0; i<durLines.Length; i++)
            {
                if (durLines[i] != "")
                {
                    if (durLines[i] != "-1")
                    {
                        htbar.hotDurability.Add(float.Parse(durLines[i]));
                    }
                    else
                    {
                        htbar.hotDurability.Add(0);
                    }
                }
            }

            htbar.UpdateHotbarUI();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    } 

    private void SaveEquipment()
    {
        try
        {
            string saveData = "[items]" + Environment.NewLine;

            for (int i = 0; i < equip.equipmentSlots.Count; i++)
            {
                if (equip.equipmentSlots[i] != null)
                {
                    saveData += equip.equipmentSlots[i].GetComponent<UI_Item>().itemID + Environment.NewLine;
                }
                else
                {
                    saveData += "-1" + Environment.NewLine;
                }
            }
            saveData += "[/items]";

            string encrypted = USecurity.EncryptString(saveData, encryptPassword);
            saveData = "0";

            PlayerPrefs.SetString(saveName + ".eqm", encrypted);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void LoadEquipment() //this will replace the current items
    {
        try
        {
            string loadData = USecurity.DecryptString(PlayerPrefs.GetString(saveName + ".eqm"), encryptPassword);

            string itemsString = USecurity.ExtractBetween(loadData, "[items]", "[/items]", false);

            equip.equipmentSlots.Clear();

            string[] itemLines = itemsString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i=0; i<itemLines.Length; i++)
            {
                if (itemLines[i] != "")
                {
                    if (itemLines[i] !="-1")
                    {
                        equip.equipmentSlots.Add(itemsList.allItems[int.Parse(itemLines[i])]);
                    }
                    else
                    {
                        equip.equipmentSlots.Add(null);
                    }
                }
            }
            equip.UpdateEquipmentUI();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    void LoadItems(string itemsString, string stackString, string durString)
    {
        try
        {
            inv.allSlots.Clear();
            inv.allSlotsStack.Clear();
            inv.allDur.Clear();

            string[] itemsLines = itemsString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i=0; i<itemsLines.Length; i++)
            {
                if (itemsLines[i] != "")
                {
                    if (itemsLines[i] != "-1")
                    {
                        inv.allSlots.Add(itemsList.allItems[int.Parse(itemsLines[i])]);
                    }
                    else
                    {
                        inv.allSlots.Add(null);
                    }
                }
            }

            string[] stackLines = stackString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < stackLines.Length; i++)
            {
                if (stackLines[i] != "")
                {
                    if (stackLines[i] != "-1")
                    {
                        inv.allSlotsStack.Add(int.Parse(stackLines[i]));
                    }
                    else
                    {
                        inv.allSlotsStack.Add(0);
                    }
                }
            }

            string[] durLines = durString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < durLines.Length; i++)
            {
                if (durLines[i] != "" )
                {
                    if (durLines[i] != "-1")
                    {
                        inv.allDur.Add(float.Parse(durLines[i]));
                    }
                    else
                    {
                        inv.allDur.Add(0);
                    }
                }
            }

            inv.UpdateInventoryUI();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

}
