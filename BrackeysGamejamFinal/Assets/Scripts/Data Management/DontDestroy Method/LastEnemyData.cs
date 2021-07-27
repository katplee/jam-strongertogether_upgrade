using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastEnemyData : MonoBehaviour
{
    public static LastEnemyData Instance;

    public string attackScene = "AttackScene";

    //enemy stats
    public string enemyName;
    public Sprite enemySprite;
    public ElementType enemyType;
    public float enemyHP;
    public float enemyArmor;
    public float enemyDamageAmount;
    public WeaknessType enemyWeakness;
    public float enemyWeaknessFactor;
    public int enemyFireAttack;
    public int enemyWaterAttack;
    public int enemyWindAttack;
    public int enemyEarthAttack;

    public static bool enemyDefeated = false;


    void Start()
    {
        //SceneTransition.JustBeforeSceneTransition += SaveEnemyData;

        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveEnemyData(Element lastEnemy)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == attackScene) { return; }

        enemySprite = lastEnemy.GetComponent<SpriteRenderer>().sprite;

        enemyName = lastEnemy.gameObject.name;

        enemyType = lastEnemy.Type;
        //enemyHP = lastEnemy.hp;
        //enemyArmor = lastEnemy.armor;
        enemyDamageAmount = lastEnemy.DamageAmount();
        //enemyWeakness = lastEnemy.weakness;
        //enemyWeaknessFactor = lastEnemy.weaknessFactor;

        //enemyFireAttack = lastEnemy.fireAttack;
        //enemyWaterAttack = lastEnemy.waterAttack;
        //enemyWindAttack = lastEnemy.windAttack;
        //enemyEarthAttack = lastEnemy.earthAttack;
    }
}
