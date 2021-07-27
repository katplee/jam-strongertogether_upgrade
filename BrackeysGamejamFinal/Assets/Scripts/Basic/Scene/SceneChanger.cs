using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string main;
    public string firstLvl;
    public SceneTransition sceneTransition;
    public bool regularLvl = false;


    public void Play()
    {
        sceneTransition.FadeTo(firstLvl);
    }

    public void MainMenu()
    {
        sceneTransition.FadeTo(main);
    }

    public void Exit()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }


    ///PUASE THINGS
    ///
    public GameObject ui;    

    //public GameManager gm;
    private bool inPause;

    void Update()
    {
        if (regularLvl)
        {
            if (inPause)
                return;
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                //Debug.Log("Pressing escape!");
                Toggle();
            }
        }        
    }
    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        if (ui.activeSelf)
        {
            //Debug.Log("Supposedly freezing");
            inPause = true;
            Time.timeScale = 0f;//freezing!!
        }
        else
        {
            //Debug.Log("Supposedly NOT freezing");
            inPause = false;
            Time.timeScale = 1f;//Not Freezing, normal timescale!
        }

    }
    public void Retry()
    {
        Toggle();
        //WaveSpawner.EnemiesAlive = 0;
        sceneTransition.FadeTo(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reloads active scene!! uses buildIndex to change from scene to scene
    }
}
