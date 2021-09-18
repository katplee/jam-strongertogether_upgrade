using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    private static PlayerSave instance;
    public static PlayerSave Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerSave();
            }
            return instance;
        }
    }
    public string path;

    public PlayerData player;

    public void AssignPlayer(PlayerData player)
    {
        this.player = player;
    }

    public PlayerSave LoadPlayerData()
    {
        int found = path.IndexOf("/saves/");
        int level = Int32.Parse(path.Substring(found + 7, 1));

        try
        {
            if (level != GameManager.currLvl)
            {
                throw new WrongPathException();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        PlayerSave player = SerializationManager.Load(path) as PlayerSave;

        return player;
    }

    public void SavePlayerData()
    {
        SerializationManager.Save(GameManager.currLvl.ToString(), GetType().Name, this, out path);
    }
}
