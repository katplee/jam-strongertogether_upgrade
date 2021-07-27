using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Things to do:
 * *add a script in every scene by which the level can be identified.
 *  fix the currLvl value to be equal to the level using info from aforementioned script.
 */

public class GameManager : MonoBehaviour
{
    //OnLevelFirstInstance starts the initialization of variables in the game elements script
    public static event Action OnLevelFirstInstance;
    public static event Action OnLevelNormalInstance;
    public static event Action OnLevelWin;

    public static GameManager Instance { get; private set; }

    public const int baseLevel = 1;
    public const int fireLevel = 2;
    public const int waterLevel = 3;
    public const int windLevel = 4;
    public const int earthLevel = 5;

    [Header("General")]
    private bool onMission;
    public GameObject winGameUI;
    public GameObject winLvlUI;
    public GameObject gameOverUI;

    [Header("Level Control")]
    public static int currLvl = 1;
    //this records the first instance a level is unlocked
    //if the level number is already tallied in this list,
    //then it is not the first time unlocking the level
    //and, therefore, variable initialization is not necessary
    public static List<int> levelList = new List<int>();

    #region Scene Control
    public static string currentSceneName;
    public const string attackScene = "AttackScene";
    public static Stack<string> sceneStack = new Stack<string>();
    #endregion

    private void Awake()
    {
        //ConvertToPersistentData will be invoked only once to convert this object to persistent data
        ConvertToPersistentData();
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
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

    public virtual void EndDialogue()
    {

    }

    public void EndGame()
    {
        gameOverUI.SetActive(true);
    }

    //will act as mission manager also
    public void EndMission()
    {
        winGameUI.SetActive(true);
    }

    public void LevelStart()
    {
        /*
            if(winGameUI!=null)
                winGameUI.SetActive(false);
            if(gameOverUI!=null)
                gameOverUI.SetActive(false);
            if (winLvlUI!= null)
                winLvlUI.SetActive(false);
        */

        if (currentSceneName == attackScene) { return; }

        //OnLevelFirstInstance will be invoked upon Level 1's first instantiation
        //if it is not the first instantiation, a different method will be called
        if (!OnFirstInstantiation())
        {
            OnNormalInstantiation();
        }

        //TESTING...
        EnemySave trial = SerializationManager.Load(EnemySave.Instance.path) as EnemySave;
        Debug.Log($"{trial.enemies.Count}");
    }

    private void LevelUp()
    {
        //increase the level number
        currLvl++;

        //add the level number to the finished levels list, and
        //invoke OnLevelFirstInstance
        OnFirstInstantiation();

        winLvlUI.SetActive(true);
    }

    private bool OnFirstInstantiation()
    {
        if (!levelList.Contains(currLvl))
        {
            //set a variable/call a method or event
            //this loop is mostly dedicated for instances when a player
            //     would go back to the basic scene from a mini-fight
            //     to avoid any re-initialization
            //OnLevelFirstInstance will be invoked upon the first instantiation of levels other than Level 1.
            OnLevelFirstInstance?.Invoke();
            levelList.Add(currLvl);

            return true;
        }
        return false;
    }

    private void OnNormalInstantiation()
    {
        Debug.Log("OnNormalInstantiation");
        OnLevelNormalInstance?.Invoke();
    }

    public void UpdateSceneName()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        //set current scene name
        currentSceneName = currentScene.name;

        //push the scene name to the scene stack
        sceneStack.Push(currentSceneName);

        return;
    }

    private void SubscribeEvents()
    {
        OnLevelWin += LevelUp;
    }

    private void UnsubscribeEvents()
    {
        OnLevelWin -= LevelUp;
    }
}