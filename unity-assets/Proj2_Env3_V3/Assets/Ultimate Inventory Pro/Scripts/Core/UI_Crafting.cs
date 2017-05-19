using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_Crafting : MonoBehaviour {

    public GameObject advancedQueue;
    public GameObject contextMenu;
    public UI_CraftingQueueSystem queueGO;
    public List<GameObject> allTabs = new List<GameObject>();
    public List<GameObject> queue = new List<GameObject>();
    public int queueLimit = 0;//queue limit, 0 = inf
    bool isCrafting = false, count = false;
    public Text label, queueLabel;
    public Slider progress;
    float craftTime = 0, craftTotal = 0;
    int currentI = 0;

    public void ToggleAdvancedQueue()
    {
        advancedQueue.SetActive(!advancedQueue.activeSelf);
        advancedQueue.GetComponent<UI_CraftingQueueSystem>().DisplayUI();
    }

    void Update()
    {
        Craft(); //check if the crafting queue is empty
        FailSafe(); //if the crafting queue system fails, it will be fixed automatically
    }

    public void FailSafe(bool force=false)
    {
        if (force == false)
        {
            if (label.text.Contains("Inf"))
            {
                currentI = 0;
                isCrafting = false;
                count = false;
                craftTotal = 0;
                craftTime = 0;
                label.text = "0%";
                StopAllCoroutines();
            }
        }
        else
        {
            currentI = 0;
            isCrafting = false;
            count = false;
            craftTotal = 0;
            craftTime = 0;
            label.text = "0%";
            StopAllCoroutines();
        }
    }

    public void ContextMenu()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            contextMenu.SetActive(true);
            Vector2 pos = new Vector2(Input.mousePosition.x + contextMenu.GetComponent<RectTransform>().rect.width / 2, Input.mousePosition.y - contextMenu.GetComponent<RectTransform>().rect.height / 2);
            contextMenu.transform.position = pos;
        }
    }

    public void DisableContextMenu()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            contextMenu.SetActive(false);
        }
    }

    public void AddItemToQueue(int id) //this functions add a craftable item to the queue
    {
        if (queueLimit != 0) //if there is queue limit
        {
            if (queue.Count < queueLimit) //if the queue is less than the limit
            {
              
                queue.Add(GetComponent<UI_ItemsList>().allItems[id]); //add item to the queue
                queueGO.DisplayUI(); // refrest queue UI
            }
        }
        else
        {
            queue.Add(GetComponent<UI_ItemsList>().allItems[id]); //add item to the queue
            queueGO.DisplayUI(); // refrest queue UI
        }
    }

    public void Craft()
    {
        if (queue.Count > 0) //if the queue is not empty
        {
            for (int i = 0; i < queue.Count; i++) //for each item in the queue
            {
                if (isCrafting == false) //if we don't already craft
                {
                    currentI = i;
                    isCrafting = true; //we now craft
                    craftTime = 0;
                    craftTotal = queue[i].GetComponent<UI_Item>().craftingTime;
                    StartCoroutine(Crafting(queue[i].GetComponent<UI_Item>().craftingTime, queue[i].GetComponent<UI_Item>().itemID, i));
                }
                if (isCrafting == true) //if we craft
                {
                    if (count == false) //if we don't count time
                    {
                        count = true; //we count
                        StartCoroutine(CountSec()); //count
                    }
                }
            }
        }
        else
        {
            //reset ui
            craftTotal = 0;
            craftTime = 0;
            label.text = "0%";
        }
    }

    IEnumerator Crafting(float time,int id,int index)
    {
        yield return new WaitForSeconds(time); //wait for the item be crafted
        queue.RemoveAt(index);
        isCrafting = false; //we no longer craft
        Crafted(id); //add the item to the inv
    }

    IEnumerator CountSec()
    {
        yield return new WaitForSeconds(1); //wait for 1 sec
        count = false;//we no longer count
        craftTime++; //add time
        UpdateUI(); //update crafting UI
    }

    public void UpdateUI() //this function updates the UI for crafting
    {
       // Debug.Log(craftTime + "s / " + craftTotal + "s");

        progress.value = craftTime; //reset progress value
        progress.maxValue = craftTotal; //set max value
        float per = 100 * craftTime / craftTotal; //convert progress to %
        per = Mathf.Round(per); //round result
        label.text = per.ToString() + "%"; //display results
        if (queue.Count > 0)
        {
            try
            {
                string queueNfo = queue.Count.ToString() + " | " + queue[currentI].GetComponent<UI_Item>().name; //set up queue string
                queueLabel.text = queueNfo; //display queue info
            }
            catch { queueLabel.text = ""; }
        }
        else
        {
            queueLabel.text = "";
        }
    }

    public void Crafted(int id)
    {
        //Debug.Log(id + " Crafted");
        queueGO.DisplayUI(); // refrest queue UI
        GameObject craftedGO = (GameObject)Instantiate(GetComponent<UI_ItemsList>().allItems[id], GetComponent<UI_Inventory>().dropPos.position, GetComponent<UI_ItemsList>().allItems[id].transform.rotation);
        GetComponent<UI_Inventory>().PickupItem(craftedGO);
    }
}
