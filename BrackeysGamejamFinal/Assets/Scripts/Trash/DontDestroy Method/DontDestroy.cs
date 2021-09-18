using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;

    private int count = 20;

    private Player player;
    private bool playerIsFound;
    public List<Dragon> dragons;

    //player stats
    public ElementType playerType;
    public float playerHP;
    public float playerArmor;
    public float playerDamageAmount;
    public int playerFireAttack;
    public int playerWaterAttack;
    public int playerWindAttack;
    public int playerEarthAttack;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //SavePlayer();
        SaveDragons();

    }

    private void SavePlayer()
    {
        /*
        if (playerIsFound) { return; }
        player = FindObjectOfType<Player>();
        playerIsFound = true;

        playerType = player.Type;
        playerHP = player.hp;
        playerArmor = player.armor;
        playerDamageAmount = player.DamageAmount();

        playerFireAttack = player.fireAttack;
        playerWaterAttack = player.waterAttack;
        playerWindAttack = player.windAttack;
        playerEarthAttack = player.earthAttack;
        */
    }

    private void SaveDragons()
    {

    }
}
