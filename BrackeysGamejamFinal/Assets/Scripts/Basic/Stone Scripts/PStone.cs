using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoneType
{
    BASE, FIRE, WATER, EARTH, AIR
}

public class PStone : MonoBehaviour
{
    public StoneType type;

    private StoneData stoneData;

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
        PopulateWithStone();
    }

    private void InitializeSerialization()
    {
        stoneData = new StoneData();

        stoneData.type = type;
        stoneData.name = name;
        stoneData.collected = false;
    }

    private void InitializeDeserialization()
    {
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        InventoryData inventory = inventorySave.inventory;
        List<StoneData> list = inventory.ChooseStoneList(type);

        foreach (StoneData stone in list)
        {
            if(stoneData.name == stone.name)
            {
                stoneData = stone;

                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Player")
        {
            //change stone data property collected to true
            stoneData.collected = true;

            //reflect changes to inventory save
            InventorySave.Instance.ReplaceStoneList(stoneData);
            InventorySave.Instance.SaveInventoryData();
            OnDestroy();
        }
    }

    private bool Collected()
    {
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        InventoryData inventory = inventorySave.inventory;
        List<StoneData> list = inventory.ChooseStoneList(type);

        foreach (StoneData stone in list)
        {
            if(stone.name == stoneData.name)
            {
                bool collected = stone.collected ? true : false;
                return collected;
            }
        }

        return true;
    }

    private void PopulateWithStone()
    {
        InitializeSerialization();
        InventorySave.Instance.PopulateStoneList(stoneData);
        InventorySave.Instance.SaveInventoryData();
    }

    private void ReloadThisStone()
    {        
        if (Collected())
        {
            OnDestroy();
            return;
        }

        InitializeDeserialization();
    }

    private void SubscribeEvents()
    {
        GameManager.OnLevelFirstInstance += InitialSerialization;
        GameManager.OnLevelNormalInstance += InitializeSerialization;
        SerializationCommander.ReloadAllStones += ReloadThisStone;
    }

    private void UnsubscribeEvents()
    {
        GameManager.OnLevelFirstInstance -= InitialSerialization;
        GameManager.OnLevelNormalInstance -= InitializeSerialization;
        SerializationCommander.ReloadAllStones -= ReloadThisStone;
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Total Number of Stones" + Inventory.Instance.baseStones);

            //calls the AddStone method @ Inventory
            Inventory.Instance.AddStone(this);

            //then destroys the game object soon after
            Destroy(gameObject);

            Debug.Log("Total Number of Stones" + Inventory.Instance.baseStones);
        }
    }
    */
}
