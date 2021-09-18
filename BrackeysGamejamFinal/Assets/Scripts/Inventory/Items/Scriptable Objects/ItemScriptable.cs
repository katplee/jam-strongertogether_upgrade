using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class ItemScriptable : ScriptableObject
{
    public ItemType itemType;
    public string itemName; //more specific name of the item
    public int itemAmount;
    public Sprite itemSprite;
}
