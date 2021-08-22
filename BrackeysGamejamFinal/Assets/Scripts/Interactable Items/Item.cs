using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemType itemType;
    private string itemName; //more specific name of the item
    private int itemAmount;
    private Sprite itemSprite;

    public void SetItemFields(ItemScriptable itemScript)
    {
        itemType = itemScript.itemType;
        itemName = itemScript.itemName;
        itemAmount = itemScript.itemAmount;
        itemSprite = itemScript.itemSprite;
    }

    public void IncreaseDataAmount(int amount)
    {
        itemAmount += amount;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public int GetItemAmount()
    {
        return itemAmount;
    }

}
