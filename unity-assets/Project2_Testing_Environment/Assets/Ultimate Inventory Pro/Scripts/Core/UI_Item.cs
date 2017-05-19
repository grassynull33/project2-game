using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class UI_Item : MonoBehaviour {

    public int itemID = 0;
	
    public Sprite itemPreview;
    public string name = "Item";
    public string desc = "ITEM Dec";
    public int maxStack = 64;
    public float itemWeight = 0;
    public bool hasDurability = false;
    public float itemDurability = 100;
	public float useDurability = 0;
    public string itemCat = "Normal"; //Normal, Consumable, Equipment
	public string equipmentCat = "Head"; //Head, Chest, Hands, Leggies, Feet
    public bool destroyOnUse = false;
    public GameObject[] objectsToEquip;
    public string[] statsToEditOnConsume;
    public bool pickUpWhenOver = false;
    public bool isCraftable = false;
    public float craftingTime = 10f;
    public int[] resourcesID, resourcesAmount;
    public List<bool> afford = new List<bool>();
    public GameObject inv;
    public bool isBlueprint = false;
    public GameObject blueprintUnlock;
    public bool isBuilding = false;
    void Awake()
    {
        if (inv == null)
        {
            inv = GameObject.Find("Ultimate Inventory Pro");//assign the inventory gameobject at start. [Used only once, no performance issues]
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (pickUpWhenOver == true)//if the item can be picked up when the play is over the item
        {
            if (col.gameObject.tag == "Player") //if the item which is over is the player
            {
                inv.GetComponent<UI_Inventory>().PickupItem(this.gameObject);//pick the item
            }
        }
    }

}
