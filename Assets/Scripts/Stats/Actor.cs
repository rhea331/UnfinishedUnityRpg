using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]

//Base entity class, base class used for all enemies/players
public class Actor: ScriptableObject, IComparable  {

    public string actorName;
    public int id;
    public int baseHP;
    public int currentHP;
    public int strength;
    public int defence;
    public int magic;
    public int spirit;
    public int speed;
    public Sprite sprite;

    public int CompareTo(object comparedEntity)
    {
        return id.CompareTo(((Actor)comparedEntity).id);
    }

    //Used when damaged, changes damage depending on type of attack
    public virtual int LowerHP(int damage, bool magical)
    {
        float calculatedDamage = 0;
        float defenceType = magical ? spirit : defence;
        calculatedDamage = ((float)damage * ((100f / (100f + defenceType))));
        currentHP -= (int)calculatedDamage;

        //Prevents negative HP
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        //Returns calculated damage, used for damage texts.
        return (int)calculatedDamage;
    }

    public virtual void Heal(int healing)
    {
        currentHP += healing;
        if (currentHP > baseHP)
        {
            currentHP = baseHP;
        }
    }
}
