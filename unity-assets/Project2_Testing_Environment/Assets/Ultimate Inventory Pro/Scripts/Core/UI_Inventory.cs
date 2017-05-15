/*
Ultimate Inventory 5 (2016) by GreekStudios
v.5.0
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public static string currentVersion = "5.2";
    public GameObject player;
    public GameObject inventory;
    public GameObject slotGOHolder;
    public bool autoAdjustSlotsID = true, autoAddSlots = true;
    public KeyCode toggleInv = KeyCode.I, pickupKey = KeyCode.E;
    public bool raycastOnMouse = false;
    public float pickupRange = 100f;
    public List<GameObject> allSlots = new List<GameObject>();
    public List<int> allSlotsStack = new List<int>();
    public List<GameObject> slotGOs = new List<GameObject>();
    public List<float> allDur = new List<float>();
    public bool allowDragDrop = true;
    public bool allowContextMenu = true;
    public bool allowPreviewWin = true;
    public bool hideCursor = true;
    UI_ItemsList itemsList;
    public static int dragItem = -1;
    public static int dropID = -1;
    public static int dragStartID = -1;
    public static int dragStartEquipID = -1;
    public static int dragStartConID = -1;
    public static int dropEquipID = -1;
    public static int dropConID = -1;
    public static int dragHotID = -1;
    public static int dropHotID = -1;
    public static int dragStartHotID = -1;
    public UI_EventManager eventManager;
    public Camera cam;
    public Sprite defaultEmpty;
    public GameObject contextMenu;
    public bool disableComponentOnToggle = true;
    public GameObject[] components;
    public Transform dropPos;
    public static int selectedSlot = -1;
    public bool dropWhenDragOutOfInv = true;
    public float dragIconFactor = 0.2f;
    public Color fullDur = Color.green, halfDur = Color.yellow, lowDur = Color.red;
    public GameObject playerEquipmentGO;
    public static bool enablePlayerEquipment = true;
    //item preview
    public Text itemName, itemPreview;
    public Image itemIcon;
    public GameObject previewPanel;
    public GameObject durBar;
    public GameObject ecButton;
    public static bool isOpen = false;
    public KeyCode containerToggleKey = KeyCode.E;
    public float containerOpenRange = 5f;
    Vector3 pos;
    public bool countWeight = true;
    public float totalWeight = 20;
    public float speedFactor = 0.2f;
    public float currentWeight = 0;
    public static GameObject openedContainer;
    //not in editor yet
    public Transform buildingPos;
    public GameObject[] buildingProps;
    public int[] buildingPropsIds;
    public AudioClip pickupItemSound,fullSound;
    public float CountWeight()
    {
        float result = 0;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] != null)
            {
                result += (allSlots[i].GetComponent<UI_Item>().itemWeight * allSlotsStack[i]);
            }
        }
        return result;
    }

    public int CountItem(int index) //this function returns (int) the amount of the given item (index)
    {

        int result = 0; //set the result to 0
        for (int i = 0; i < allSlots.Count; i++) //for each slot
        {
            if (allSlots[i] != null)
            {
                if (allSlots[i].GetComponent<UI_Item>().itemID == index) //if the slot contains the given item
                {
                    result += allSlotsStack[i]; //add the amount of the item on the result
                }
            }
        }
        return result; //return the result
    }

    public void DecreaseItem(int index, int amount)
    {
        int left = amount;
        bool found = false;
        for (int i = 0; i < allSlots.Count; i++) //for each slot
        {
            if (allSlots[i] != null)
            {
                if (found == false)
                {
                    if (allSlots[i].GetComponent<UI_Item>().itemID == index) //if the slot contains the given item
                    {
                        if (allSlotsStack[i] > left)
                        {
                            int tmp = left;
                            left = 0;
                            allSlotsStack[i] -= tmp;
                            found = true;
                        }
                        else
                        {
                            int tmp = allSlotsStack[i];
                            allSlotsStack[i] = 0;
                            allSlots[i] = null;
                            left -= tmp;
                        }
                    }
                }
            }
        }
        UpdateInventoryUI();
    }

    //initializing variables, modify at your own risk
    void Awake()
    {
        itemsList = GetComponent<UI_ItemsList>();
        if (autoAddSlots == true)
        {
            slotGOs.Clear();
            allSlots.Clear();
            allSlotsStack.Clear();
            allDur.Clear();
            for (int i = 0; i < slotGOHolder.transform.childCount; i++)
            {
                slotGOs.Add(slotGOHolder.transform.GetChild(i).gameObject);
                allSlots.Add(null);
                allSlotsStack.Add(0);
                allDur.Add(0);
            }
        }
        if (autoAdjustSlotsID == true)
        {
            for (int i = 0; i < slotGOHolder.transform.childCount; i++)
            {
                slotGOHolder.transform.GetChild(i).GetComponent<UI_Slot>().slotID = i;
                slotGOHolder.transform.GetChild(i).GetComponent<UI_Slot>().contextMenu = contextMenu;
            }
        }
    }

    public void ToggleIn()
    {
        inventory.SetActive(!inventory.activeSelf);
        if (hideCursor == true)
        {
            if (inventory.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        if (disableComponentOnToggle == true) //Replace the component with what you like to disable when you toggle inventory
        {
            components[0].GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = !inventory.activeSelf;
            components[1].GetComponent<UnityStandardAssets.ImageEffects.Blur>().enabled = inventory.activeSelf;
        }
        isOpen = inventory.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleInv))
        {
            ToggleIn();
        }


        if (Input.GetKeyDown(pickupKey))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, pickupRange))
            {
                if (hit.collider.GetComponent<UI_Item>() != null)
                {
                    PickupItem(hit.collider.gameObject);
                }
            }

        }

        if (Input.GetKeyDown(containerToggleKey))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, containerOpenRange))
            {
                if (hit.collider.GetComponent<UI_Container>() != null)
                {
                    hit.collider.GetComponent<UI_Container>().ToggleContainer(!hit.collider.GetComponent<UI_Container>().isOpen);
                }
            }

        }

        if (allowDragDrop == true)
        {
            if (dragItem != -1)
            {
                pos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            }
        }
    }

    void OnGUI()
    {
        if (allowDragDrop == true)
        {
            if (dragItem != -1)
            {
                GUI.DrawTexture(new Rect(pos.x, pos.y, itemsList.allItems[dragItem].GetComponent<UI_Item>().itemPreview.texture.width * dragIconFactor, itemsList.allItems[dragItem].GetComponent<UI_Item>().itemPreview.texture.height * dragIconFactor), itemsList.allItems[dragItem].GetComponent<UI_Item>().itemPreview.texture);
            }
        }
    }


    public void PickupItem(GameObject item)
    {
        bool found = false;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (found == false) //Checks whether the item has been placed yet or not.
            {
                if (allSlots[i] != null)
                {
                    //Check if there is already the same item on your inventory, if yes then it will be stacked.
                    if (allSlots[i].GetComponent<UI_Item>().itemID == item.GetComponent<UI_Item>().itemID && allSlotsStack[i] < allSlots[i].GetComponent<UI_Item>().maxStack)
                    {
                        found = true;
                        allSlotsStack[i]++; //Increase the slot stack with the new item.
                                            //Play Sound CODE FOR SOUND
                        if (item.GetComponent<UI_Item>().hasDurability == true) //checks if the item has durability
                        {
                            allDur[i] = item.GetComponent<UI_Item>().itemDurability; //assign the durability
                        }
                        Destroy(item); //Destroy the world model.
                        eventManager.PickupItemEvent(itemsList.allItems[item.GetComponent<UI_Item>().itemID], i, true); //Send information about the pickup on the event manager.
                    }
                }
                //The item does not exist so it would be added into a new slot.
                else
                {
                    found = true;
                    allSlots[i] = itemsList.allItems[item.GetComponent<UI_Item>().itemID]; //Set the null slot with the picked item.
                    allSlotsStack[i]++;//Increase the slot stack with the new item.
                                       //Play Sound
                    if (item.GetComponent<UI_Item>().hasDurability == true) //checks if the item has durability
                    {
                        allDur[i] = item.GetComponent<UI_Item>().itemDurability; //assign the durability
                    }
                    Destroy(item); //Destroy the world model.
                    eventManager.PickupItemEvent(itemsList.allItems[item.GetComponent<UI_Item>().itemID], i, false); //Send information about the pickup on the event manager.
                }
            }
        }

        if (found == false) //Inventory is full
        {
           if (fullSound != null)
                GetComponent<AudioSource>().PlayOneShot(fullSound);
            // eventManager.InventoryFullEvent(itemsList.allItems[item.GetComponent<UI_Item>().itemID]);
        }
        else
        {
            if (pickupItemSound != null)
                GetComponent<AudioSource>().PlayOneShot(pickupItemSound);
        }
        UpdateInventoryUI();
    }

    //Load icons of items on each slot and refresh the stack label.
    public void UpdateInventoryUI()
    {
        eventManager.InventoryRefreshBegin();
        for (int i = 0; i < slotGOs.Count; i++)
        {
            //Checks whether the slot contains an item
            if (allSlots[i] != null) //If yes then it does change the sprite & the stack text.
            {
                slotGOs[i].transform.FindChild("Image").GetComponent<Image>().sprite = allSlots[i].GetComponent<UI_Item>().itemPreview;

                slotGOs[i].transform.FindChild("Text").GetComponent<Text>().text = allSlotsStack[i].ToString();
            }
            else//if no it will set the stack to 0 and the sprite to the default.
            {
                slotGOs[i].transform.FindChild("Image").GetComponent<Image>().sprite = defaultEmpty;
                slotGOs[i].transform.FindChild("Text").GetComponent<Text>().text = "0";
            }
        }
        ActivateItemPreview();
        PlayerAndWeightFunction();
        eventManager.InventoryRefreshEnd();
    }
    bool activated = false;
    public void PlayerAndWeightFunction()//this function determind the player's speed depending on inventory's weight
    {
        if (countWeight == true)
        {
            currentWeight = CountWeight();

            if (currentWeight >= totalWeight)
            {
                if (activated == false)
                {
                    //modify your code here if you use an other player controller [#12872]
                    player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeSpeed(speedFactor);
                    activated = true;
                }
            }
            else
            {
                if (activated == true)
                {
                    player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeSpeed(1 / speedFactor);
                    activated = false;
                }
            }

        }
    }


	public void ConsumeEquipButton()
	{
		ContextMenuEquip (selectedSlot);
	}

    public void ContextMenuEquip(int itemIDForced = -1) //Use item Code
    {
        try
        {
            int itemID = -100;
            if (itemIDForced == -1)
            {
                itemID = contextMenu.GetComponent<UI_ContextButton>().slotID;
            }
            else
            {
                itemID = itemIDForced;
            }

            //Debug.Log ("Item : " + allSlots [itemID].name);
            if (allSlots[itemID].GetComponent<UI_Item>().itemCat == "Normal")
            {
                GetComponent<UI_ItemActivate>().ActivateItem(allSlots[itemID].GetComponent<UI_Item>().itemID, "Normal");
            }
            else if (allSlots[itemID].GetComponent<UI_Item>().itemCat == "Equipment")
            {
                //   Debug.Log("Equipment item has been equipped. [code here]"); //Code Here
                //GameObject[] itemsToEquip = allSlots [itemID].GetComponent<UI_Item> ().objectsToEquip; //Gameobjects you want to activate
                /*
             * this code activates all the objects
            for (int i=0; i<itemsToEquip.Length; i++)
            {
                itemsToEquip[i].SetActive(true);
            }
            */
                for (int i = 0; i < playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipSlotsGO.Count; i++) //for each equipment slot
                {
                    if (playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipSlotsGO[i].GetComponent<UI_EquipSlot>().equipmentType == allSlots[selectedSlot].GetComponent<UI_Item>().equipmentCat) //if the item equipment type match the equipment type of the slot
                    {
                        if (playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i] == null) //and if the slot is empty
                        {
                            //equip the item.
                            playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i] = allSlots[selectedSlot];
                            //remove the item from the slot
                            //Checks if the item was on a stack
                            if (allSlotsStack[selectedSlot] == 1)
                            {
                                //if its not on stack it removes the item from the slot
                                allSlotsStack[selectedSlot] = 0;
                                allSlots[selectedSlot] = null;
                            }
                            else
                            {
                                //else if it is on stack it just decrease the stack
                                allSlotsStack[selectedSlot]--;
                            }
                        }
                        else //equipment slot has an other item
                        {
                            //if the slot on the inventory does not have stack we will swap the items
                            if (allSlotsStack[selectedSlot] == 1)
                            {
                                //store the equipment item temporarily
                                GameObject temp = playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i];
                                //equip the item to the equipment slot
                                playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i] = allSlots[selectedSlot];
                                //equip the temp item to the inventory slot
                                allSlots[selectedSlot] = temp;
                                allSlotsStack[selectedSlot] = 1; //this is not neccessary
                            }
                            else //the slot had stack
                            {
                                //check if inventory has empty space
                                bool found = false;
                                for (int y = 0; y < allSlots.Count; y++)
                                {
                                    if (found == false) //Checks whether the item has been placed yet or not.
                                    {
                                        if (allSlots[y] != null)
                                        {
                                            //Check if there is already the same item on your inventory, if yes then it will be stacked.
                                            if (allSlots[y].GetComponent<UI_Item>().itemID == playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i].GetComponent<UI_Item>().itemID && allSlotsStack[y] < playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i].GetComponent<UI_Item>().maxStack)
                                            {
                                                found = true;
                                                allSlotsStack[y]++; //Increase the slot stack with the new item.
                                            }
                                        }
                                        //The item does not exist so it would be added into a new slot.
                                        else
                                        {
                                            found = true;
                                            allSlots[y] = itemsList.allItems[playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i].GetComponent<UI_Item>().itemID]; //Set the null slot with the picked item.
                                            allSlotsStack[y]++;//Increase the slot stack with the new item.
                                            //Play Sound
                                        }
                                    }
                                }

                                if (found == true) //item was placed into the inventory
                                {
                                    playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i] = allSlots[selectedSlot];
                                }
                                else //inventory is full
                                {
                                    //item will be dropped
                                    Instantiate(playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i], dropPos.position, playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i].transform.rotation);
                                    //place the new equipment
                                    playerEquipmentGO.GetComponent<UI_PlayerEquipment>().equipmentSlots[i] = allSlots[selectedSlot];
                                }
                                allSlotsStack[selectedSlot]--; //decrease the stack of the equipped item
                            }
                        }
                        //refresh the ui
                        UpdateInventoryUI();
                        playerEquipmentGO.GetComponent<UI_PlayerEquipment>().UpdateEquipmentUI();
                    }
                }
            }
            else if (allSlots[itemID].GetComponent<UI_Item>().itemCat == "Consumable")
            {
                GetComponent<UI_ItemActivate>().ActivateItem(allSlots[itemID].GetComponent<UI_Item>().itemID, "Consumable"); //#29549
                string[] consumeEffects = allSlots[itemID].GetComponent<UI_Item>().statsToEditOnConsume;

                for (int i = 0; i < consumeEffects.Length; i++)
                {
                    if (consumeEffects[i] == "DrinkItem")
                    {
                        Debug.Log("You are drunk now!");
                    }
                }

                if (allSlots[itemID].GetComponent<UI_Item>().isBuilding == true)
                {
                    for (int i = 0; i < buildingProps.Length; i++)
                    {
                        if (allSlots[itemID].GetComponent<UI_Item>().itemID == buildingPropsIds[i])
                        {
                            GameObject newObj = (GameObject)Instantiate(buildingProps[i], buildingPos.position, buildingPos.rotation);
                            newObj.transform.SetParent(buildingPos.transform, true);
   
                        }
                    }
                }
            }

            if (allSlots[itemID].GetComponent<UI_Item>().hasDurability == true)
            {
                if (allSlots[itemID].GetComponent<UI_Item>().useDurability > 0)
                { // Check if item has durability

                    if (allDur[itemID] - allSlots[itemID].GetComponent<UI_Item>().useDurability > 0) //checks if the item has enough durability
                    {
                        allDur[itemID] -= allSlots[itemID].GetComponent<UI_Item>().useDurability; //decrease the durability
                    }
                    else
                    {
                        //destroy the item
                        //Checks if the item was on a stack
                        if (allSlotsStack[itemID] == 1)
                        {
                            //if its not on stack it removes the item from the slot
                            allSlotsStack[itemID] = 0;
                            allSlots[itemID] = null;
                        }
                        else
                        {
                            //else if it is on stack it just decrease the stack
                            allSlotsStack[itemID]--;
                        }
                    }
                }
            }
            if (allSlots[itemID].GetComponent<UI_Item>().destroyOnUse == true)
            {
                //Checks if the item was on a stack
                if (allSlotsStack[itemID] == 1)
                {
                    //if its not on stack it removes the item from the slot
                    allSlotsStack[itemID] = 0;
                    allSlots[itemID] = null;
                }
                else
                {
                    //else if it is on stack it just decrease the stack
                    allSlotsStack[itemID]--;
                }
                //	Debug.Log ("Item destroyed");

            }
        }
        catch
        {
        }
        contextMenu.SetActive(false);
        UpdateInventoryUI();
    }

    public void ContextMenuDrop() //Drop item Code
    {
        try
        {
            int itemID = contextMenu.GetComponent<UI_ContextButton>().slotID;
            //Debug.Log("Item : " + allSlots[itemID].name);

            //Spawn the item
            GameObject dropped = (GameObject)Instantiate(allSlots[itemID], dropPos.position, allSlots[itemID].transform.rotation);

            if (dropped.GetComponent<UI_Item>().hasDurability == true) //checks if the item has durability
            {
                dropped.GetComponent<UI_Item>().itemDurability = allDur[selectedSlot];//sync the durability with the spawned item
            }
            //Checks if the item was on a stack
            if (allSlotsStack[itemID] == 1)
            {
                //if its not on stack it removes the item from the slot
                allSlotsStack[itemID] = 0;
                allSlots[itemID] = null;
            }
            else
            {
                //else if it is on stack it just decrease the stack
                allSlotsStack[itemID]--;
            }

        }
        catch { }
        UpdateInventoryUI();
        contextMenu.SetActive(false);
    }

    public void ContextMenuSplit() //Split stack Code
    {
        try
        {
            int itemID = contextMenu.GetComponent<UI_ContextButton>().slotID;

            if (allSlotsStack[itemID] > 1) //Checks if the slot contains more than 1 item(stack)
            {
                int stackNumber = allSlotsStack[itemID];
                int newStack = 0;
                int finalStack = 0;
                if (stackNumber % 2 == 0) //Checks if the number is even
                {
                    newStack = stackNumber / 2;
                    finalStack = stackNumber - newStack;
                }
                else //or if is odd
                {
                    newStack = (stackNumber + 1) / 2;
                    finalStack = stackNumber - newStack + 1;
                    newStack -= 1;
                }
                //after splitting the stacks on numbers we must do that on the inventory

                bool found = false; //this will help for stopping the search when we find our slot.

                for (int i = 0; i < allSlots.Count; i++) //For each slot
                {
                    if (found == false) //if we haven't found our slot yet
                    {
                        if (allSlots[i] == null) //if we find the slot empty
                        {
                            //then just place our stack of items on this slot
                            found = true;
                            allSlots[i] = allSlots[itemID];
                            allSlotsStack[i] = newStack;
                            //and make sure we remove the placed stack from the starting slot
                            allSlotsStack[itemID] = finalStack;
                            //then just update our Inventory
                            UpdateInventoryUI();
                        }
                        else //If we don't find any empty slots [Bug Fix Needed !]
                        {/*
                        //we will try to find an other slot with the same item

                        if (allSlots[i].GetComponent<UI_Item>().itemID == allSlots[itemID].GetComponent<UI_Item>().itemID)
                        {
                            //we found a slot with the same item

                            //now we check if the slot has enough space for all the quantity
                            if (allSlotsStack[i] + newStack <= allSlots[i].GetComponent<UI_Item>().maxStack)
                            {
                                Debug.Log("Had");
                                //it has enough space so we just place it here
                                found = true;
                                allSlotsStack[i] += newStack;
                                //and make sure we remove the placed stack from the starting slot
                                allSlotsStack[itemID] = finalStack;
                                //then just update our Inventory
                                UpdateInventoryUI();
                            }
                            else
                            {
                                found = true;
                                //the slot didn't have enough space, so we will place as much as it fits.
                                int fits = allSlots[i].GetComponent<UI_Item>().maxStack;
                                //the slot we found is now full
                                allSlotsStack[i] = fits;
                                //and we will return back to the starting slot what left
                                int left = (newStack + allSlotsStack[i]) - fits;
                                allSlotsStack[itemID] = left + finalStack;
                                //then just update our Inventory
                                UpdateInventoryUI();
                            }
                            //No slots found, all items remain on the starting slot
                        }
                    
                      * */
                        }
                    }
                }

            }
        }
        catch { }
        UpdateInventoryUI();
        contextMenu.SetActive(false);
    }

    public void ActivateItemPreview()
    {
        if (selectedSlot != -1 && allSlots[selectedSlot] != null)
        { //checks if the player has an item selected 
            previewPanel.SetActive(true); //activate the preview panel
            //Checks the selected item's category
            if (allSlots[selectedSlot].GetComponent<UI_Item>().itemCat == "Equipment")
            {
                //it's equipment, so the equip button is activated
                ecButton.SetActive(true);
                ecButton.transform.GetChild(0).GetComponent<Text>().text = "Equip";
            }
            else if (allSlots[selectedSlot].GetComponent<UI_Item>().itemCat == "Consumable")
            {
                //it's Consumable so the Consume button is activated
                ecButton.SetActive(true);
                ecButton.transform.GetChild(0).GetComponent<Text>().text = "Consume";
            }
            else
            {
                //The action (Consume/Equip) button is disabled
                ecButton.SetActive(false);
            }
            itemName.text = allSlots[selectedSlot].GetComponent<UI_Item>().name; //load the item's name
            itemPreview.text = allSlots[selectedSlot].GetComponent<UI_Item>().desc; //load the item's description
            itemIcon.sprite = allSlots[selectedSlot].GetComponent<UI_Item>().itemPreview; //load the item's preview icon
            if (allSlots[selectedSlot].GetComponent<UI_Item>().hasDurability == true)
            {
                durBar.SetActive(true); //activate the durability bar
                durBar.GetComponent<Transform>().localScale = new Vector3(1, allDur[selectedSlot] / 100, 1); //set the appropriate size
                if (allDur[selectedSlot] > 50)
                {
                    durBar.GetComponent<Image>().color = fullDur;
                }
                else if (allDur[selectedSlot] > 20)
                {
                    durBar.GetComponent<Image>().color = halfDur;
                }
                else
                {
                    durBar.GetComponent<Image>().color = lowDur;
                }

            }
            else
            {
                durBar.SetActive(false);
            }
        }
        else
        {
            selectedSlot = -1;
            previewPanel.SetActive(false);
        }
    }

    public void DropItemButton()
    {
        try
        {
            //Spawn the item
            GameObject dropped = (GameObject)Instantiate(allSlots[selectedSlot], dropPos.position, allSlots[selectedSlot].transform.rotation);

            if (dropped.GetComponent<UI_Item>().hasDurability == true) //checks if the item has durability
            {
                dropped.GetComponent<UI_Item>().itemDurability = allDur[selectedSlot];//sync the durability with the spawned item
            }

            //Checks if the item was on a stack
            if (allSlotsStack[selectedSlot] == 1)
            {
                //if its not on stack it removes the item from the slot
                allSlotsStack[selectedSlot] = 0;
                allSlots[selectedSlot] = null;
            }
            else
            {
                //else if it is on stack it just decrease the stack
                allSlotsStack[selectedSlot]--;
            }
            UpdateInventoryUI();
        }
        catch { }
    }
}