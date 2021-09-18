using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DragonData
{
    //BASIC STATS
    public bool isTame;
    public float hp;
    public float maxHP;
    public float xp;
    public float maxXP;
    public ElementType type;
    public DragonType dType;
    public int id;
    public string name;

    //COMBAT STATS
    public float armor;
    public float maxArmor;
    public WeaknessType weakness;
    public int weaknessFactor;
    public float dragonImmunity;
    public int fireAttack;
    public int waterAttack;
    public int windAttack;
    public int earthAttack;
    public int baseAttack;
}
