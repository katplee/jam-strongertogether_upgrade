using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public class DragonsData : MonoBehaviour
{
    public static event Action NewDragonSaved;

    public static DragonsData Instance;

    public static List<List<Object>> dragonsStats = new List<List<Object>>();
    public static List<List<Object>> sortedDragonsStats = new List<List<Object>>();
    public List<List<Object>> dragonsList = new List<List<Object>>();

    [TextArea(15, 15)]
    [SerializeField] private List<string> dragonList;

    private Scene currentScene;
    private string attackScene = "AttackScene";

    ///format of List<float>
    ///List<float> dragonStats = new List<float>()
    ///     {
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
    ///     }

    private enum Stat
    {
        dragon_type,
        hp,
        armor,
        damage_amount,
        weakness,
        weakness_factor,
        fire_attack,
        water_attack,
        wind_attack,
        earth_attack,
        maxHP
    }

    private Dictionary<Stat, int> stat = new Dictionary<Stat, int>()
    {
        { Stat.dragon_type, 0 },
        { Stat.hp, 1 },
        { Stat.armor, 2 },
        { Stat.damage_amount, 3 },
        { Stat.weakness, 4 },
        { Stat.weakness_factor, 5 },
        { Stat.fire_attack, 6 },
        { Stat.water_attack, 7 },
        { Stat.wind_attack, 8 },
        { Stat.earth_attack, 9 },
        { Stat.maxHP, 10}
    };

    private void Start()
    {
        //SceneTransition.JustAfterSceneTransition += LogDragonStats;
        NewDragonSaved += SortLogDragonStats;
        //FightManager.OnFightEnd += RefreshLogs;

        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        NewDragonSaved -= SortLogDragonStats;
        //FightManager.OnFightEnd -= RefreshLogs;
    }

    public void SaveDragon(Dragon dragon)
    {
        //create a new temporary list to save each dragon's stats
        List<Object> dragonStats = new List<Object>()
        {
            dragon.DType,
            //dragon.hp,
            //dragon.armor,
            dragon.DamageAmount(),
            //dragon.weakness,
            //dragon.weaknessFactor,
            //dragon.fireAttack,
            //dragon.waterAttack,
            //dragon.windAttack,
            //dragon.earthAttack,
            //dragon.maxHP
        };

        dragonsStats.Add(dragonStats);
        NewDragonSaved?.Invoke();
    }

    private void SortLogDragonStats()
    {
        Debug.Log("I'm invoked!");

        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == attackScene) { return; }

        if (dragonsStats.Count == 0) { return; }

        dragonList.Clear();

        var sorted = dragonsStats.OrderBy(list => list[0]).ThenByDescending(list => list[1]);
        sortedDragonsStats = sorted.ToList();

        foreach (var sort in sorted)
        {
            List<string> _sort = sort.Select(i => i.ToString()).ToList();

            string lastLoggedDragonText =
            $"{(Stat)0} : {_sort[0]}\n" +
            $"{(Stat)1} : {_sort[1]}\n" +
            $"{(Stat)2} : {_sort[2]}\n" +
            $"{(Stat)3} : {_sort[3]}\n" +
            $"{(Stat)4} : {_sort[4]}\n" +
            $"{(Stat)5} : {_sort[5]}\n" +
            $"{(Stat)6} : {_sort[6]}\n" +
            $"{(Stat)7} : {_sort[7]}\n" +
            $"{(Stat)8} : {_sort[8]}\n" +
            $"{(Stat)9} : {_sort[9]}\n" +
            $"{(Stat)10} : {_sort[10]}";


            dragonList.Add(lastLoggedDragonText);
        }

        /*
        List<Object> lastLoggedDragon = dragonsStats[dragonsStats.Count - 1];

        string lastLoggedDragonText = 
            $"{(Stat)0} : {lastLoggedDragon[0]}\n" +
            $"{(Stat)1} : {lastLoggedDragon[1]}\n" +
            $"{(Stat)2} : {lastLoggedDragon[2]}\n" +
            $"{(Stat)3} : {lastLoggedDragon[3]}\n" +
            $"{(Stat)4} : {lastLoggedDragon[4]}\n" +
            $"{(Stat)5} : {lastLoggedDragon[5]}\n" +
            $"{(Stat)6} : {lastLoggedDragon[6]}\n" +
            $"{(Stat)7} : {lastLoggedDragon[7]}\n" +
            $"{(Stat)8} : {lastLoggedDragon[8]}\n" +
            $"{(Stat)9} : {lastLoggedDragon[9]}";

        dragonList.Add(lastLoggedDragonText);
        */
    }

    private void RefreshLogs()
    {
        dragonList.Clear();

        foreach (List<Object> list in sortedDragonsStats)
        {
            string lastLoggedDragonText =
            $"{(Stat)0} : {list[0]}\n" +
            $"{(Stat)1} : {list[1]}\n" +
            $"{(Stat)2} : {list[2]}\n" +
            $"{(Stat)3} : {list[3]}\n" +
            $"{(Stat)4} : {list[4]}\n" +
            $"{(Stat)5} : {list[5]}\n" +
            $"{(Stat)6} : {list[6]}\n" +
            $"{(Stat)7} : {list[7]}\n" +
            $"{(Stat)8} : {list[8]}\n" +
            $"{(Stat)9} : {list[9]}\n" +
            $"{(Stat)10} : {list[10]}";

            dragonList.Add(lastLoggedDragonText);
        }
    }
}


