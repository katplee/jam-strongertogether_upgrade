using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    private static PrefabManager instance;
    public static PrefabManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PrefabManager>();
            }
            return instance;
        }
    }

    public GameObject PPlayer { get; private set; }
    public GameObject PEnemy { get; private set; }

    private void Start()
    {
        Debug.Log("All prefabs setting...");

        PPlayer = Resources.Load("Prefabs/Player") as GameObject;
        
        PEnemy = Resources.Load("Prefabs/Enemy") as GameObject;
    }
}
