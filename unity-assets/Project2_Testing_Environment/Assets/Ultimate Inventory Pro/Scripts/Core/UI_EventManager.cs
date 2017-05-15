using UnityEngine;
using System.Collections;

public class UI_EventManager : MonoBehaviour {


    public void PickupItemEvent(GameObject item, int slotID, bool wasStacked) //This event will be triggered when you pick up an item.
    {
        if (wasStacked) //Item was stacked on the inventory
        {

        }
        else //Item was placed on a new slot.
        {

        }
    }

    public void InventoryFullEvent(GameObject item) //This event will be triggered when you try to pickup an item but the inventory is full.
    {

    }

    public void InventoryRefreshBegin() //This event will be triggered right before the inventory's UI refresh.
    {

    }

    public void InventoryRefreshEnd() //This event will be triggered after the inventory's UI refresh.
    {

    }

    public void OnDragBegin(int dragBegin) //This event will be triggered when the user start dragging an item.
    {

    }

    public void OnDragEnd(int dragBegin, int dragEnd) //This event will be triggered when the user end the dragging. [-1 means that the item was dropped on invalid slot]
    {

    }

	public void EquipmentRefreshBegin() //This event will be triggered right before the equipment's UI refresh.
	{

	}

	public void EquipmentRefreshEnd() //This event will be triggered after the equipment's UI refresh.
	{
		
	}

}
