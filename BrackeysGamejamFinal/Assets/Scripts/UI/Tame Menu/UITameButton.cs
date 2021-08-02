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

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        //set the type of dragon in the panel tile
        SetDragonType();

        //set the interactability of the button based on the inventory
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        inventory = inventorySave.inventory;

        SetInteractability();
    }

    private void SetInteractability()
    {
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
}
