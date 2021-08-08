using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Object = System.Object;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, ATTACKING, WON, LOST }

public class FightManager : MonoBehaviour
{
    public static event Action OnTurnEnd;
    public static event Action<BattleState> OnFightEnd;
    public static event Action OnDragonFuse;

    private static FightManager instance;
    public static FightManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FightManager>();
            }
            return instance;
        }
    }

    //GENERAL PARAMETERS
    public BattleState State { get; private set; }
    public float timeToWait = 2f; //remove this eventually
    private float test = 0;

    //ACTIONS PANEL UI
    public TMP_Text dialogueBox;
    public Button attackButton;
    public Button leaveButton;
    public Button switchButton;

    //DRAGON PANEL UI
    public GameObject dragonPanel;

    #region PLAYER CORNER UI
    public Player Player { get; set; }
    public GameObject PGO { get; set; }
    public UIBattleHUD PHUD { get; set; }
    #endregion

    #region ENEMY CORNER UI
    public Enemy Enemy { get; set; }
    public GameObject EGO { get; set; }
    public UIBattleHUD EHUD { get; set; }
    #endregion

    public Dragon currentDragon;
    public int currentDragonIndex = 0;

    [Header("Transition Values")]
    public SceneTransition sceneTransition;

    private void Start()
    {
        //SpriteManager.Instance.AssignRefIndex(dragonToFuseIndex);
        //SpriteManager.Instance.LoadAndAssign("Player");
    }

    public void ChangeStateName(BattleState state)
    {
        State = state;
    }

    public void OnAttack(bool isPlayerTurn)
    {
        Debug.Log(State);
        if (State != BattleState.ATTACKING) { return; }

        if (isPlayerTurn) { DealAttack(Player, Enemy); }
        else { DealAttack(Enemy, Player); }

        OnTurnEnd?.Invoke();
    }

    private void DealAttack(Element attacker, Element receiver)
    {

        if (attacker as Player)
        {
            // METHOD CALLER: not a dragon | ELEMENT WHOSE TAKEDAMAGE METHOD IS CALLED: not a dragon
            // call format: TakeDamage(float damageAmount)
            //if (attacker.Armor == 0) { NonDragonAttackNonDragon(attacker, receiver); }
            NonDragonAttackNonDragon(attacker, receiver);

        }

        else if (attacker as Enemy)
        {
            // METHOD CALLER: not a dragon | ELEMENT WHOSE TAKEDAMAGE METHOD IS CALLED: not a dragon
            // call format: TakeDamage(float damageAmount)
            if (!(attacker as Dragon)) { NonDragonAttackNonDragon(attacker, receiver); }
        }
    }

    private void NonDragonAttackNonDragon(Element attacker, Element receiver)
    {
        //inflict damage upon the enemy
        bool end = receiver.TakeDamage(attacker.DamageAmount());

        //check if there's a winner
        Check(end, receiver);
    }

    private void NonDragonAttackDragon()
    {

    }

    private void DragonAttackNonDragon()
    {

    }

    private void DragonAttackDragon()
    {

    }

    private void Check(bool end, Element receiver)
    {
        if (end == false) { return; }

        if (receiver as Player) { State = BattleState.LOST; }
        else if (receiver as Enemy) { State = BattleState.WON; }

        OnFightEnd?.Invoke(State);
    }

    public void OnFusePreview(int dragonToFuseIndex)
    {
        //erased parameter: dragondata?
        //must assign the refIndex needed in the sprite manager
        PlayerSpriteManager.Instance.AssignRefIndex(dragonToFuseIndex);
        PlayerSpriteManager.Instance.LoadAndAssign();

        //change avatar of player
        //SpriteRenderer spriteRenderer = PGO.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite;

        //change stats to that of dragon's
    }

    public void FuseDragonHP()
    {
        if (!UIDragonSubPanel.Instance.IsSelected) { return; }

        OnDragonFuse?.Invoke();
    }

    public void PassDragonData(DragonData dragon)
    {
        //set the 
        Player.SetParametersOnFuse(dragon);

        //update the HUDs
        HUDManager.Instance.HPlayer.UpdateHUD(Player);
        HUDManager.Instance.HEnemy.UpdateHUD(Enemy);
    }

    public void OnLeave()
    {
        //keeps the player's data and carries it over to the basic scene
        Player.Instance.AssignPlayer();
        
        sceneTransition.FadeTo(LevelData.mapScene);
    }




    /*

    public void TESTOnEnemyDefeated()
    {
        EnemySave.Instance.lastEnemy.hp = 0;
        EnemySave.Instance.lastEnemy.armor = 0;
        OnLeave();
    }

    private void SetEnemySprite()
    {
        //throw new NotImplementedException();
    }

    private void SetEnemyStats()
    {
        /*
        LastEnemyData saved = LastEnemyData.Instance;

        enemyGO.GetComponent<SpriteRenderer>().sprite = saved.enemySprite;
               
        enemy.hp = saved.enemyHP;
        enemy.armor = saved.enemyArmor;
        enemy.damageAmount = saved.enemyDamageAmount;

        enemy.weakness = saved.enemyWeakness;
        enemy.weaknessFactor = saved.enemyWeaknessFactor;

        enemy.fireAttack = saved.enemyFireAttack;
        enemy.waterAttack = saved.enemyWaterAttack;
        enemy.windAttack = saved.enemyWindAttack;
        enemy.earthAttack = saved.enemyEarthAttack;
        
    }

    private void SetPlayerStats()
    {
        /*
        if (player.Type == Element.ElementType.PLAYER)
        {
            PlayerData saved = PlayerData.Instance;

            player.hp = saved.playerHP;
            player.armor = saved.playerArmor;
            player.damageAmount = saved.playerDamageAmount;

            player.weakness = saved.playerWeakness;
            player.weaknessFactor = saved.playerWeaknessFactor;

            player.fireAttack = saved.playerFireAttack;
            player.waterAttack = saved.playerWaterAttack;
            player.windAttack = saved.playerWindAttack;
            player.earthAttack = saved.playerEarthAttack;
        }
        
    }

    private void UpdateSavedPlayerStats()
    {
        /*
        Debug.Log("It's a human!");
        PlayerData saved = PlayerData.Instance;

        saved.playerHP = player.hp;
        saved.playerArmor = player.armor;
        saved.playerDamageAmount = player.damageAmount;

        saved.playerWeakness = player.weakness;
        saved.playerWeaknessFactor = player.weaknessFactor;

        saved.playerFireAttack = player.fireAttack;
        saved.playerWaterAttack = player.waterAttack;
        saved.playerWindAttack = player.windAttack;
        saved.playerEarthAttack = player.earthAttack;
        
    }

    private void UpdateSavedDragonStats()
    {
        ///List<Object> saved = DragonsData.sortedDragonsStats[currentDragonIndex - 1];
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

        //saved[1] = player.armor;        
    }


    IEnumerator OnEnemyTurn()
    {
        //dialogueBox.text = state.ToString();

        //player cannot attack
        attackButton.interactable = false;

        //player cannot leave
        leaveButton.interactable = false;

        yield return new WaitForSeconds(timeToWait);

        //bool playerIsDead = player.TakeDamage(enemy.DamageAmount());

        //update the saved stats
        //UpdateSavedPlayerStats();
        if (currentDragonIndex != 0)
        {
            UpdateSavedDragonStats();
        }
        OnFightEnd?.Invoke();

        //update the player stats
        //playerHUD.UpdateHPArmor<Element>(player.hp, player.armor, player.maxHP, player.maxArmor);

        //update the dialogue
        dialogueBox.text = "ENEMYATTACKDONE";

        yield return new WaitForSeconds(timeToWait);

        //CheckForEnemyWin(playerIsDead);
    }

    private void DealDamage()
    {
        //if (enemy.Type == ElementType.DRAGON)
        //{

        //}

        //bool playerIsDead = player.TakeDamage(enemy.DamageAmount());
        //return playerIsDead;
    }

    public void TESTOnEnemyDefeated()
    {
        EnemySave.Instance.lastEnemy.hp = 0;
        EnemySave.Instance.lastEnemy.armor = 0;
    }

    public void OnSwitchButton()
    {
        if (state != BattleState.PLAYERTURN) { return; }

        //Show a panel where you can choose your dragons
        if (dragonPanel.activeSelf) { dragonPanel.SetActive(false); }
        else { dragonPanel.SetActive(true); }
    }

    public void OnSwitchDragon(GameObject dragonGO)
    {
        //if (state != BattleState.PLAYERTURN) { return; }

        dragonGO.TryGetComponent<PanelLabel>(out PanelLabel dragonLabel);

        //if the dragon is still the same, return
        if (currentDragonIndex == dragonLabel.index) { return; }

        //save the current armor to the hp of the old dragon
        if (currentDragonIndex != 0)
        {
            //currentDragon.hp = player.armor;
            UpdateSavedDragonStats();
            OnFightEnd?.Invoke();
        }        

        //if the dragon is new, do the new fusion
        currentDragonIndex = dragonLabel.index;

        //change the player script to the chosen dragon
        dragonGO.TryGetComponent<Dragon>(out Dragon dragonChosen);

        ///add the dragon's hp to your armor
        currentDragon = dragonChosen;
        //player.armor = dragonChosen.hp;
        //player.maxArmor = dragonChosen.maxHP;

        //change the sprite to the chosen dragon
        //playerGO.GetComponent<SpriteRenderer>().sprite = ChoosePlayerSprite(dragonChosen.DType);

        //update the HUD
        //playerHUD.UpdateHUD(player);

        StartCoroutine(SwitchDragon());
    }

    private void ChoosePlayerSprite(DragonType dType)
    {
        /*
        switch (dType)
        {
            case DragonType.FIRE:
                return fireFusedSprite;

            case DragonType.WATER:
                return waterFusedSprite;

            case DragonType.WIND:
                return windFusedSprite;

            case DragonType.EARTH:
                return earthFusedSprite;

            case DragonType.BASE:
                return baseFusedSprite;
        }
        

        //return playerSprite;
    }

    IEnumerator SwitchDragon()
    {
        yield return new WaitForSeconds(timeToWait);

        //state = BattleState.ENEMYTURN;
        StartCoroutine(OnEnemyTurn());
    }

    

    public void OnEndFight()
    {
        attackButton.interactable = false;

        if (state == BattleState.WON)
        {
            dialogueBox.text = "PLAYERWON";
            LastEnemyData.enemyDefeated = true;
        }
        else if (state == BattleState.LOST)
        {
            dialogueBox.text = "PLAYERLOST";
            //disable the battle scene/something
        }
    }    

    public void OnLeaveButton()
    {
        RestartDragonStats();
        OnFightEnd?.Invoke();
        sceneTransition.FadeTo(LevelData.mapScene);
    }

    private void RestartDragonStats()
    {
        List<List<Object>> saved = DragonsData.sortedDragonsStats;

        foreach (List<Object> dragonList in saved)
        {
            dragonList[1] = dragonList[10];
        }
    }

    */
}