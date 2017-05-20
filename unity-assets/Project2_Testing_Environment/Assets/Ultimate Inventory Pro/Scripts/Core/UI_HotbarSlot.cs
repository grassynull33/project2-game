using UnityEngine;
using System.Collections;

public class UI_HotbarSlot : MonoBehaviour
{

    public GameObject inventoryGO;
    public GameObject playerEquipGO;
    public int slotID = -1;
    public GameObject weaponManagerGO;

    UI_Hotbar hotbar;
    UI_Inventory inv;
    UI_ItemsList itemList;
    UI_EventManager eventManager;
    UI_PlayerEquipment playerEquipment;

    void Awake()
    {
        inv = inventoryGO.GetComponent<UI_Inventory>();
        itemList = inventoryGO.GetComponent<UI_ItemsList>();
        eventManager = inventoryGO.GetComponent<UI_EventManager>();
        hotbar = inventoryGO.GetComponent<UI_Hotbar>();
        if (playerEquipGO == null)
        {
            playerEquipGO = GameObject.Find("PlayerEquipment");
        }
        if (UI_Inventory.enablePlayerEquipment == true)
        {
            playerEquipment = playerEquipGO.GetComponent<UI_PlayerEquipment>();
        }
        ResetStuff();
    }

    public void SetDropID() //Setting the dropID on the id of the slot where the mouse is hovering.
    {
        UI_Inventory.dropHotID = slotID;
    }
    public void ResetDropID() //Reset the dropID to null (-1)
    {
        UI_Inventory.dropHotID = -1;
    }

    public void ResetStuff()
    {
        UI_Inventory.dropID = -1;
        UI_Inventory.dragItem = -1;
        UI_Inventory.dragStartEquipID = -1;
        UI_Inventory.dropEquipID = -1;
        UI_Inventory.dropConID = -1;
        UI_Inventory.dragStartConID = -1;
        UI_Inventory.dragHotID = -1;
        UI_Inventory.dropHotID = -1;
        UI_Inventory.dragStartHotID = -1;
    }

    public void DragItem()
    {
        if (hotbar.hotbarSlots[slotID] != null)
        { //Check if the slot is not empty.
            UI_Inventory.dragItem = hotbar.hotbarSlots[slotID].GetComponent<UI_Item>().itemID; //Set the drag item id on the id of the item on the current slot.
            UI_Inventory.dragStartHotID = slotID;
            UI_Inventory.dragStartConID = -1;
            UI_Inventory.dragStartEquipID = -1;
            UI_Inventory.dragStartID = -1;
            //  eventManager.OnDragBegin(slotID);
        }
    }

    void Update()
    {
        // Debug.Log("Slot id : " + UI_Inventory.dragStartID);
        if (slotID == UI_Inventory.dragStartHotID)
        {
            if (UI_Inventory.dragItem != -1)//IF you drag an item,
            {
                if (!Input.GetKey(KeyCode.Mouse0)) // and if you don't hold the mouse button then ,
                {
                    DropItem(); //drop the item.
                    ResetStuff();
                }
            }
        }
    }

