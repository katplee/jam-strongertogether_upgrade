using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class OnGameAssign : MonoBehaviour
{
    private int panelIndex;
    private Dragon dragon;

    private void Start()
    {
        DragonsData.NewDragonSaved += UpdateDragonCollection;

        panelIndex = GetComponent<PanelLabel>().index;
        dragon = gameObject.AddComponent<Dragon>();

        //if the number of dragons you own are less than 5, hide parts of the container
        if (panelIndex > DragonsData.sortedDragonsStats.Count) { gameObject.SetActive(false); return; }

        //insert the stats of the corresponding dragon to the newly created code
        SetDragonStats();
    }

    private void OnDestroy()
    {
        DragonsData.NewDragonSaved -= UpdateDragonCollection;
    }

    private void UpdateDragonCollection()
    {

    }

    ///         dragon type,
    ///         hp,
    ///         armor,
    ///         damage amount,
    ///         weakness,
    ///         weakness factor,
    ///         fire attack,
    ///         water attack,
    ///         wind attack,
    ///         earth attack

    private void SetDragonStats()
    {
        List<Object> saved = DragonsData.sortedDragonsStats[panelIndex - 1];

        /*
        dragon.DType = (Dragon.DragonType)saved[0];
        dragon.hp = (float)saved[1];
        dragon.armor = (float)saved[2];
        dragon.damageAmount = (float)saved[3];
        dragon.weakness = (Element.WeaknessType)saved[4];
        dragon.weaknessFactor = (float)saved[5];
        dragon.fireAttack = (int)saved[6];
        dragon.waterAttack = (int)saved[7];
        dragon.windAttack = (int)saved[8];
        dragon.earthAttack = (int)saved[9];
        */
    }
}
