using UnityEngine;
using System.Collections;

public class UI_ItemActivate : MonoBehaviour
{


    //modify this function with the code you want to be executed when you activate an item
    public void ActivateItem(int id,string category,int source=-1)
    {
        UI_Item currentItem = GetComponent<UI_ItemsList>().allItems[id].GetComponent<UI_Item>(); //use this to access the activated item

       

        //Code for activating blueprint, modify with caution
        if (currentItem.isBlueprint == true) // the item will unlock a blueprint
        {
            currentItem.blueprintUnlock.SetActive(true);
        }

        //End of code
        //Add your code here
    }
}
