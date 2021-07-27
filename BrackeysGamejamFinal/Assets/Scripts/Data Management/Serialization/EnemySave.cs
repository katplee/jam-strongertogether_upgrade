using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySave
{
    private static EnemySave instance;
    public static EnemySave Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemySave();
            }
            return instance;
        }
    }
    public string path;

    public EnemyData lastEnemy = new EnemyData();
    public List<EnemyData> enemies = new List<EnemyData>();

    public void AssignLastEnemy(EnemyData lastEnemy)
    {
        this.lastEnemy = lastEnemy;
    }

    public void PopulateEnemyList(EnemyData enemy)
    {
        enemies.Add(enemy);
    }

    public void ReplaceEnemyList(EnemyData enemy)
    {
        int index = Find(enemy);
        enemies[index] = enemy;
    }

    public void ReplaceLastEnemyList()
    {
        int index = Find(lastEnemy);
        enemies[index] = lastEnemy;
    }

    private int Find(EnemyData specificEnemy)
    {
        foreach (EnemyData enemy in enemies)
        {
            if (enemy.name == specificEnemy.name)
            {
                return enemies.IndexOf(enemy);
            }
        }

        throw new NotFoundInListException();
    }

    public EnemySave LoadEnemyData()
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

        EnemySave enemy = SerializationManager.Load(path) as EnemySave;

        return enemy;
    }

    public void RemoveEnemyList(EnemyData enemy)
    {
        int index = Find(enemy);
        enemies.RemoveAt(index);
    }

    public void SaveEnemyData()
    {
        SerializationManager.Save(GameManager.currLvl.ToString(), GetType().Name, this, out path);
    }
}
