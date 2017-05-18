using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Hotbar : MonoBehaviour
{

    public List<GameObject> hotbarSlots = new List<GameObject>();
    public List<float> hotDurability = new List<float>();
    public List<int> hotbarStack = new List<int>();

    public GameObject slotsHolder;
    public List<GameObject> hotSlotsGO = new List<GameObject>();
    public bool allowHotKeys = true;
    public bool autoAdjustSlots = true;
    public Sprite defaultEmpty;

    void Awake()
    {
        if (autoAdjustSlots) //automatically set up and assign the values
        {
            hotbarSlots.Clear();
            hotDurability.Clear();
            hotbarStack.Clear();
            hotSlotsGO.Clear();
            for (int i = 0; i < slotsHolder.transform.childCount; i++)
            {
                hotSlotsGO.Add(slotsHolder.transform.GetChild(i).gameObject);
                slotsHolder.transform.GetChild(i).gameObject.GetComponent<UI_HotbarSlot>().slotID = i;
                hotbarSlots.Add(null);
                hotbarStack.Add(0);
                hotDurability.Add(0);
            }
        }
    }

    public void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            //Checks whether the slot contains an item
            if (hotbarSlots[i] != null) //If yes then it does change the sprite & the stack text.
            {
                hotSlotsGO[i].transform.FindChild("Image").GetComponent<Image>().sprite = hotbarSlots[i].GetComponent<UI_Item>().itemPreview;

                hotSlotsGO[i].transform.FindChild("Text").GetComponent<Text>().text = hotbarStack[i].ToString();
            }
            else//if no it will set the stack to 0 and the sprite to the default.
            {
                hotSlotsGO[i].transform.FindChild("Image").GetComponent<Image>().sprite = defaultEmpty;
                hotSlotsGO[i].transform.FindChild("Text").GetComponent<Text>().text = "0";
            }
        }
    }

    void Update()
    {
        if (allowHotKeys)
            HandleControls();
    }

    void HandleControls()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActivateItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ActivateItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ActivateItem(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ActivateItem(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ActivateItem(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ActivateItem(8);
        }
    }

    public void ActivateItem(int index)
    {
        if (hotbarSlots[index] != null)
        {
            //if the slot is not empty
            if (hotbarSlots[index].GetComponent<UI_Item>().itemCat == "Normal")
            {
                GetComponent<UI_ItemActivate>().ActivateItem(hotbarSlots[index].GetComponent<UI_Item>().itemID, "Normal",index);
            }
            else if (hotbarSlots[index].GetComponent<UI_Item>().itemCat == "Consumable")
            {
                GetComponent<UI_ItemActivate>().ActivateItem(hotbarSlots[index].GetComponent<UI_Item>().itemID, "Consumable");
                string[] consumeEffects = hotbarSlots[index].GetComponent<UI_Item>().statsToEditOnConsume;

                for (int i = 0; i < consumeEffects.Length; i++)
                {
                    if (consumeEffects[i] == "DrinkItem")
                    {
                        Debug.Log("You are drunk now!");
                    }
                }
            }

            if (hotbarSlots[index].GetComponent<UI_Item>().hasDurability == true)
            {
                if (hotbarSlots[index].GetComponent<UI_Item>().useDurability > 0)
                { // Check if item has durability

                    if (hotDurability[index] - hotbarSlots[index].GetComponent<UI_Item>().useDurability > 0) //checks if the item has enough durability
                    {
                       hotDurability[index] -= hotbarSlots[index].GetComponent<UI_Item>().useDurability; //decrease the durability
                    }
                    else
                    {
                        //destroy the item
                        //Checks if the item was on a stack
                        if (hotDurability[index] == 1)
                        {
                            //if its not on stack it removes the item from the slot
                            hotDurability[index] = 0;
                            hotbarSlots[index] = null;
                        }
                        else
                        {
                            //else if it is on stack it just decrease the stack
                            hotDurability[index]--;
                        }
                    }
                }
            }
            if (hotbarSlots[index].GetComponent<UI_Item>().destroyOnUse == true)
            {
                //Checks if the item was on a stack
                if (hotbarStack[index] == 1)
                {
                    //if its not on stack it removes the item from the slot
                    hotbarStack[index] = 0;
                    hotbarSlots[index] = null;
                }
                else
                {
                    //else if it is on stack it just decrease the stack
                    hotbarStack[index]--;
                }
                //	Debug.Log ("Item destroyed");

            }
        }

        UpdateHotbarUI();
    }

}
