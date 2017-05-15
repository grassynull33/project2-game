using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UI_EventManager : MonoBehaviour {
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    private string description = "";
    private string item = "";
    const int kMaxLogSize = 16382;
   void Start()
    {
        dependencyStatus = FirebaseApp.CheckDependencies();
        if (dependencyStatus != DependencyStatus.Available) {
            FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
                dependencyStatus = FirebaseApp.CheckDependencies();
                if (dependencyStatus == DependencyStatus.Available) {
                    InitializeFirebase();
                } else {
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        } else {
            InitializeFirebase();
        }
}
            void InitializeFirebase() {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.SetEditorDatabaseUrl("https://unity-93b07.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
  }

    TransactionResult InventoryUpdate(MutableData mutableData) {
        List<object> inventory = mutableData.Value as List<object>;
        Dictionary<string, object> inventorySetup = new Dictionary<string, object>();
        inventorySetup["item"] = item;
        // inventorySetup["description"] = description;
        inventory.Add(inventorySetup);
        mutableData.Value = inventory;
    return TransactionResult.Success(mutableData);
  }

    public void PickupItemEvent(GameObject item, int slotID, bool wasStacked, string desc) //This event will be triggered when you pick up an item.
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Inventory");

        reference.RunTransaction(InventoryUpdate).ContinueWith(task => {
         if (task.Exception != null) {
        Debug.Log("Nope");
      } else if (task.IsCompleted) {
        Debug.Log("Works");
      }
    });
        
        reference.Child("name").Push().SetValueAsync(item.name);
        reference.Child("description").Push().SetValueAsync(desc);
        reference.Child("slotID").Push().SetValueAsync(slotID);
        reference.Child("More than One in Inventory").Push().SetValueAsync(wasStacked);

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
