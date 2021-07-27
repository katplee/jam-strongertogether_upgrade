using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformManager : MonoBehaviour
{
    private static TransformManager instance;
    public static TransformManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TransformManager>();
            }
            return instance;
        }
    }

    public Transform TPlayer { get; private set; }
    public Transform TEnemy { get; private set; }

    private void SetTPlayer(Transform TPlayer)
    {
        this.TPlayer = TPlayer;
    }

    private void SetTEnemy(Transform TEnemy)
    {
        this.TEnemy = TEnemy;
    }

    public void DeclareThis(string tag, Transform transform)
    {
        switch (tag)
        {
            case "Player":
                SetTPlayer(transform);
                break;

            case "Enemy":
                SetTEnemy(transform);
                break;
        }
    }
}