    void DropItem()
    {
        if (slotID == UI_Inventory.dragStartHotID)
        {

            if (UI_Inventory.dropID == -1 && UI_Inventory.dropHotID == -1 && UI_Inventory.dropEquipID == -1 && UI_Inventory.dropConID == -1)
            { //Drag was ended out of any slot
                if (inv.dropWhenDragOutOfInv == false)
                {
                    //reset drag & drop variables
                    ResetStuff();
                }
                else
                {
                    //send the stack number on the item before dropping it
                    for (int i = 0; i < hotbar.hotbarStack[slotID]; i++)
                    {
                        //drop the item
                        //spawn the item
                        GameObject dropped = (GameObject)Instantiate(hotbar.hotbarSlots[slotID], inv.dropPos.position, hotbar.hotbarSlots[slotID].transform.rotation);

                        if (dropped.GetComponent<UI_Item>().hasDurability == true)
                        {//checks if the item has durability
                            dropped.GetComponent<UI_Item>().itemDurability = hotbar.hotDurability[slotID]; //sync the durability with the spawned item
                        }
                        //Reset the drag system
                    }
                    hotbar.hotbarSlots[slotID] = null; //empty slot
                    hotbar.hotbarStack[slotID] = 0; //reset the stack
                    //reset the drag&drop
                    ResetStuff();
                    inv.UpdateInventoryUI();
                    hotbar.UpdateHotbarUI();
                }
            }

            if (UI_Inventory.dropHotID != -1)
            {
                //the item was dropped on an hotbar slot
                if (UI_Inventory.dropHotID == slotID)
                {
                    //the item was dropped on the same slot
                    //reset the drag & drop
                    ResetStuff();
                }
                else
                {
                    //item dropped on an other slot
                    if (hotbar.hotbarSlots[UI_Inventory.dropHotID] != null)
                    {//the slot is not empty
                        //checks if the slot contains an item of the same type
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        {
                            //checks if the slot has stack space
                            if (hotbar.hotbarStack[UI_Inventory.dropHotID] + hotbar.hotbarStack[UI_Inventory.dragStartHotID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            {//The slot can fit all the new stack
                                //increase the stack and return the drag to null
                                hotbar.hotbarStack[UI_Inventory.dropHotID] += hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                                UI_Inventory.dropHotID = -1;
                                UI_Inventory.dragItem = -1;
                                //Reset item stack
                                hotbar.hotbarSlots[slotID] = null;
                                hotbar.hotbarStack[slotID] = 0;
                            }
                            else
                            {//the slot does not fit all the new stack
                             //find how much we can take
                                int canPlace = hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().maxStack - hotbar.hotbarStack[UI_Inventory.dropHotID];
                                int toPlace = hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                                //max the slot
                                hotbar.hotbarStack[UI_Inventory.dropHotID] = hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().maxStack;
                                //place the rest on the starting slot
                                hotbar.hotbarStack[UI_Inventory.dragStartHotID] = toPlace - canPlace;
                            }
                        }
                        else
                        {
                            //Item is not the same
                            //Drag is canceled
                            //	eventManager.OnDragEnd (slotID, -1);
                            UI_Inventory.dropHotID = -1;
                            UI_Inventory.dragItem = -1;
                        }
                    }
                    else
                    {

                        //the slot was empty
                        //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                        hotbar.hotbarSlots[UI_Inventory.dropHotID] = itemList.allItems[UI_Inventory.dragItem];
                        hotbar.hotbarStack[UI_Inventory.dropHotID] = hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().hasDurability == true)
                        { //checks if the item has durability
                            hotbar.hotDurability[UI_Inventory.dropHotID] = hotbar.hotDurability[UI_Inventory.dragStartHotID]; //move the durability through the slots
                        }
                        UI_Inventory.dropHotID = -1;
                        UI_Inventory.dragItem = -1;
                        //Reset item stack
                        hotbar.hotbarSlots[slotID] = null;
                        hotbar.hotbarStack[slotID] = 0;
                    }
                }
            }

            if (UI_Inventory.dropEquipID != -1)
            {
                //the item was dropped on an equipment slot
                if (playerEquipment.equipSlotsGO[UI_Inventory.dropEquipID].GetComponent<UI_EquipSlot>().equipmentType == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().equipmentCat)
                {//check if the equipment category is the same
                    if (playerEquipment.equipmentSlots[UI_Inventory.dropEquipID] == null)
                    { //checks if the equipment slot is empty
                      //set the dragged item on the slot
                        playerEquipment.equipmentSlots[UI_Inventory.dropEquipID] = itemList.allItems[UI_Inventory.dragItem];
                        //checks if the item was on stack
                        if (hotbar.hotbarStack[slotID] == 1)
                        {
                            //it is not on a stack so we remove it from the slot
                            hotbar.hotbarSlots[slotID] = null;
                            hotbar.hotbarStack[slotID] = 0;
                        }
                        else
                        {
                            //else decrease the stack
                            hotbar.hotbarStack[slotID]--;
                        }
                    }
                    else
                    {
                        //reset drag & drop variables
                        ResetStuff();
                    }
                }
                else
                {
                    //reset drag & drop variables
                    ResetStuff();
                }
            }


            if (UI_Inventory.dropConID != -1)
            {
                //the item was dropped on an container slot
                if (slotID == UI_Inventory.dragStartHotID)
                {
                    //Item dropped on a slot.
                    if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID] != null)
                    {
                        //The slot already contains an item.
                        //Checks whether the slot contains an item of the same type or not.
                        if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        {
                            //Checks if the slot has stack space
                            if (UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] + hotbar.hotbarStack[UI_Inventory.dragStartHotID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            {//The slot can fit all the new stack

                                //Increase the stack and return the drag to null
                                UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] += hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                                UI_Inventory.dropConID = -1;
                                UI_Inventory.dragItem = -1;
                                //Reset item stack
                                hotbar.hotbarSlots[slotID] = null;
                                hotbar.hotbarStack[slotID] = 0;
                            }
                            else
                            {//slot does not fit all the new stack
                                //find how much we can take
                                int canPlace = UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropEquipID].GetComponent<UI_Item>().maxStack - UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID];
                                int toPlace = hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                                //max the slot
                                UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] = UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropEquipID].GetComponent<UI_Item>().maxStack;
                                //place the rest on the starting slot
                                hotbar.hotbarStack[UI_Inventory.dragStartHotID] = toPlace - canPlace;
                            }
                        }
                        else
                        {
                            //reset drag & drop variables
                            ResetStuff();
                        }
                    }
                    else
                    {
                        //If the slot is empty
                        //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                        UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID] = itemList.allItems[UI_Inventory.dragItem];
                        UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] = hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                        if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().hasDurability == true)
                        { //checks if the item has durability
                            UI_Inventory.openedContainer.GetComponent<UI_Container>().allDur[UI_Inventory.dropConID] = hotbar.hotDurability[UI_Inventory.dragStartHotID]; //move the durability through the slots
                        }
                        UI_Inventory.dropConID = -1;
                        UI_Inventory.dragItem = -1;
                        //Reset item stack
                        hotbar.hotbarSlots[slotID] = null;
                        hotbar.hotbarStack[slotID] = 0;
                    }
                }
            }

            if (UI_Inventory.dropID != -1)
            {
                //the item was dropped on an inventory slot
                if (slotID == UI_Inventory.dragStartHotID)
                {
                    //Item dropped on a slot.
                    if (inv.allSlots[UI_Inventory.dropID] != null)
                    {
                        //The slot already contains an item.
                        //Checks whether the slot contains an item of the same type or not.
                        if (inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        {
                            //Checks if the slot has stack space
                            if (inv.allSlotsStack[UI_Inventory.dropID] + hotbar.hotbarStack[UI_Inventory.dragStartHotID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            {//The slot can fit all the new stack
                                //Increase the stack and return the drag to null
                                inv.allSlotsStack[UI_Inventory.dropID] += hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                                //Reset item stack
                                hotbar.hotbarSlots[slotID] = null;
                                hotbar.hotbarStack[slotID] = 0;
                            }
                            else
                            {//slot does not fit all the new stack
                                //find how much we can take
                                int canPlace = inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().maxStack - inv.allSlotsStack[UI_Inventory.dropID];
                                int toPlace = hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                                //max the slot
                                inv.allSlotsStack[UI_Inventory.dropID] = inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().maxStack;
                                //place the rest on the starting slot
                                hotbar.hotbarStack[UI_Inventory.dragStartHotID] = toPlace - canPlace;
                            }
                        }
                        else
                        {
                            //reset drag & drop variables
                            ResetStuff();
                        }
                    }
                    else
                    {
                        //If the slot is empty
                        //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                        inv.allSlots[UI_Inventory.dropID] = itemList.allItems[UI_Inventory.dragItem];
                        inv.allSlotsStack[UI_Inventory.dropID] = hotbar.hotbarStack[UI_Inventory.dragStartHotID];
                        if (inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().hasDurability == true)
                        { //checks if the item has durability
                            inv.allDur[UI_Inventory.dropID] = hotbar.hotDurability[slotID]; //move the durability through the slots
                        }
                        //Reset item stack
                        hotbar.hotbarSlots[slotID] = null;
                        hotbar.hotbarStack[slotID] = 0;
                    }
                }
            }

        }
        try
        {
            UI_Inventory.openedContainer.GetComponent<UI_Container>().UpdateContainerUI(); //refresh the opened container UI
        }
        catch { }
        if (UI_Inventory.enablePlayerEquipment == true)
        {
            playerEquipment.UpdateEquipmentUI(); //refresh the equipment ui
        }
        inv.UpdateInventoryUI();
        hotbar.UpdateHotbarUI();
    }

    public void Click()
    {

    }
}
