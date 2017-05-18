using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_CraftingQueue : MonoBehaviour {

    UI_Crafting craft;
    public Image icon;
    public Text title;

    void Awake()
    {
        craft = GameObject.Find("Ultimate Inventory Pro").GetComponent<UI_Crafting>(); //assign inv GO at once
    }

    public void RemoveItem()
    {
        craft.queue.Remove(this.gameObject);
    }
}
