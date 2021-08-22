using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    //DATA OF STONES 
    public List<StoneData> baseStones = new List<StoneData>();
    public List<StoneData> fireStones = new List<StoneData>();
    public List<StoneData> waterStones = new List<StoneData>();
    public List<StoneData> earthStones = new List<StoneData>();
    public List<StoneData> airStones = new List<StoneData>();

    //DRAGONS
    public int dragons;
    public int flyingDragonIndex;

    //QUANTITY PER TYPE
    public List<DragonData> baseDragons = new List<DragonData>();
    public List<DragonData> fireDragons = new List<DragonData>();
    public List<DragonData> waterDragons = new List<DragonData>();
    public List<DragonData> earthDragons = new List<DragonData>();
    public List<DragonData> airDragons = new List<DragonData>();

    //INTERACTABLE ITEMS
    public List<ItemData> interactableItems = new List<ItemData>();

    #region Dragons

    public List<DragonData> ChooseDragonList(DragonType type)
    {
        switch (type)
        {
            case DragonType.FIRE:
                return fireDragons;

            case DragonType.WATER:
                return waterDragons;

            case DragonType.AIR:
                return airDragons;

            case DragonType.EARTH:
                return earthDragons;

            case DragonType.BASE:
                return baseDragons;

            case DragonType.NOTDRAGON:
                return null;
        }

        return null;
    }

    public List<DragonData> SendTamedDragonList(DragonType type)
    {
        if(CountTamedDragons(type) == 0) { return null; }

        List<DragonData> list = ChooseDragonList(type);
        List<DragonData> tamedList = new List<DragonData>();

        foreach (DragonData dragon in list)
        {
            if(dragon.isTame == true)
            {
                tamedList.Add(dragon);
            }
        }

        return tamedList;
    }

    public int CountTamedDragons(DragonType type)
    {
        List<DragonData> list = ChooseDragonList(type);
        int count = 0;

        foreach (DragonData dragon in list)
        {
            if (dragon.isTame == true)
            {
                count++;
            }
        }
        return count;
    }

    public void PopulateDragonList(DragonData dragon)
    {
        List<DragonData> list = ChooseDragonList(dragon.dType);
        list.Add(dragon);
    }

    public void AssignFlyingDragonIndex(int dragonIndex)
    {
        flyingDragonIndex = dragonIndex;
    }

    #endregion

    #region Stones

    public List<StoneData> ChooseStoneList(StoneType type)
    {
        switch (type)
        {
            case StoneType.FIRE:
                return fireStones;

            case StoneType.WATER:
                return waterStones;

            case StoneType.AIR:
                return airStones;

            case StoneType.EARTH:
                return earthStones;

            case StoneType.BASE:
                return baseStones;
        }

        return null;
    }

    public int CountCollectedStones(StoneType type)
    {
        List<StoneData> list = ChooseStoneList(type);
        int count = 0;

        foreach (StoneData stone in list)
        {
            if(stone.collected == true) { count++; }
        }
        return count;
    }

    public void PopulateStoneList(StoneData stone)
    {
        List<StoneData> list = ChooseStoneList(stone.type);
        list.Add(stone);
    }

    public void UseStone(StoneType type, int quantity)
    {
        List<StoneData> list = ChooseStoneList(type);
        int i = 0;
        int count = 0;

        while(count < quantity)
        {
            if (list[i].collected == true) 
            {
                list.Remove(list[i]);
                count++;
            }
            else { i++; }
        }
    }

    #endregion

    #region Items

    public void PopulateItemList(ItemData item)
    {
        interactableItems.Add(item);
    }

    public List<ItemData> ReturnCollectedItems()
    {
        List<ItemData> collectedItems = new List<ItemData>();

        foreach(ItemData item in interactableItems)
        {
            if (item.collected) { collectedItems.Add(item); }
        }

        return collectedItems;
    }

    public void TrashItem(ItemType type, int quantity)
    {
        List<ItemData> collectedItems = ReturnCollectedItems();

        int i = 0;
        int count = 0;

        while (count < quantity)
        {
            if (collectedItems[i].itemType == type)
            {
                interactableItems.Remove(collectedItems[i]);
                count++;
            }
            else { i++; }
        }
    }

    #endregion

}
