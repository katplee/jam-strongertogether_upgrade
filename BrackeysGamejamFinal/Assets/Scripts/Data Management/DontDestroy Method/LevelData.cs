using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    public static LevelData Instance;

    public string attackScene = "AttackScene";
    public static string mapScene;

    void Start()
    {
        SceneTransition.JustBeforeSceneTransition += SaveLevelData;

        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SaveLevelData();
    }

    private void SaveLevelData()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == attackScene) { return; }

        mapScene = SceneManager.GetActiveScene().name;
    }
}
