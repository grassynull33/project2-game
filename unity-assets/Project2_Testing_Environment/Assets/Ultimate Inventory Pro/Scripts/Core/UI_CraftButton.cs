using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_CraftButton : MonoBehaviour {

    public GameObject inv;
    

    void Awake()
    {
        inv = GameObject.Find("Ultimate Inventory Pro");
    }

    void Reset(int id)
    {
        inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().afford.Clear();
        foreach (object obj in inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().resourcesID)
        {
            inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().afford.Add(false);
        }
    }

    public void CraftItem(int id)
    {
        Reset(id);
        //check if the item is craftable
        if (inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().isCraftable == true)
        {
            for (int i = 0; i < inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().resourcesID.Length; i++) //for each resource needed
            {
                if (inv.GetComponent<UI_Inventory>().CountItem(inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().resourcesID[i]) >= inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().resourcesAmount[i]) //check if inventory has enough items to craft
                {
                    inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().afford[i] = true;
                }
                else
                {
                    inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().afford[i] = false;
                }
            }

            bool build = true;
            //now checks if the player has all the resources needed
            for (int i = 0; i < inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().afford.Count; i++)
            {
                if (build == true)
                {
                    if (inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().afford[i] == false)
                    {
                        build = false;
                    }
                }
            }

            if (build == true) //player can craft the item
            {
                for (int i = 0; i < inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().resourcesAmount.Length; i++) //for each resource
                {
                    inv.GetComponent<UI_Inventory>().DecreaseItem(inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().resourcesID[i], inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().resourcesAmount[i]); //remove from the inventory what you used for the crafting
                }
                string craftingItem = inv.GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>().name; //get the craftable's item name
                inv.GetComponent<UI_Crafting>().AddItemToQueue(id); //add this item to the crafting queue
            }
        }
        else
            Debug.LogError("This item is not marked as 'Craftable' !");
    }
}
