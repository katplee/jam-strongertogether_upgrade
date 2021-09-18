using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
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

    /*
     * JUST AFTER SCENE TRANSITION:
     * This method contains the correct sequence of events to run just after a scene transition.
     */
    public void JustAfterSceneTransition()
    {
        GameManager.Instance.UpdateSceneName();

        if (GameManager.currentSceneName != GameManager.attackScene)
        {
            GameManager.Instance.LevelStart(); //will not be called at AttackScene
            SerializationCommander.Instance.A_BFromASerialization();
            return; 
        }

        if (GameManager.currentSceneName == GameManager.attackScene)
        {
            //methods are called from the battle states
            //currently empty
            //SerializationCommander.Instance.A_AFromBSerialization();
            //return;
        }
    }

    private void SubscribeEvents()
    {
        SceneTransition.JustAfterSceneTransition += JustAfterSceneTransition;
    }

    private void UnsubscribeEvents()
    {
        SceneTransition.JustAfterSceneTransition -= JustAfterSceneTransition;
    }
}
