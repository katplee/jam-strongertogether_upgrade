using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    public delegate ItemScriptable spriteUpdateDelegate(ItemData itemData);
    public static event spriteUpdateDelegate OnItemUpdate;

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

    private List<string> itemSlotParam = new List<string>()
    {
        "Border", //object carrying the border sprite
        "Image", //object carrying the item image
        "Amount", //object carrying the amount container
        "Text", //object carrying the amount text
        "Exit" //object carrying the exit button sprite
    };

    List<GameObject> displayedItems = new List<GameObject>();
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
        //display collected items
        //RefreshInventoryItems();
    }

    public void RefreshInventoryItems(ItemData specItemData, ItemScriptable specItemScriptable)
    {
        Transform _transform = GenerateInstance(specItemData, out bool newItem);
        ItemCollected _item = AttachItemObject(specItemScriptable, _transform, newItem);
        UpdateSpriteParameters(_transform, _item);
    }

    private Transform GenerateInstance(ItemData specItemData, out bool newItem)
    {
        //update each item slot's visibility, name, etc etc
        
        if (!displayedItems.Exists(s => s.name == specItemData.itemType.ToString()))
        {
            Transform transform = Instantiate(itemSlotTemplate, itemContainer);
            //destroy the UI slot template script
            Destroy(transform.GetComponent<UISlotTemplate>());
            //set the inactive template to active
            transform.gameObject.SetActive(true);
            //set the slot's name for better readability
            transform.name = specItemData.itemType.ToString();
            //add the item's game object to the displayedItems list
            displayedItems.Add(transform.gameObject);

            newItem = true;
            return transform;
        }
        else
        {
            int i = displayedItems.FindIndex(s => s.name == specItemData.itemType.ToString());
            Transform transform = displayedItems[i].transform;

            newItem = false;
            return transform;
        }
    }

    private ItemCollected AttachItemObject(ItemScriptable specItemScriptable, Transform itemTransform, bool newItem)
    {
        //attach scripts that need to be attached, if any

        if (newItem)
        {
            ItemCollected item = itemTransform.gameObject.AddComponent<ItemCollected>();
            item.SetItemFields(specItemScriptable);

            return item;
        }
        else
        {
            ItemCollected item = itemTransform.GetComponent<ItemCollected>();
            item.IncreaseDataAmount(specItemScriptable.itemAmount);

            return item;
        }
    }

    private void UpdateSpriteParameters(Transform itemTransform, ItemCollected item)
    {
        //update the sprite
        Image itemImage = itemTransform.GetChild(itemSlotParam.IndexOf("Image")).GetComponent<Image>();
        itemImage.sprite = item.GetItemSprite();

        //update the amount
        TMP_Text text = itemTransform.GetChild(itemSlotParam.IndexOf("Text")).GetComponent<TMP_Text>();
        text.text = item.GetItemAmount().ToString();
    }
}
