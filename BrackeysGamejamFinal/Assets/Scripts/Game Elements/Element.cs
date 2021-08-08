using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Things to decide on:
 * *what will the weakness factor be equal to? (a variable in the TakeDamage method)
*/

[System.Serializable]
public enum ElementType
{
    GOLEM, BOSS, DRAGON, ENEMY, PLAYER
}

[System.Serializable]
public enum WeaknessType
{
    FIRE, WATER, WIND, EARTH, NOTCONSIDERED
}

public abstract class Element : MonoBehaviour
{
    #region Basic Stats
    protected float hp;
    protected float maxHP = 0f;
    protected int hpMargin = 3;
    protected float hpLevelFactor = 10f;
    public abstract ElementType Type { get; }
    public virtual DragonType DType { get; protected set; }
    #endregion

    #region Combat Stats
    public virtual float Armor { get; protected set; } //dragons do not have armor because they become the armor
    protected float maxArmor = 0f;
    protected int armorFactor = 5;
    public WeaknessType Weakness { get; protected set; }
    protected int weaknessFactor = 2; //the factor by which a dragon's attack will be multiplied
    protected int specialtyAttack = 15; //the value to which a dragon's specialty attack will be set to

    protected int attackMargin = 5;
    protected int specialtyAttackMultiplier = 2; //the multiplier with which to multiply the basic attacks to get the speciality attack
    //pertains to the amount of damage an element could cause
    //for example, if the fire attack is 3, the element could inflict 3 more damage points
    protected int fireAttack;
    protected int waterAttack;
    protected int windAttack;
    protected int earthAttack;
    protected int baseAttack;
    #endregion

    #region Miscellaneous Stats
    protected float remainder; //used in the computation of armor/hp to subtract
    //protected float damageAmount; //for testing only, because this will be computed
    #endregion

    protected virtual void Awake()
    {
        SubscribeEvents();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeEvents();
    }

    protected virtual void Initialization()
    {
        InitializeAttributes();
        InitializeAttacks();
        InitialSerialization();
    }

    protected virtual void InitializeAttributes()
    {
        //set DType
        DType = DragonType.NOTDRAGON;

        //each element's max HP is dependent on the level (temporary fix)
        float elementMaxHP = (GameManager.currLvl * hpLevelFactor) +
            Random.Range(-hpMargin, hpMargin);
        SetStatMaximum(ref maxHP, elementMaxHP);
        hp = maxHP;

        //each element's max armor is dependent on the level (temporary fix)
        float elementMaxArmor = Random.Range(GameManager.currLvl * armorFactor,
            GameManager.currLvl * armorFactor - armorFactor);
        SetStatMaximum(ref maxArmor, elementMaxArmor);
        Armor = maxArmor;

        //determine weakness
        //in the meantime, weakness is randomly generated
        //  but for the final version, we might want to establish a correlation
        //  between the element and its weakness
        //weakness will be set at the moment of instantiation
        int weaknessInd = Random.Range(0, 4);
        Weakness = (WeaknessType)weaknessInd;
    }

    protected abstract void InitializeAttacks();

    public abstract void InitialSerialization();

    protected abstract void InitializeSerialization();

    public abstract void InitializeDeserialization();

    protected void SetStatMaximum(ref float maxStat, float newMaxStat)
    {
        maxStat = Mathf.Max(maxStat, newMaxStat);
    }

    private void SubscribeEvents()
    {
        GameManager.OnLevelFirstInstance += Initialization;
        GameManager.OnLevelNormalInstance += InitializeSerialization;
    }

    private void UnsubscribeEvents()
    {
        GameManager.OnLevelFirstInstance -= Initialization;
        GameManager.OnLevelNormalInstance -= InitializeSerialization;
    }

    /*
     * This method will be called when:
     * METHOD CALLER: not a dragon | ELEMENT WHOSE TAKEDAMAGE METHOD IS CALLED: not a dragon
     *      call format: TakeDamage(float damageAmount)
     * METHOD CALLER: not a dragon | ELEMENT WHOSE TAKEDAMAGE METHOD IS CALLED: dragon
     *      received format: TakeDamage(float effDamageAmount)
     * METHOD CALLER: dragon/player fused with a dragon | ELEMENT WHOSE TAKEDAMAGE METHOD IS CALLED: not a dragon
     *      received format: TakeDamage(float damageAmount)     
     */
    //called to take damage by the enemy
    public virtual bool TakeDamage(float damageAmount, WeaknessType enemyWeakness = WeaknessType.NOTCONSIDERED, GameObject enemyGO = null)
    {
        //the armor gets damaged first!
        if (Armor != 0)
        {
            //subtract the damage from the armor
            remainder = Armor - damageAmount;
            Armor = Mathf.Max(Armor - damageAmount, 0);

            //if there is positive remainder (meaning there is still armor), return the element is not dead
            if (remainder > 0) { return false; }

            //if there is negative remainder
            else if (remainder < 0)
            {
                //subtract the remainder from the health
                hp = Mathf.Max((hp - Mathf.Abs(remainder)), 0);
            }
        }
        //if there is no more armor
        else
        {
            //if the hp is already zero, return the element is dead
            if (hp == 0) { return true; }

            //if not, then subtract the damage from the hp
            hp = Mathf.Max(hp - damageAmount, 0);
        }

        //if there is 0 remainder from the armor, check if hp is not depleted
        //if the hp was subtracted damage from
        //if the hp is not zero at the time of attack

        //if the hp is not depleted after subtracting, return the element is not dead
        if (hp != 0) { return false; }

        //else return the element is dead
        return true;
    }

    /*
     * This method will be called when:
     * METHOD CALLER: dragon/player fused with a dragon | ELEMENT WHOSE TAKEDAMAGE METHOD IS CALLED: not a dragon
     *      call format: TakeDamage(float damageAmount, DragonType callerDType)
     * METHOD CALLER: dragon/player fused with a dragon | ELEMENT WHOSE TAKEDAMAGE METHOD IS CALLED: dragon
     *      received format: TakeDamage(float effDamageAmount, DragonType callerDType)
     *      
     */
    public bool TakeDamage(float damageAmount, DragonType dragonType)
    {
        /*OPTIONAL CODE:
         * I added this because maybe we would want to increase the effect of the damage
         * if the weakness of the enemy is the strength/kind of the dragon that attacks.
         * For example:
         *      ENEMY'S WEAKNESS: FIRE
         *      DRAGON TYPE: FIRE
         *      When the dragon attacks, the effect will be multiplied by 2 or something.
         *      And when the enemy attacks, since it's its weakness and the dragon's strength, the effect is diminished to half.
         */

        if (dragonType.ToString() == Weakness.ToString())
        {
            return TakeDamage(damageAmount * weaknessFactor);
        }
        else
        {
            return TakeDamage(damageAmount);
        }
    }

    //returns the amount of damage the element could inflict at any given time
    //may be used together with the TakeDamage method
    public float DamageAmount()
    {
        float damageAmount = baseAttack + fireAttack + waterAttack + windAttack + earthAttack;

        return damageAmount;
    }

    public float NormalHP(out string valueString)
    {
        if (maxHP == 0)
        {
            valueString = "0/0";
            return 0;
        }

        float normalHP = hp / maxHP;
        valueString = $"{hp}/{maxHP}";
        return normalHP;
    }

    public float NormalArmor(out string valueString)
    {
        if(maxArmor == 0) 
        {
            valueString = "0/0";
            return 0;
        }

        float normalArmor = Armor / maxArmor;
        valueString = $"{Armor}/{maxArmor}";
        return normalArmor;
    }
}
