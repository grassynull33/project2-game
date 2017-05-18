using UnityEngine;
using System.Collections;

public class UI_EquipSlot : MonoBehaviour
{

    public GameObject inventoryGO;

    public GameObject playerEquipGO;
    public int slotID = 0;
    public string equipmentType = "Head"; //Head, Chest, Hands, Leggies, Feet
    public Sprite emptyIcon = null;

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
        playerEquipment = playerEquipGO.GetComponent<UI_PlayerEquipment>();
    }

    public void DragItem()
    {
        if (playerEquipment.equipmentSlots[slotID] != null)
        { //Check if the slot is not empty.
            UI_Inventory.dragItem = playerEquipment.equipmentSlots[slotID].GetComponent<UI_Item>().itemID; //Set the drag item id on the id of the item on the current slot.
            UI_Inventory.dragStartEquipID = slotID;
            UI_Inventory.dragStartID = -1;
            UI_Inventory.dragStartConID = -1;
            UI_Inventory.dragStartHotID = -1;
            //eventManager.OnDragBegin(slotID);
        }
    }

    void Update()
    {
        if (UI_Inventory.dragStartEquipID == slotID)
        {
            if (UI_Inventory.dragItem != -1)
            {//IF you drag an item,
                if (!Input.GetKey(KeyCode.Mouse0))
                { // and if you don't hold the mouse button then ,

                    DropItem(); //drop the item.
                    ResetStuff();
                }
            }
        }
    }

    public void DropItem() //Drop item
    {
        if (slotID == UI_Inventory.dragStartEquipID)
        {
            if (UI_Inventory.dropID == -1 && UI_Inventory.dropConID == -1 && UI_Inventory.dropEquipID == -1 && UI_Inventory.dropHotID == -1)
            {
                //drag was ended out of any slot
                if (inv.dropWhenDragOutOfInv == false)
                {
                    //reset drag & drop variables
                    ResetStuff();
                }
                else
                {
                    //drop the item
                    GameObject dropped = (GameObject)Instantiate(playerEquipment.equipmentSlots[slotID], inv.dropPos.position, playerEquipment.equipmentSlots[slotID].transform.rotation);
                    playerEquipment.equipmentSlots[slotID] = null;
                    ResetStuff();
                    playerEquipment.UpdateEquipmentUI();
                }
            }

            if (UI_Inventory.dropID != -1)
            {
                //item was dropped on an inventory slot
                if (slotID == UI_Inventory.dragStartEquipID)
                {
                    if (inv.allSlots[UI_Inventory.dropID] != null)
                    {
                        //the slot already contains an item
                        //checks whether the slot contains an item of the same type
                        if (inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        {
                            //Checks if the slot has stack space
                            if (inv.allSlotsStack[UI_Inventory.dropID] < itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            {
                                //Increase the stack and return the drag to null
                                inv.allSlotsStack[UI_Inventory.dropID]++;
                                //remove the item from the slot
                                playerEquipment.equipmentSlots[slotID] = null;
                            }
                            else
                            {
                                //slot is full
                                ResetStuff();
                            }
                        }
                        else
                        {
                            //item was not the same
                            ResetStuff();
                        }
                    }
                    else
                    {
                        //the slot is empty
                        //add the item into the slot
                        inv.allSlots[UI_Inventory.dropID] = itemList.allItems[UI_Inventory.dragItem];
                        //increase the stack
                        inv.allSlotsStack[UI_Inventory.dropID]++;
                        //durability does not supported yet
                        //------------------DURABILITY CODE HERE-------------------
                        //remove the item from the slot
                        Debug.Log("Removing Item From Slot");
                        playerEquipment.equipmentSlots[slotID] = null;
                    }
                }
            }

            //items can not be moved from equipment slot to equipment slot !!

            if (UI_Inventory.dropConID != -1)
            {
                //item was dropped on a container
                if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID] != null)
                {
                    //the slot already contains an item
                    //Checks whether the slot contains an item of the same type or not.
                    if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                    {
                        //check if the slot has space
                        if (UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] < itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                        {
                            //Increase the stack and return the drag to null
                            UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID]++;
                            //empty the slot
                            playerEquipment.equipmentSlots[slotID] = null;

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
                        ResetStuff();
                    }
                }
                else
                {
                    //the slot was empty
                    //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                    UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID] = itemList.allItems[UI_Inventory.dragItem];
                    UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID]++;
                    playerEquipment.equipmentSlots[slotID] = null;
                }
            }

            if (UI_Inventory.dropHotID != -1)
            {
                //the item was dropped on an hotbar slot
                if (slotID == UI_Inventory.dragStartEquipID)
                {
                    //check if the hotbar slot is empty
                    if (hotbar.hotbarSlots[UI_Inventory.dropHotID] != null)
                    { //it is not empty
                        //check if it contains the same item with the dropped
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        { //the item is the same
                            //check if there is stack space
                            if (hotbar.hotbarStack[UI_Inventory.dropHotID] < itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            { //it has space
                              //Increase the stack and return the drag to null
                                hotbar.hotbarStack[UI_Inventory.dropHotID]++;
                                UI_Inventory.dragItem = -1;
                                UI_Inventory.dropHotID = -1;
                                //empty the slot
                                playerEquipment.equipmentSlots[slotID] = null;
                            }
                            else
                            {  //Slot is full
                                //Drag is canceled
                                //	eventManager.OnDragEnd (slotID, -1);
                                ResetStuff();
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
                        hotbar.hotbarStack[UI_Inventory.dropHotID]++;
                        //check if the item has durability
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().hasDurability == true)
                        {//it does
                            hotbar.hotDurability[UI_Inventory.dropHotID] = inv.allDur[UI_Inventory.dragStartID]; //move the durability through the slots
                        }
                        UI_Inventory.dragItem = -1;
                        UI_Inventory.dropHotID = -1;
                        //empty the slot
                        playerEquipment.equipmentSlots[slotID] = null;
                    }
                }
            }
            hotbar.UpdateHotbarUI();
            playerEquipment.UpdateEquipmentUI();
            inv.UpdateInventoryUI();
            try
            {
                UI_Inventory.openedContainer.GetComponent<UI_Container>().UpdateContainerUI(); //refresh the opened container UI
            }
            catch { }
            if (UI_Inventory.enablePlayerEquipment == true)
            {
                playerEquipment.UpdateEquipmentUI(); //refresh the equipment ui
            }
        }
    }

    public void SetDropID() //Setting the dropID on the id of the slot where the mouse is hovering.
    {
        UI_Inventory.dropEquipID = slotID;
    }
    public void ResetDropID() //Reset the dropID to null (-1)
    {
        UI_Inventory.dropEquipID = -1;
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
