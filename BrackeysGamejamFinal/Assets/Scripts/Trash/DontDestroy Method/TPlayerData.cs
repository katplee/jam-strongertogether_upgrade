using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TPlayerData : MonoBehaviour
{
    public static TPlayerData Instance;    

    private Player player;    

    //player stats
    public ElementType playerType;
    public float playerHP;
    public float playerArmor;
    public float playerDamageAmount;
    public WeaknessType playerWeakness;
    public float playerWeaknessFactor;
    public int playerFireAttack;
    public int playerWaterAttack;
    public int playerWindAttack;
    public int playerEarthAttack;

    public Vector3 playerBasicPosition;

    //dragon overview
    //public Dragon[] dragons;
    public int dragonCount;

    public string attackScene = "AttackScene";

    void Start()
    {
        SceneTransition.JustBeforeSceneTransition += SavePlayerPosition;

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
    }

    /*
    private void SavePlayer()
    {
        player = FindObjectOfType<Player>();
                
        playerType = player.Type;
        playerHP = player.hp;
        playerArmor = player.armor;
        playerDamageAmount = player.DamageAmount();
        playerWeakness = player.weakness;
        playerWeaknessFactor = player.weaknessFactor;

        playerFireAttack = player.fireAttack;
        playerWaterAttack = player.waterAttack;
        playerWindAttack = player.windAttack;
        playerEarthAttack = player.earthAttack;
    }
    */

    private void SavePlayerPosition()
    {
        player = FindObjectOfType<Player>();

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == attackScene) { return; }

        playerBasicPosition = player.transform.position;
    }    
}