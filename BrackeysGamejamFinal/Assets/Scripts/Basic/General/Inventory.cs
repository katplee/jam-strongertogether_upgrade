using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    #region Stones
    //[HideInInspector]
    public int fireStones = 0;
    //[HideInInspector]
    public int earthStones = 0;
    //[HideInInspector]
    public int waterStones = 0;
    //[HideInInspector]
    public int airStones = 0;
    //[HideInInspector]
    public  int baseStones = 0;
    #endregion


    #region Dragons
    public int Dragons { get; private set; }

    private Dragon fireDragon = null;
    public GameObject fDragoPrefab;
    public bool hasFireD = false;

    private Dragon waterDragon = null;
    public GameObject wDragoPrefab;
    public bool hasWaterD = false;

    private Dragon earthDragon = null;
    public GameObject eDragoPrefab;
    public bool hasEarthD = false;

    private Dragon airDragon = null;
    public GameObject aDragoPrefab;
    public bool hasAirD = false;

    private Dragon baseDragon = null;
    public GameObject bDragoPrefab;
    public bool hasBaseD = false;

    public Dragon _dragon;
    #endregion

    private void Awake()
    {
        ConvertToPersistentData();       
    }

    public void AddDragon(Dragon d)
    {
        switch (d.DType)
        {
            case DragonType.FIRE:
                _dragon = fDragoPrefab.GetComponent<Dragon>();
                hasFireD = true;
                fireDragon = _dragon;
                break;
            case DragonType.WATER:
                _dragon = wDragoPrefab.GetComponent<Dragon>();
                hasWaterD = true;
                waterDragon = _dragon;
                break;
            case DragonType.EARTH:
                _dragon = eDragoPrefab.GetComponent<Dragon>();
                hasEarthD = true;
                earthDragon = _dragon;
                break;
            case DragonType.AIR:
                _dragon = aDragoPrefab.GetComponent<Dragon>();
                hasAirD = true;
                airDragon = _dragon;
                break;
            case DragonType.BASE:
                _dragon = bDragoPrefab.GetComponent<Dragon>();
                hasBaseD = true;
                baseDragon = _dragon;
                break;
        }

        /* temporarily disabling this until I fix data management
        _dragon.DType = d.DType;
        _dragon.xpPerMinute = d.xpPerMinute;
        _dragon.xpPerWonFight = d.xpPerWonFight;
        _dragon.xpPerLvl = d.xpPerLvl;
        _dragon.maxLvl = d.maxLvl;
        _dragon.tamingReqs.pStones = d.tamingReqs.pStones;
        _dragon.tamingReqs.pStonesReq = null;
        */

        Dragons++;
        GameObject.FindGameObjectWithTag("Player").GetComponent<TameMenu>().UpdateMenu();
    }

    //OKAY!
    public void AddStone(PStone stone)
    {
        switch (stone.type)
        {
            case StoneType.BASE:
                baseStones++;
                break;

            case StoneType.FIRE:
                fireStones++;
                break;

            case StoneType.WATER:
                waterStones++;
                break;

            case StoneType.AIR:
                airStones++;
                break;

            case StoneType.EARTH:
                earthStones++;
                break;
        }
    }

    private void ConvertToPersistentData()
    {
        DontDestroyOnLoad(this);

        //to avoid duplication of game objects when transitioning between scenes
        //this is not a singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //OKAY!
    public void UseStone(StoneType type, int amt)
    {
        switch (type)
        {
            case StoneType.BASE:
                baseStones++;
                break;
            case StoneType.FIRE:
                fireStones++;
                break;
            case StoneType.WATER:
                waterStones++;
                break;
            case StoneType.AIR:
                airStones++;
                break;
            case StoneType.EARTH:
                earthStones++;
                break;
        }
    }
}
