using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //BASIC STATS
    public Vector3 position;
    public float hp;
    public float maxHP;
    public ElementType type;
    public DragonType dType;
    public int id;
    public string name;

    //COMBAT STATS
    public float armor;
    public float maxArmor;
    public WeaknessType weakness;
    public int weaknessFactor;
    public int fireAttack;
    public int waterAttack;
    public int windAttack;
    public int earthAttack;
    public int baseAttack;
    
    //DRAGON STATS
    public int dragons;
}
