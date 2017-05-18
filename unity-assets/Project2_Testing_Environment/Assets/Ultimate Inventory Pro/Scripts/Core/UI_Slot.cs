using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Slot : MonoBehaviour {
	
    public GameObject inventoryGO;
	public GameObject playerEquipGO;
    public int slotID = 0;

    UI_Hotbar hotbar;
    UI_Inventory inv;
    UI_ItemsList itemList;
    UI_EventManager eventManager;
	UI_PlayerEquipment playerEquipment;
    public GameObject contextMenu;

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

    void Update()
    {
       // Debug.Log("Slot id : " + UI_Inventory.dragStartID);
        if (slotID == UI_Inventory.dragStartID)
        {
            if (UI_Inventory.dragItem != -1)//IF you drag an item,
            {
                if (!Input.GetKey(KeyCode.Mouse0)) // and if you don't hold the mouse button then ,
                {
                    DropItem(); //drop the item.
                    ResetStuff();
                }
            }
            if (contextMenu.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartCoroutine(secureCloseCon());
                }
            }
        }
    }

    IEnumerator secureCloseCon()
    {
        yield return new WaitForSeconds(0.1f);
        contextMenu.SetActive(false);
    }

    public void DragItem()
    {
        if (inv.allSlots[slotID] != null)
        { //Check if the slot is not empty.
            UI_Inventory.dragItem = inv.allSlots[slotID].GetComponent<UI_Item>().itemID; //Set the drag item id on the id of the item on the current slot.
            UI_Inventory.dragStartID = slotID;
            UI_Inventory.dragStartConID = -1;
            UI_Inventory.dragStartEquipID = -1;
            UI_Inventory.dragStartHotID = -1;
          //  eventManager.OnDragBegin(slotID);
        }
    }

    public void DropItem() //Drop item
    {

        if (slotID == UI_Inventory.dragStartID)
        {

            if (UI_Inventory.dropID == -1 && UI_Inventory.dropEquipID == -1 && UI_Inventory.dropConID == -1 && UI_Inventory.dropHotID == -1)
            { //Drag was ended out of any slot.
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
                    for (int i = 0; i < inv.allSlotsStack[slotID]; i++)
                    {
                        //Drop the item
                        //Spawn the item
                        GameObject dropped = (GameObject)Instantiate(inv.allSlots[slotID], inv.dropPos.position, inv.allSlots[slotID].transform.rotation);

                        if (dropped.GetComponent<UI_Item>().hasDurability == true)
                        { //checks if the item has durability
                            dropped.GetComponent<UI_Item>().itemDurability = inv.allDur[slotID];//sync the durability with the spawned item
                        }
                        //Reset the drag system
                    }
                    inv.allSlots[slotID] = null; //empty the slot
                    inv.allSlotsStack[slotID] = 0; //reset the stack
                    //reset drag & drop variables
                    ResetStuff();
                    inv.UpdateInventoryUI();
                }
            }

            if (UI_Inventory.dropID != -1)
            { //the item was dropped on an inventory slot
                if (slotID == UI_Inventory.dragStartID)
                {
                    if (UI_Inventory.dropID == slotID)
                    { //Item dropped on the same slot where it was.
                        //reset drag & drop variables
                        ResetStuff();
                        //	eventManager.OnDragEnd (slotID, -1);
                    }
                    else
                    {

                        //Item dropped on a slot.
                        if (inv.allSlots[UI_Inventory.dropID] != null)
                        { //The slot already contains an item.
                            //Checks whether the slot contains an item of the same type or not.
                            if (inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                            {
                                //Checks if the slot has stack space
                                if (inv.allSlotsStack[UI_Inventory.dropID] + inv.allSlotsStack[UI_Inventory.dragStartID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                                {//The slot can fit all the new stack

                                    //Increase the stack and return the drag to null
                                    inv.allSlotsStack[UI_Inventory.dropID] += inv.allSlotsStack[UI_Inventory.dragStartID];
                                    //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                                    UI_Inventory.dropID = -1;
                                    UI_Inventory.dragItem = -1;
                                    //Reset item stack
                                    inv.allSlotsStack[slotID] = 0;
                                    inv.allSlots[slotID] = null;

                                }
                                else
                                {//slot does not fit all the new stack
                                    //find how much we can take
                                    int canPlace = inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().maxStack - inv.allSlotsStack[UI_Inventory.dropID];
                                    int toPlace = inv.allSlotsStack[UI_Inventory.dragStartID];
                                    //max the slot
                                    inv.allSlotsStack[UI_Inventory.dropID] = inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().maxStack;
                                    //place the rest on the starting slot
                                    inv.allSlotsStack[UI_Inventory.dragStartID] = toPlace - canPlace;
                                }
                            }
                            else
                            {
                                //Item is not the same
                                //Drag is canceled
                                //	eventManager.OnDragEnd (slotID, -1);
                                UI_Inventory.dropID = -1;
                                UI_Inventory.dragItem = -1;
                            }
                        }
                        else
                        { //If the slot is empty
                            //Add the item into the slot, increase the stack to 1 and then reset the drag to null
                            inv.allSlots[UI_Inventory.dropID] = itemList.allItems[UI_Inventory.dragItem];
                            inv.allSlotsStack[UI_Inventory.dropID] = inv.allSlotsStack[UI_Inventory.dragStartID];
                            if (inv.allSlots[UI_Inventory.dropID].GetComponent<UI_Item>().hasDurability == true)
                            { //checks if the item has durability
                                inv.allDur[UI_Inventory.dropID] = inv.allDur[UI_Inventory.dragStartID]; //move the durability through the slots
                            }
                            //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                            UI_Inventory.dropID = -1;
                            UI_Inventory.dragItem = -1;
                            //Reset item stack
                            inv.allSlotsStack[slotID] = 0;
                            inv.allSlots[slotID] = null;
                        }
                    }
                }
            }


            if (UI_Inventory.dropEquipID != -1)
            { //the item was dropped on an equipment slot
                if (playerEquipment.equipSlotsGO[UI_Inventory.dropEquipID].GetComponent<UI_EquipSlot>().equipmentType == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().equipmentCat)
                { //check if the equipment category is the same
                    if (playerEquipment.equipmentSlots[UI_Inventory.dropEquipID] == null)
                    { //checks if the equipment slot is empty
                        //set the dragged item on the slot
                        playerEquipment.equipmentSlots[UI_Inventory.dropEquipID] = itemList.allItems[UI_Inventory.dragItem];
                        //checks whether the item was dragged from an inventory slot or from an equipment
                        if (UI_Inventory.dragStartID != -1)
                        {

                            //item was dragged from an inventory slot
                            //Checks if the item was on a stack
                            if (inv.allSlotsStack[UI_Inventory.dragStartID] == 1)
                            {
                                //if its not on stack it removes the item from the slot
                                inv.allSlotsStack[UI_Inventory.dragStartID] = 0;
                                inv.allSlots[UI_Inventory.dragStartID] = null;
                            }
                            else
                            {
                                //else if it is on stack it just decrease the stack
                                inv.allSlotsStack[UI_Inventory.dragStartID]--;
                            }

                            //set durability

                            if (playerEquipment.equipmentSlots[UI_Inventory.dropEquipID].GetComponent<UI_Item>().hasDurability == true)
                            { //checks if the item has durability
                                playerEquipment.allDur[UI_Inventory.dropEquipID] = inv.allDur[UI_Inventory.dragStartID]; //move the durability through the slots
                            }
                        }
                        if (UI_Inventory.dragStartEquipID != -1)
                        {
                            //item was dragged from an equipment slot
                            //remove the item from the slot
                            playerEquipment.equipmentSlots[UI_Inventory.dragStartEquipID] = null;
                            Debug.Log("Should never show up");

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
                //the item was dropped on an container slot
                if (slotID == UI_Inventory.dragStartID)
                {
                    //Item dropped on a slot.
                    if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID] != null)
                    { //The slot already contains an item.
                        //Checks whether the slot contains an item of the same type or not.
                        if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        {
                            //Checks if the slot has stack space
                            if (UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] + inv.allSlotsStack[UI_Inventory.dragStartID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            {//The slot can fit all the new stack

                                //Increase the stack and return the drag to null
                                UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] += inv.allSlotsStack[UI_Inventory.dragStartID];
                                //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                                UI_Inventory.dropConID = -1;
                                UI_Inventory.dragItem = -1;
                                //Reset item stack
                                inv.allSlotsStack[slotID] = 0;
                                inv.allSlots[slotID] = null;

                            }
                            else
                            {//slot does not fit all the new stack
                             //find how much we can take
                                int canPlace = UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().maxStack - UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID];
                                int toPlace = inv.allSlotsStack[UI_Inventory.dragStartID];
                                //max the slot
                                UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] = UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().maxStack;
                                //place the rest on the starting slot
                                inv.allSlotsStack[UI_Inventory.dragStartID] = toPlace - canPlace;
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
                        UI_Inventory.openedContainer.GetComponent<UI_Container>().stackSlots[UI_Inventory.dropConID] = inv.allSlotsStack[UI_Inventory.dragStartID];
                        if (UI_Inventory.openedContainer.GetComponent<UI_Container>().containerSlots[UI_Inventory.dropConID].GetComponent<UI_Item>().hasDurability == true)
                          { //checks if the item has durability
                              UI_Inventory.openedContainer.GetComponent<UI_Container>().allDur[UI_Inventory.dropConID] = inv.allDur[UI_Inventory.dragStartID]; //move the durability through the slots
                         }
                        //	eventManager.OnDragEnd (slotID, UI_Inventory.dropID);
                        UI_Inventory.dropConID = -1;
                        UI_Inventory.dragItem = -1;
                        //Reset item stack
                        inv.allSlotsStack[slotID] = 0;
                        inv.allSlots[slotID] = null;
                    }
                }
            }

            if (UI_Inventory.dropHotID != -1)
            {
                //the item was dropped on an hotbar slot
                if (slotID == UI_Inventory.dragStartID)
                {
                    //check if the hotbar slot is empty
                    if (hotbar.hotbarSlots[UI_Inventory.dropHotID] != null)
                    { //it is not empty
                        //check if it contains the same item with the dropped
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().itemID == itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().itemID)
                        { //the item is the same
                            //check if there is stack space
                            if (hotbar.hotbarStack[UI_Inventory.dropHotID] + inv.allSlotsStack[UI_Inventory.dragStartID] <= itemList.allItems[UI_Inventory.dragItem].GetComponent<UI_Item>().maxStack)
                            { //the slot can fit all the new stack
                              //Increase the stack and return the drag to null
                                hotbar.hotbarStack[UI_Inventory.dropHotID] += inv.allSlotsStack[UI_Inventory.dragStartID];
                                UI_Inventory.dragItem = -1;
                                UI_Inventory.dropHotID = -1;
                                //Reset Item stack
                                inv.allSlotsStack[slotID] = 0;
                                inv.allSlots[slotID] = null;
                            }
                            else
                            {
                                //slot does not fit all the new stack
                                //find how much we can take
                                int canPlace = hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().maxStack - hotbar.hotbarStack[UI_Inventory.dropHotID];
                                int toPlace = inv.allSlotsStack[UI_Inventory.dragStartID];
                                //max the slot
                                hotbar.hotbarStack[UI_Inventory.dropHotID] = hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().maxStack;
                                //place the rest on the starting slot
                                inv.allSlotsStack[UI_Inventory.dragStartID] = toPlace - canPlace;
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
                        hotbar.hotbarStack[UI_Inventory.dropHotID] = inv.allSlotsStack[UI_Inventory.dragStartID];
                        //check if the item has durability
                        if (hotbar.hotbarSlots[UI_Inventory.dropHotID].GetComponent<UI_Item>().hasDurability == true)
                        {//it does
                            hotbar.hotDurability[UI_Inventory.dropHotID] = inv.allDur[UI_Inventory.dragStartID]; //move the durability through the slots
                        }
                        UI_Inventory.dragItem = -1;
                        UI_Inventory.dropHotID = -1;
                        //Reset Item stack
                        inv.allSlotsStack[slotID] = 0;
                        inv.allSlots[slotID] = null;
                    }
                }
            }

            inv.UpdateInventoryUI(); //refresh the inventory ui
            hotbar.UpdateHotbarUI(); //refresh the hotbar ui
            try
            {
                UI_Inventory.openedContainer.GetComponent<UI_Container>().UpdateContainerUI(); //refresh the opened container UI
            }
            catch { }
            if (UI_Inventory.enablePlayerEquipment == true)
            {
                playerEquipment.UpdateEquipmentUI(); //refresh the equipment ui
            }
            ResetStuff();
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

    public void SetDropID() //Setting the dropID on the id of the slot where the mouse is hovering.
    {
        UI_Inventory.dropID = slotID;
    }
    public void ResetDropID() //Reset the dropID to null (-1)
    {
        UI_Inventory.dropID = -1;
    }

    public void PerformRightClick() //Pop-ups the context menu on the selected slot on right mouse button.
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && inv.allowContextMenu == true)
        {
            if (inv.allSlots[slotID] != null)
            {
                contextMenu.SetActive(true);
                Vector2 pos = new Vector2(Input.mousePosition.x + contextMenu.GetComponent<RectTransform>().rect.width / 2, Input.mousePosition.y - contextMenu.GetComponent<RectTransform>().rect.height / 2);
                contextMenu.transform.position = pos;
                contextMenu.GetComponent<UI_ContextButton>().slotID = slotID;
            }
        }
    }



    bool mouseClicksStarted = false; int mouseClicks = 0; float mouseTimerLimit = 0.25f;

    public void UseItem()
    {
        if (inv.allSlots[slotID] != null)
        {
            if (inv.allowPreviewWin == false)
            {
                inv.ContextMenuEquip(slotID);
            }
            else
            {
                UI_Inventory.selectedSlot = slotID;
                inv.ActivateItemPreview();
            }
        }
    }

    public void OnSlotClick()
    {
        mouseClicks++;
        if (mouseClicksStarted)
        {
            return;
        }
        mouseClicksStarted = true;
        Invoke("checkMouseDoubleClick", mouseTimerLimit);
    }

    private void checkMouseDoubleClick()
    {
        if (mouseClicks > 1)
        {
            Debug.Log("Double Click");
        }
        else
        {
            UseItem();
        }
        mouseClicksStarted = false;
        mouseClicks = 0;
    }

}
