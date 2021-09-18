using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Square,
    Circle,
    Diamond
}

public class ItemNotCollected : MonoBehaviour
{
    public ItemScriptable itemScriptable;
    
    private ItemData itemData;

    private void Awake()
    {
        SubscribeEvents();
    }
    
    private void OnDestroy()
    {
        Destroy(gameObject);
        UnsubscribeEvents();
    }

    public void InitialSerialization()
    {
        PopulateWithItem();
    }

    private void InitializeSerialization()
    {
        itemData = new ItemData();

        itemData.itemType = itemScriptable.itemType;
        itemData.itemName = itemScriptable.itemName;
        itemData.itemAmount = itemScriptable.itemAmount;
        itemData.itemID = name;
        itemData.collected = false;
    }

    private void InitializeDeserialization()
    {
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        InventoryData inventory = inventorySave.inventory;
        List<ItemData> list = inventory.interactableItems;

        foreach (ItemData item in list)
        {
            if (item.itemID == itemData.itemID)
            {
                itemData = item;

                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player")
        {
            //change item data property collected to true
            itemData.collected = true;

            //reflect changes to inventory save
            InventorySave.Instance.ReplaceItemList(itemData);
            InventorySave.Instance.SaveInventoryData();
            
            //update the inventory immediately
            UIInventory.Instance.RefreshInventoryItems(itemData, itemScriptable);
            
            OnDestroy();
        }
    }

    private bool Collected(out bool used)
    {
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        InventoryData inventory = inventorySave.inventory;
        List<ItemData> list = inventory.interactableItems;

        foreach (ItemData item in list)
        {
            if (item.itemID == itemData.itemID)
            {
                bool collected = item.collected ? true : false;
                used = false;
                return collected;
            }
        }
        used = true;
        return true;
    }

    private void PopulateWithItem()
    {
        InitializeSerialization();
        InventorySave.Instance.PopulateItemList(itemData);
        InventorySave.Instance.SaveInventoryData();
    }

    private void ReloadThisItem()
    {
        if (Collected(out bool used))
        {
            if (!used)
            {
                //make sure it is properly reflected in the inventory ui before removing
                UIInventory.Instance.RefreshInventoryItems(itemData, itemScriptable);
            }
            //then destroy the item in the map, to make sure items that have been collected do not show up again
            OnDestroy();
            return;
        }

        InitializeDeserialization();
    }


    private void SubscribeEvents()
    {
        //Serialization, proper instantiation @ scene start
        GameManager.OnLevelFirstInstance += InitialSerialization;
        GameManager.OnLevelNormalInstance += InitializeSerialization;
        SerializationCommander.ReloadAllItems += ReloadThisItem;
    }

    private void UnsubscribeEvents()
    {
        //Serialization, proper instantiation @ scene start
        GameManager.OnLevelFirstInstance -= InitialSerialization;
        GameManager.OnLevelNormalInstance -= InitializeSerialization;
        SerializationCommander.ReloadAllItems -= ReloadThisItem;
    }
}
