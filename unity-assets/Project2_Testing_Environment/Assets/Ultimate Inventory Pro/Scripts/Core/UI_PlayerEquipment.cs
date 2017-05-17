using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_PlayerEquipment : MonoBehaviour {

	public List<GameObject> equipmentSlots = new List<GameObject>();
	public List<GameObject> equipSlotsGO = new List<GameObject> ();
	public List<float> allDur = new List<float> ();
	public GameObject slotHolder;
	public bool autoAddSlots = true;
	public UI_EventManager eventManager;
	public Sprite defaultEmpty;
	public bool autoAdjustSlots = true;

    void Awake()
    {
        if (autoAddSlots == true)
        {
            for (int i = 0; i < slotHolder.transform.childCount; i++)
            {
                equipmentSlots.Add(null);
                equipSlotsGO.Add(slotHolder.transform.GetChild(i).gameObject);
                allDur.Add(0);
            }
        }

        if (autoAdjustSlots == true)
        {
            for (int i = 0; i < slotHolder.transform.childCount; i++)
            {
                equipSlotsGO[i].GetComponent<UI_EquipSlot>().slotID = i;
            }
        }
    }

	void Start()
	{
		UpdateEquipmentUI ();
	}

    public void UpdateEquipmentUI()
    {
        //eventManager.InventoryRefreshBegin();
        for (int i = 0; i < equipSlotsGO.Count; i++)
        {
            //Checks whether the slot contains an item
            if (equipmentSlots[i] != null) //If yes then it does change the sprite & the stack text.
            {
                equipSlotsGO[i].transform.FindChild("Image").GetComponent<Image>().sprite = equipmentSlots[i].GetComponent<UI_Item>().itemPreview;
            }
            else//if no it will set the stack to 0 and the sprite to the default.
            {
                if (equipSlotsGO[i].GetComponent<UI_EquipSlot>().emptyIcon == null)
                {
                    equipSlotsGO[i].transform.FindChild("Image").GetComponent<Image>().sprite = defaultEmpty;
                }
                else
                {
                    equipSlotsGO[i].transform.FindChild("Image").GetComponent<Image>().sprite = equipSlotsGO[i].GetComponent<UI_EquipSlot>().emptyIcon;
                }
            }
        }
        //eventManager.InventoryRefreshEnd();
    }
}
