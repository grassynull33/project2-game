using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MyExtensions;
using System.Collections.Generic;

public class UI_CraftingQueueSystem : MonoBehaviour {

    public GameObject parent;
    public GameObject inv;
    public GameObject itemGO;
    public GameObject contextMenu;
    public List<GameObject> elements = new List<GameObject>();
    UI_Crafting craft;
    UI_ItemsList items;

    void Awake()
    {
        //assign crafting and itemslist scripts
        craft = inv.GetComponent<UI_Crafting>();
        items = inv.GetComponent<UI_ItemsList>();
    }
    
    public void DisplayUI()
    {
        try
        {
            //clean items
            for (int i = 0; i < elements.Count; i++)
            {
                GameObject.Destroy(elements[i]);
            }
            elements.Clear();
            //reset size
            parent.GetComponent<RectTransform>().offsetMin.Set(parent.GetComponent<RectTransform>().offsetMin.x, 333.5f);
            //for each item in queue
            for (int i = 0; i < craft.queue.Count; i++)
            {
                //create a new UI item
                GameObject newUI = (GameObject)Instantiate(itemGO, Vector3.zero, Quaternion.identity);
                //parent the new UI item to the scroll view
                newUI.transform.SetParent(parent.transform);
                //set icon
                newUI.GetComponent<UI_CraftingQueue>().icon.sprite = craft.queue[i].GetComponent<UI_Item>().itemPreview;
                //set title
                newUI.GetComponent<UI_CraftingQueue>().title.text = craft.queue[i].GetComponent<UI_Item>().name;
                //add item to elements list
                elements.Add(newUI);
                //increase size
                parent.GetComponent<RectTransform>().SetSize(new Vector2(parent.GetComponent<RectTransform>().GetSize().x, 330 + (i * 34)));
            }
        }
        catch { }
    }

    public void RemoveItem(string status)
    {
        try
        {
            if (status == "first")
            {
                //return the resources back
                for (int i = 0; i < craft.queue[0].GetComponent<UI_Item>().resourcesID.Length; i++) //for each resource
                {
                    for (int x = 0; x < craft.queue[0].GetComponent<UI_Item>().resourcesAmount[i]; x++) //for the amount of each resource
                    {
                        GameObject craftedGO = (GameObject)Instantiate(inv.GetComponent<UI_ItemsList>().allItems[craft.queue[0].GetComponent<UI_Item>().resourcesID[i]], inv.GetComponent<UI_Inventory>().dropPos.position, inv.GetComponent<UI_ItemsList>().allItems[craft.queue[0].GetComponent<UI_Item>().resourcesID[i]].transform.rotation); //spawn the item
                        inv.GetComponent<UI_Inventory>().PickupItem(craftedGO); //try to pick it up
                    }
                }
                craft.FailSafe(true);
                craft.queue.RemoveAt(0); //remove the first item on queue
            }
            else if (status == "last")
            {
                //return the resources back
                for (int i = 0; i < craft.queue[craft.queue.Count - 1].GetComponent<UI_Item>().resourcesID.Length; i++) //for each resource
                {
                    for (int x = 0; x < craft.queue[craft.queue.Count - 1].GetComponent<UI_Item>().resourcesAmount[i]; x++) //for the amount of each resource
                    {
                        GameObject craftedGO = (GameObject)Instantiate(inv.GetComponent<UI_ItemsList>().allItems[craft.queue[craft.queue.Count - 1].GetComponent<UI_Item>().resourcesID[i]], inv.GetComponent<UI_Inventory>().dropPos.position, inv.GetComponent<UI_ItemsList>().allItems[craft.queue[craft.queue.Count - 1].GetComponent<UI_Item>().resourcesID[i]].transform.rotation); //spawn the item
                        inv.GetComponent<UI_Inventory>().PickupItem(craftedGO); //try to pick it up
                    }
                }
                craft.queue.RemoveAt(craft.queue.Count - 1);
            }
            else
            {
                for (int y = 0; y < craft.queue.Count; y++) // for each item on queue
                {
                    //return the resources back
                    for (int i = 0; i < craft.queue[y].GetComponent<UI_Item>().resourcesID.Length; i++) //for each resource
                    {
                        for (int x = 0; x < craft.queue[y].GetComponent<UI_Item>().resourcesAmount[i]; x++) //for the amount of each resource
                        {
                            GameObject craftedGO = (GameObject)Instantiate(inv.GetComponent<UI_ItemsList>().allItems[craft.queue[y].GetComponent<UI_Item>().resourcesID[i]], inv.GetComponent<UI_Inventory>().dropPos.position, inv.GetComponent<UI_ItemsList>().allItems[craft.queue[y].GetComponent<UI_Item>().resourcesID[i]].transform.rotation); //spawn the item
                            inv.GetComponent<UI_Inventory>().PickupItem(craftedGO); //try to pick it up
                        }
                    }
                }
                craft.queue.Clear();
            }
            DisplayUI();
        }
        catch { }
    }

    public void DisableContex()
    {
        contextMenu.SetActive(false);
    }
}
