using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    //contained in the ItemScriptable script
    public ItemType itemType;
    public string itemName; //more specific name of the item
    public int itemAmount;

    public string itemID;
    public bool collected;

}
