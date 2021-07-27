using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script contains all the time instances when serialization will take place.
 */

public class SerializationCommander : MonoBehaviour
{
    #region Enemy events
    public static Action ResaveAllEnemies;
    public static Action ReloadAllEnemies;
    #endregion

    #region Stone events
    public static Action ReloadAllStones;
    #endregion

    #region Dragon events
    public static Action ReloadAllDragons;
    #endregion

    public static SerializationCommander Instance { get; private set; }

    private void Awake()
    {
        ConvertToPersistentData();
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        /*
         * A. SCENE TRANSITIONS
         * 
         * JUST BEFORE THE SCENE TRANSITIONS TO: [CODE:B]
         * 1. BASIC SCENE:
         *    1.1 From the attack scene:
         *        - save all the stats of all dragons
         *        - save the stats of the player
         *        - save the stats of the enemy
         *          if the enemy was defeated, remove it from the list of enemies
         *    1.2 From the xxx scene:
         *    
         * 2. ATTACK SCENE:
         *    2.1 From the basic scene:
         *        - save the information of the enemy to fight
         *        - save the position of the player
         */

        //1.1 ATTACK SCENE TO THE BASIC SCENE
        SceneTransition.JustBeforeSceneTransition += B_AToBSerialization;

        //2.1 BASIC SCENE TO THE ATTACK SCENE
        SceneTransition.JustBeforeSceneTransition += B_BToASerialization;

        /* 
         * JUST AFTER THE SCENE TRANSITIONS TO: [CODE:A]
         * 1. BASIC SCENE:
         *    1.1 From the attack scene:
         *        - load the enemies based on the enemies in the list of enemies
         *        - load the dragons that can still be tamed based on the list of dragons
         *        - bring the player to the pre-fight position
         * 
         * 2. ATTACK SCENE:
         *    2.1 From the basic scene:
         *        - set the information of the enemy to fight
         *        - set the information of the player
         */

        //2.1 ATTACK SCENE FROM THE BASIC SCENE
    }

    private void UnsubscribeEvents()
    {
        //B_1.1 BASIC SCENE TO THE ATTACK SCENE
        SceneTransition.JustBeforeSceneTransition -= B_BToASerialization;

        //B_2.1 ATTACK SCENE TO THE BASIC SCENE
        SceneTransition.JustBeforeSceneTransition -= B_AToBSerialization;
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


    public void SerializeAll()
    {
        //Serialize the enemy data and save.        
        EnemySave.Instance.SaveEnemyData();

        //Serialize the player data and save.
        PlayerSave.Instance.SavePlayerData();
    }

    private void B_BToASerialization()
    {
        //this method will only be called during BASIC SCENE TO THE ATTACK SCENE
        if (GameManager.currentSceneName == GameManager.attackScene) { return; }

        //save enemy data - includes the last enemy data, all enemies' stats
        ResaveAllEnemies?.Invoke();

        //save the position of the player
        Player.Instance.AssignPlayer();
    }

    private void B_AToBSerialization()
    {
        if (GameManager.currentSceneName != GameManager.attackScene) { return; }

        //save the stats of the enemy
        EnemySave.Instance.SaveEnemyData();
    }

    //A_1.1 BASIC SCENE FROM THE ATTACK SCENE
    public void A_BFromASerialization()
    {
        //this method will only be called during BASIC SCENE FROM THE ATTACK SCENE
        if (GameManager.currentSceneName == GameManager.attackScene) { return; }

        //load the enemies based on the enemies in the list of enemies
        ReloadAllEnemies?.Invoke();
        ReloadAllStones?.Invoke();
        ReloadAllDragons?.Invoke();

        //reload the stats of the last enemy

        //bring player to the pre-fight position
        Player.Instance.InitializeDeserialization();
        Player.Instance.Reposition();
    }

    public void A_AFromBSerialization()
    {
        if (GameManager.currentSceneName != GameManager.attackScene) { return; }

        //serialization was done from the BattleStartState instead
    }

}
