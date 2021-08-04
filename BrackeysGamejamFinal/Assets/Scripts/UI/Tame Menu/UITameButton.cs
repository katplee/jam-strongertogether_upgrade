using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITameButton : MonoBehaviour
{
    private Button button;

    private InventoryData inventory = new InventoryData();
    private DragonType type;

    private void Awake()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    void Start()
    {
        button = GetComponent<Button>();

        //set the type of dragon in the panel tile
        SetDragonType();

        //do not show dragon icon if no tamed dragons of the type has been tamed
        SetInteractability();
    }

    private void SetInteractability()
    { 
        //set the interactability of the button
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        inventory = inventorySave.inventory;

        if (inventory.CountTamedDragons(type) > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetDragonType()
    {
        int found = tag.IndexOf("Dragon");
        string element = tag.Substring(0, found).ToUpper();
        type = (DragonType)Enum.Parse(typeof(DragonType), element);
    }

    private void SubscribeEvents()
    {
        UITameMenu.OnTameMenuDisplay += SetInteractability;
    }

    private void UnsubscribeEvents()
    {
        UITameMenu.OnTameMenuDisplay -= SetInteractability;
    }
}
