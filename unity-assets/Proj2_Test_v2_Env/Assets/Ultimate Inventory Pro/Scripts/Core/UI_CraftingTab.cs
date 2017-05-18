using UnityEngine;
using System.Collections;

public class UI_CraftingTab : MonoBehaviour {

    public GameObject invGO;
    public GameObject tab;

    public void ToggleTab()
    {
        //disable all the crafting tabs
        for (int i = 0; i < invGO.GetComponent<UI_Crafting>().allTabs.Count; i++)
        {
            invGO.GetComponent<UI_Crafting>().allTabs[i].SetActive(false);
        }
        tab.SetActive(true);
    }
}
