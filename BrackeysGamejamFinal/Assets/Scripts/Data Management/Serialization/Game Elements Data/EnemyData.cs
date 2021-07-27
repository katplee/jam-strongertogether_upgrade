using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    //BASIC STATS
    public float hp;
    public float maxHP;
    public ElementType type;
    public DragonType dType;
    public int id;
    public string name;
    public int spriteIndex;

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
}
