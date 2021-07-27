using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySave
{
    private static InventorySave instance;
    public static InventorySave Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InventorySave();
            }
            return instance;
        }
    }
    public string path;

    public InventoryData inventory = new InventoryData();

    #region Dragons

    public void AddDragon()
    {
        inventory.dragons++;
    }

    public int CountTamedDragons(DragonType type)
    {
        return inventory.CountTamedDragons(type);
    }

    private int FindDragon(DragonData specificDragon)
    {
        List<DragonData> list = inventory.ChooseDragonList(specificDragon.dType);

        foreach (DragonData dragon in list)
        {
            if (dragon.name == specificDragon.name)
            {
                return list.IndexOf(dragon);
            }
        }

        throw new NotFoundInListException();
    }

    public void PopulateDragonList(DragonData dragon)
    {
        inventory.PopulateDragonList(dragon);
    }

    public void ReplaceDragonList(DragonData dragon)
    {
        int index = FindDragon(dragon);
        List<DragonData> list = inventory.ChooseDragonList(dragon.dType);
        list[index] = dragon;
    }

    #endregion

    #region Stones

    public int CountCollectedStones(StoneType type)
    {
        return inventory.CountCollectedStones(type);
    }

    private int FindStone(StoneData specificStone)
    {
        List<StoneData> list = inventory.ChooseStoneList(specificStone.type);

        foreach (StoneData stone in list)
        {
            if (stone.name == specificStone.name)
            {
                return list.IndexOf(stone);
            }
        }

        throw new NotFoundInListException();
    }

    public void UseStone(StoneType type, int quantity)
    {
        inventory.UseStone(type, quantity);
    }

    public void PopulateStoneList(StoneData stone)
    {
        inventory.PopulateStoneList(stone);
    }

    public void ReplaceStoneList(StoneData stone)
    {
        int index = FindStone(stone);

        List<StoneData> list = inventory.ChooseStoneList(stone.type);
        list[index] = stone;
    }

    #endregion

    public InventorySave LoadInventoryData()
    {
        int found = path.IndexOf("/saves/");
        int level = Int32.Parse(path.Substring(found + 7, 1));

        try
        {
            if (level != GameManager.currLvl)
            {
                throw new WrongPathException();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        InventorySave inventory = SerializationManager.Load(path) as InventorySave;

        return inventory;
    }

    public void SaveInventoryData()
    {
        SerializationManager.Save($"{GameManager.currLvl.ToString()}/inv", GetType().Name, this, out path);
    }
}
