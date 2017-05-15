using UnityEngine;
using System.Collections;

public class UI_ContainerSlot : MonoBehaviour {

    public GameObject inventoryGO;
    public GameObject playerEquipGO;
    public GameObject container;
    public int slotID = 0;


    UI_Hotbar hotbar;
    UI_Inventory inv;
    UI_ItemsList itemList;
    UI_EventManager eventManager;
    UI_PlayerEquipment playerEquipment;
    UI_Container conScript;

    void Awake()
    {
        inventoryGO = GameObject.Find("Ultimate Inventory Pro");
        inv = inventoryGO.GetComponent<UI_Inventory>();
        itemList = inventoryGO.GetComponent<UI_ItemsList>();
        eventManager = inventoryGO.GetComponent<UI_EventManager>();
        hotbar = inventoryGO.GetComponent<UI_Hotbar>();
        if (playerEquipGO == null)
        {
            playerEquipGO = GameObject.Find("PlayerEquipment");
        }
        playerEquipment = playerEquipGO.GetComponent<UI_PlayerEquipment>();
        conScript = container.GetComponent<UI_Container>();
    }

    void Update()
    {
        if (slotID == UI_Inventory.dragStartConID)
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

    public void DragItem()
    {
        if (conScript.containerSlots[slotID] != null)
        { //Check if the slot is not empty.
            UI_Inventory.dragItem = conScript.containerSlots[slotID].GetComponent<UI_Item>().itemID; //Set the drag item id on the id of the item on the current slot.
            UI_Inventory.dragStartConID = slotID;
            UI_Inventory.dragStartID = -1;
            UI_Inventory.dragStartEquipID = -1;
            UI_Inventory.dragStartHotID = -1;
            //       eventManager.OnDragBegin(slotID);
        }
    }

    public void SetDropID() //Setting the dropID on the id of the slot where the mouse is hovering.
    {
        UI_Inventory.dropConID = slotID;
    }
    public void ResetDropID() //Reset the dropID to null (-1)
    {
        UI_Inventory.dropConID = -1;
    }

    public void DropItem() //Drop item
    {

        if (slotID == UI_Inventory.dragStartConID)
        {
            if (UI_Inventory.dropID == -1 && UI_Inventory.dropEquipID == -1 && UI_Inventory.dropConID == -1 && UI_Inventory.dropHotID == -1)
            { //Drag was ended out of any slot.
                //Drag was ended out of any slot.
                if (inv.dropWhenDragOutOfInv == false)
                {
                    //	eventManager.OnDragEnd (slotID, -1);
                    //reset drag & drop variables
                    ResetStuff();
                }
                else
                {
                    //eventManager.OnDragEnd (slotID, -2);
                    //Send the stack number on the item before dropping it
                    for (int i = 0; i < conScript.stackSlots[slotID]; i++)
                    {
                        //Drop the item
                        //Spawn the item
                        GameObject dropped = (GameObject)Instantiate(conScript.containerSlots[slotID], inv.dropPos.position, conScript.containerSlots[slotID].transform.rotation);

                        if (dropped.GetComponent<UI_Item>().hasDurability == true)
                        { //checks if the item has durability
                            dropped.GetComponent<UI_Item>().itemDurability = conScript.allDur[slotID];//sync the durability with the spawned item
                        }
                        //Reset the drag system
                    }
                    conScript.containerSlots[slotID] = null; //empty the slot
                    conScript.stackSlots[slotID] = 0; //reset the stack
                    //reset drag & drop variables
                    ResetStuff();
                    conScript.UpdateContainerUI();
                }
            }

            if (UI_Inventory.dropID != -1)
            { //the item was dropped on an inventory slot
                if (slotID == UI_Inventory.dragStartConID)
                {
                    //Item dropped on a slot.
                    if (inv.allSlots[UI_Inventory.dropID] != null)
                    { //The slot already contains an item.
                        //Checks whether the slot contains an item of the same type or not.
                        if (inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        {
                            //Checks if the slot has stack space
                            if (inv.allSlotsStack[UI_Inventory.dropID] + conScript.stackSlots[UI_Inventory.dragStartConID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            {//the slot can fit all the new stack

                                //Increase the stack and return the drag to null
                                inv.allSlotsStack[UI_Inventory.dropID] += conScript.stackSlots[UI_Inventory.dragStartConID];
                                //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                                //Reset item stack
                                conScript.stackSlots[slotID] = 0;
                                conScript.containerSlots[slotID] = null;

                            }
                            else
                            {
                                //slot does not fit all the new stack
                                int canPlace = inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().maxStack - inv.allSlotsStack[UI_Inventory.dropID];
                                int toPlace = conScript.stackSlots[UI_Inventory.dragStartConID];
                                //max the slot
                                inv.allSlotsStack[UI_Inventory.dropID] = inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().maxStack;
                                //place the rest on the starting slot
                                conScript.stackSlots[UI_Inventory.dragStartConID] = toPlace - canPlace;
                            }
                        }
                        else
                        {
                            //Item is not the same
                            //Drag is canceled
                            //	eventManager.OnDragEnd (slotID, -1);
                            ResetStuff();
                        }
                    }
                    else
                    { //If the slot is empty
                        //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                        inv.allSlots[UI_Inventory.dropID] = itemList.allItems[UI_Inventory.dragItem];
                        inv.allSlotsStack[UI_Inventory.dropID] = conScript.stackSlots[UI_Inventory.dragStartConID];
                        if (inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().hasDurability == true)
                        { //checks if the item has durability
                            inv.allDur[UI_Inventory.dropID] = conScript.allDur[slotID]; //move the durability through the slots
                        }
                        //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                        //Reset item stack
                        conScript.stackSlots[slotID] = 0;
                        conScript.containerSlots[slotID] = null;
                    }
                }

            }
            if (UI_Inventory.dropEquipID != -1)
            { //the item was dropped on an equipment slot
                if (playerEquipment.equipSlotsGO[UI_Inventory.dropEquipID].GetComponent<UI_EquipSlot>().equipmentType == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().equipmentCat)
                {
                    if (playerEquipment.equipmentSlots[UI_Inventory.dropEquipID] == null)
                    { //checks if the equipment slot is empty
                        //set the dragged item on the slot
                        playerEquipment.equipmentSlots[UI_Inventory.dropEquipID] = itemList.allItems[UI_Inventory.dragItem];
                        //checks whether the item was dragged from an inventory slot or from an equipment
                        if (UI_Inventory.dragStartConID != -1)
                        {
                            //item was dragged from an inventory slot
                            //Checks if the item was on a stack
                            if (conScript.stackSlots[slotID] == 1)
                            {
                                //if its not on stack it removes the item from the slot
                                conScript.stackSlots[slotID] = 0;
                                conScript.containerSlots[slotID] = null;
                            }
                            else
                            {
                                //else if it is on stack it just decrease the stack
                                conScript.stackSlots[slotID]--;
                            }

                            //set durability

                            if (playerEquipment.equipmentSlots[UI_Inventory.dropEquipID].GetComponent<UI_Item>().hasDurability == true)
                            { //checks if the item has durability
                                playerEquipment.allDur[UI_Inventory.dropEquipID] = conScript.allDur[slotID]; //move the durability through the slots
                            }
                        }
                        if (UI_Inventory.dragStartEquipID != -1)
                        {
                            //item was dragged from an equipment slot
                            //remove the item from the slot
                            playerEquipment.equipmentSlots[UI_Inventory.dragStartEquipID] = null;

                        }

                        //reset drag & drop variables
                        ResetStuff();
                    }
                    else
                    {
                        //Slot is full
                        //Drag is canceled
                        //	eventManager.OnDragEnd (slotID, -1);
                        ResetStuff();
                    }
                }
                else
                {
                    //equipment category does not match
                    //Drag is canceled
                    //	eventManager.OnDragEnd (slotID, -1);
                    ResetStuff();
                }

            }

            if (UI_Inventory.dropConID != -1)
            {
                if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID] != null)
                { //The slot already contains an item.
                    //Checks whether the slot contains an item of the same type or not.
                    if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                    {
                        //Checks if the slot has stack space
                        if (UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] + conScript.stackSlots[UI_Inventory.dragStartConID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                        {//The slot can fit all the new stack
                            //Increase the stack and return the drag to null
                            UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] += conScript.stackSlots[UI_Inventory.dragStartConID];
                            //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                            //Reset item stack
                            conScript.stackSlots[slotID] = 0;
                            conScript.containerSlots[slotID] = null;
                        }
                        else
                        {
                            //slot does not fit all the new stack
                            //find how much we can take
                            int canPlace = conScript.containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().maxStack - conScript.stackSlots[UI_Inventory.dropConID];
                            int toPlace = conScript.stackSlots[UI_Inventory.dragStartConID];
                            //max the slot
                            conScript.stackSlots[UI_Inventory.dropConID] = conScript.containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().maxStack;
                            //place the rest on the starting slot
                            conScript.stackSlots[UI_Inventory.dragStartConID] = toPlace - canPlace;
                        }
                    }
                    else
                    {
                        //Item is not the same
                        //Drag is canceled
                        //	eventManager.OnDragEnd (slotID, -1);
                        ResetStuff();
                    }
                }
                else
                { //If the slot is empty
                    //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                    UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID] = itemList.allItems[UI_Inventory.dragItem];
                    UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] = conScript.stackSlots[UI_Inventory.dragStartConID];
                    if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().hasDurability == true)
                    { //checks if the item has durability 
                        UI_Inventory.openedContainer.GetComponent<UI_Container>().allDur[UI_Inventory.dropConID] = conScript.allDur[slotID];//move the durability through the slots
                    }
                    //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                    //Reset item stack
                    conScript.stackSlots[slotID] = 0;
                    conScript.containerSlots[slotID] = null;
                }
            }

            if (UI_Inventory.dropHotID != -1)
            {
                //the item was dropped on an hotbar slot
                if (slotID == UI_Inventory.dragStartConID)
                {
                    //check if the hotbar slot is empty
                    if (hotbar.hotbarSlots[UI_Inventory.dropHotID] != null)
                    { //it is not empty
                        //check if it contains the same item with the dropped
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        { //the item is the same
                            //check if there is stack space
                            if (hotbar.hotbarStack[UI_Inventory.dropHotID] + conScript.stackSlots[UI_Inventory.dragStartConID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            { //The slot can fit all the new stack

                                //Increase the stack and return the drag to null
                                hotbar.hotbarStack[UI_Inventory.dropHotID] += conScript.stackSlots[UI_Inventory.dragStartConID];
                                UI_Inventory.dragItem = -1;
                                UI_Inventory.dropHotID = -1;
                                //Reset item Stack
                                conScript.stackSlots[slotID] = 0;
                                conScript.containerSlots[slotID] = null;
                            }
                            else
                            {
                                //slot does not fit all the new stack
                                int canPlace = hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().maxStack - hotbar.hotbarStack[UI_Inventory.dropHotID];
                                int toPlace = conScript.stackSlots[UI_Inventory.dragStartConID];
                                //max the slot
                                hotbar.hotbarStack[UI_Inventory.dropHotID] = hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().maxStack;
                                //place the rest on the starting slot
                                conScript.stackSlots[UI_Inventory.dragStartConID] = toPlace - canPlace;
                            }
                        }
                        else
                        {
                            //Item is not the same
                            //Drag is canceled
                            //	eventManager.OnDragEnd (slotID, -1);
                            ResetStuff();
                        }
                    }
                    else
                    {

                        //it is empty
                        //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                        hotbar.hotbarSlots[UI_Inventory.dropHotID] = itemList.allItems[UI_Inventory.dragItem];
                        hotbar.hotbarStack[UI_Inventory.dropHotID] = conScript.stackSlots[UI_Inventory.dragStartConID];
                        //check if the item has durability
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().hasDurability == true)
                        {//it does
                            hotbar.hotDurability[UI_Inventory.dropHotID] = inv.allDur[UI_Inventory.dragStartID]; //move the durability through the slots
                        }
                        UI_Inventory.dragItem = -1;
                        UI_Inventory.dropHotID = -1;
                        //Reset item stack
                        conScript.stackSlots[slotID] = 0;
                        conScript.containerSlots[slotID] = null;
                    }
                }
            }
            hotbar.UpdateHotbarUI();
            playerEquipment.UpdateEquipmentUI();
            inv.UpdateInventoryUI();
            conScript.UpdateContainerUI();
        }
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
}
