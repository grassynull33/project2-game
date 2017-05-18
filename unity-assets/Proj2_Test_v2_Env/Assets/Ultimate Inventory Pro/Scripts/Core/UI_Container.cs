using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Container : MonoBehaviour {

    public List<GameObject> containerSlots = new List<GameObject>();//4
    public List<int> stackSlots = new List<int>();//4
    public List<GameObject> slotsGO = new List<GameObject>();//4
    public List<float> allDur = new List<float>();//4
    public Sprite defaultEmpty;//0
    public bool autoAddSlots = true, autoAdjustSlots = true;//4
    public GameObject containerUI;//3
    public GameObject containerHolder;//3
    public GameObject player;//3
    public bool isOpen = false;
    public KeyCode toggleKey = KeyCode.E;//0
    public GameObject inventoryGO, playerEquipGO;//3
    public float maxOpenRange = 10f;//0
    public bool animated = false;//1
    public bool useCloseAnim = true;//1
    public Animation anims;//1
    public string openAnim = "containerOpen", closeAnim = "containerClose"; //1
    public bool useSounds = true;//2
    public AudioSource sounds;//2
    public AudioClip openSound, closeSound; //2
    public bool autoClose = true;//0
    public bool autoOpenInv = true;//0
    bool opened = false;
    void Awake()
    {
        if (autoAddSlots == true)
        {
            slotsGO.Clear();
            stackSlots.Clear();
            containerSlots.Clear();
            stackSlots.Clear();
            for (int i = 0; i < containerHolder.transform.childCount; i++)
            {
                slotsGO.Add(containerHolder.transform.GetChild(i).gameObject);
                containerSlots.Add(null);
                allDur.Add(0);
                stackSlots.Add(0);
            }
        }

        if (autoAdjustSlots == true)
        {
            for (int i = 0; i < containerHolder.transform.childCount; i++)
            {
                containerHolder.transform.GetChild(i).GetComponent<UI_ContainerSlot>().slotID = i;
                containerHolder.transform.GetChild(i).GetComponent<UI_ContainerSlot>().inventoryGO = inventoryGO;
                containerHolder.transform.GetChild(i).GetComponent<UI_ContainerSlot>().playerEquipGO = playerEquipGO;
                containerHolder.transform.GetChild(i).GetComponent<UI_ContainerSlot>().container = gameObject;
            }
        }
    }

    public void ToggleContainer(bool status) //opening or closing the container
    {

        containerUI.SetActive(status); //toggle the UI
        UI_Inventory.openedContainer = gameObject; //set this container to the Ultimate Inventory Manager
        isOpen = status; //change the status

        if (autoOpenInv==true)//if it is going to open
        {
            GameObject.Find("Ultimate Inventory Pro").GetComponent<UI_Inventory>().ToggleIn();
        }
        if (animated == true)//if this container is animated
        {
            if (isOpen == true)//if it is going to open
            {
                if (useCloseAnim == true)
                {
                    anims.Play(openAnim);//play open anim
                }
                else
                {
                    if (opened == false)
                    {
                        anims.Play(openAnim);//play open anim
                    }
                }
            }
            else//if it is going to close
            {
                if (useCloseAnim)//if we allow closing anim
                {
                    anims.Play(closeAnim); //play close anim
                }
            }
        }
        if (useSounds == true)//if this container supports audio
        {
            if (isOpen == true)//if it is going to open
            {
                if (useCloseAnim == true)
                {
                    sounds.PlayOneShot(openSound);//play open sound
                }
                else
                {
                    if (opened == false)
                    {
                        sounds.PlayOneShot(openSound);//play open sound
                        opened = true;
                    }
                }
               
            }
            else//if it is going to close
            {
                if (useCloseAnim)//if we allow closing sound
                {
                    sounds.PlayOneShot(closeSound);//play close sound
                }
            }
        }
    }

    public void UpdateContainerUI()
    {
        for (int i=0; i<slotsGO.Count; i++)
        {
            //Checks whether the slot contains an item
            if (containerSlots[i] != null) //If yes then it does change the sprite & the stack text.
            {
                slotsGO[i].transform.FindChild("Image").GetComponent<Image>().sprite = containerSlots[i].GetComponent<UI_Item>().itemPreview;
                slotsGO[i].transform.FindChild("Text").GetComponent<Text>().text = stackSlots[i].ToString();
            }
            else//if no it will set the stack to 0 and the sprite to the default.
            {
                slotsGO[i].transform.FindChild("Image").GetComponent<Image>().sprite = defaultEmpty;
                slotsGO[i].transform.FindChild("Text").GetComponent<Text>().text = "0";
            }
        }
    }

    void Update()
    {
        if (isOpen == true && autoClose==true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > maxOpenRange)
            {
                ToggleContainer(false);
            }
        }
    }

}
