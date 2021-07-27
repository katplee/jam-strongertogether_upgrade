using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/*
 * Things to decide on:
 * *when fused with a dragon, how will the attack of the player change?
 * *how will the player's attacks be initialized? there should be some correlation with the current level?
 *      (for now it is same with the enemy)
 */

public class Player : Element
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    #region Variables to sort
    private int direction, legion, dieForm;
    private bool reloading = false, dead = false, armed;
    #endregion

    #region Basic Stats
    public override ElementType Type
    {
        get { return ElementType.PLAYER; }
    }
    #endregion

    #region General Parameters
    private Rigidbody2D rb2d;
    #endregion

    #region Animation/Movement Parameters
    public float speed;
    private Animator anim;
    private float moveHorizontal;
    private float moveVertical;
    private float moveSpeed;
    #endregion

    #region Serialization Parameters
    private PlayerData playerData;
    #endregion

    public bool isChoosingTame = false;

    protected override void Awake()
    {
        base.Awake();
        SubscribeEvents();
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!dead)
        {
            Move();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeEvents();
    }

    protected override void InitializeAttributes()
    {
        //set DType
        DType = DragonType.NOTDRAGON;

        //each element's max HP is dependent on the level (temporary fix)
        float elementMaxHP = (GameManager.currLvl * 2 * hpLevelFactor) +
            Random.Range(-hpMargin, hpMargin);
        SetStatMaximum(ref maxHP, elementMaxHP);
        hp = maxHP;

        //player's max armor is initialized to 0
        SetStatMaximum(ref maxArmor, 0);
        Armor = maxArmor;

        //determine weakness
        //in the meantime, weakness is randomly generated
        //  but for the final version, we might want to establish a correlation
        //  between the element and its weakness
        //weakness will be set at the moment of instantiation
        int weaknessInd = Random.Range(0, 4);
        Weakness = (WeaknessType)weaknessInd;
    }

    protected override void InitializeAttacks()
    {
        int currentLvl = GameManager.currLvl;
        int attack = Random.Range(1, GameManager.currLvl * attackMargin + 1);

        baseAttack = ((currentLvl == GameManager.baseLevel) ? specialtyAttackMultiplier : 0) * attack;
        fireAttack = ((currentLvl == GameManager.fireLevel) ? specialtyAttackMultiplier : 0) * attack;
        waterAttack = ((currentLvl == GameManager.waterLevel) ? specialtyAttackMultiplier : 0) * attack;
        windAttack = ((currentLvl == GameManager.windLevel) ? specialtyAttackMultiplier : 0) * attack;
        earthAttack = ((currentLvl == GameManager.earthLevel) ? specialtyAttackMultiplier : 0) * attack;
    }

    public override void InitialSerialization()
    {
        //assign player to save file
        AssignPlayer();
    }

    protected override void InitializeSerialization()
    {
        playerData = new PlayerData();

        //BASIC STATS

        //if-else loop is to keep the player position set to the position before scene transitioned to the attack scene
        if(GameManager.currentSceneName == GameManager.attackScene)
        {
            playerData.position = PlayerSave.Instance.LoadPlayerData().player.position;
        }
        else
        {
            playerData.position = transform.position;
        }
        
        playerData.hp = hp;
        playerData.maxHP = maxHP;
        playerData.type = Type;
        playerData.dType = DType;
        playerData.id = GetInstanceID();
        playerData.name = name;

        //COMBAT STATS
        playerData.armor = Armor;
        playerData.maxArmor = maxArmor;
        playerData.weakness = Weakness;
        playerData.weaknessFactor = weaknessFactor;
        playerData.fireAttack = fireAttack;
        playerData.waterAttack = waterAttack;
        playerData.windAttack = windAttack;
        playerData.earthAttack = earthAttack;
        playerData.baseAttack = baseAttack;

        //DRAGON STATS        
        playerData.dragons = Inventory.Instance.Dragons;
    }

    public override void InitializeDeserialization()
    {
        playerData = PlayerSave.Instance.LoadPlayerData().player;

        //BASIC STATS
        hp = playerData.hp;
        maxHP = playerData.maxHP;
        DType = playerData.dType;
        name = playerData.name;

        //COMBAT STATS
        Armor = playerData.armor;
        maxArmor = playerData.maxArmor;
        Weakness = playerData.weakness;
        weaknessFactor = playerData.weaknessFactor;
        fireAttack = playerData.fireAttack;
        waterAttack = playerData.waterAttack;
        windAttack = playerData.windAttack;
        earthAttack = playerData.earthAttack;
        baseAttack = playerData.baseAttack;
    }

    public void AssignPlayer()
    {
        InitializeSerialization();
        PlayerSave.Instance.AssignPlayer(playerData);
        PlayerSave.Instance.SavePlayerData();
    }

    private void Move()
    {
        if (GameManager.currentSceneName == GameManager.attackScene) { return; }

        if (isChoosingTame)
        {
            if (rb2d.velocity.magnitude != 0)
            {
                rb2d.velocity = Vector2.zero;
            }
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rb2d.velocity = new Vector2(speed * h, speed * v);

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            moveHorizontal = 0;
            moveVertical = -1;
            moveSpeed = 1f;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            moveHorizontal = 0;
            moveVertical = 1;
            moveSpeed = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1;
            moveVertical = 0;
            moveSpeed = 1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1;
            moveVertical = 0;
            moveSpeed = 1f;
        }
        else
        {
            moveSpeed = 0f;
        }

        anim.SetFloat("Horizontal", moveHorizontal);
        anim.SetFloat("Vertical", moveVertical);
        anim.SetFloat("Speed", moveSpeed);
    }

    public void Reposition()
    {
        transform.position = playerData.position;
    }

    private void SubscribeEvents()
    {

    }

    private void UnsubscribeEvents()
    {

    }

    public void SetParametersOnFuse(float armor)
    {
        SetStatMaximum(ref maxArmor, Armor + armor);
        Armor = Armor + armor;
    }
    
    private void TESTPrintPlayerData()
    {
        string playerStats =
            $"//BASIC STATS\n" +
            $"hp : {playerData.hp}\n" +
            $"maxHP : {playerData.maxHP}\n" +
            $"type : {playerData.type}\n" +
            $"dType : {playerData.dType}\n" +
            $"id : {playerData.id}\n" +
            $"name : {playerData.name}\n\n" +
            $"//COMBAT STATS\n" +
            $"armor : {playerData.armor}\n" +
            $"maxArmor : {playerData.maxArmor}\n" +
            $"weakness : {playerData.weakness}\n" +
            $"weaknessFactor : {playerData.weaknessFactor}\n" +
            $"fireAttack : {playerData.fireAttack}\n" +
            $"waterAttack : {playerData.waterAttack}\n" +
            $"windAttack : {playerData.windAttack}\n" +
            $"earthAttack : {playerData.earthAttack}\n" +
            $"baseAttack : {playerData.baseAttack}\n";

        Debug.Log(playerStats);
    }
}
