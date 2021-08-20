using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public delegate Sprite spriteUpdateDelegate();
    public static event spriteUpdateDelegate OnItemSpriteUpdate;

    private static UIInventory instance;
    public static UIInventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIInventory>();
            }
            return instance;
        }
    }

    private InventoryData inventory;
    private Transform itemSlotTemplate;
    private Transform itemContainer;

    public List<ItemScriptable> itemAssets = new List<ItemScriptable>(); //temporary fix

    //to be called from the UI object being declared
    public void DeclareThis<T>(string element, T UIObject)
        where T : MonoBehaviour
    {
        switch (element)
        {
            case "UISlotTemplate":
                SetItemSlotTemplate(UIObject as UISlotTemplate);
                break;

            case "UIContainer":
                SetItemContainer(UIObject as UIContainer);
                break;
        }
    }

    private void SetItemSlotTemplate(UISlotTemplate slotTemplate)
    {
        itemSlotTemplate = slotTemplate.transform;
    }

    private void SetItemContainer(UIContainer container)
    {
        itemContainer = container.transform;
    }

    private void Start()
    {
        SetInventory();
    }

    private void SetInventory()
    {
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        inventory = inventorySave.inventory;

        //display collected items
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        if(inventory.ReturnCollectedItems().Count == 0) { return; }

        foreach(ItemData item in inventory.ReturnCollectedItems())
        {
            #region Update each item slot's visibility, name, etc etc
            Transform transform = Instantiate(itemSlotTemplate, itemContainer);
            //destroy the UI slot template script
            Destroy(transform.GetComponent<UISlotTemplate>());
            //set the inactive template to active
            transform.gameObject.SetActive(true);
            //set the slot's name for better readability
            transform.name = item.itemType.ToString();
            #endregion

            #region Update the appearance
            //update the sprite
            
            #endregion


        }
    }
}
