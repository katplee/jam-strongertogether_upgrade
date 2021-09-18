using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonSpriteAssign : MonoBehaviour
{
    [SerializeField] private PanelLabel index;
    [SerializeField] private Image dragonPanelImage;

    [SerializeField] private Sprite fireDragonSprite;
    [SerializeField] private Sprite waterDragonSprite;
    [SerializeField] private Sprite windDragonSprite;
    [SerializeField] private Sprite earthDragonSprite;
    [SerializeField] private Sprite baseDragonSprite;

    private void Start()
    {
        if (index.index - 1 < 0) { return; }

        dragonPanelImage.sprite = AssignDragonSprite();
        Debug.Log($"type: {DragonsData.sortedDragonsStats[index.index - 1][0]}");
    }

    private Sprite AssignDragonSprite()
    {
        DragonType dragonType = 
            (DragonType)DragonsData.sortedDragonsStats[index.index - 1][0];

        Debug.Log($"type: {dragonType}");

        switch (dragonType)
        {
            case DragonType.FIRE:
                return fireDragonSprite;

            case DragonType.WATER:
                return waterDragonSprite;

            case DragonType.AIR:
                return windDragonSprite;

            case DragonType.EARTH:
                return earthDragonSprite;

            case DragonType.BASE:
                return baseDragonSprite;
        }

        return null;
    }
}
