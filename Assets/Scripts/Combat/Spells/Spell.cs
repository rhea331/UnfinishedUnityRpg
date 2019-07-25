using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//Base abstract class for spells
public abstract class Spell : ScriptableObject{

    public string spellName;
    public int id;

    public int manaCost;
    public int totalTargets; //total amount of targets
    public bool targetsAllies; //if it targets allies(t) or enemies(f)
    public Sprite sprite;

    //Override for when an entity casts a spell.
    public abstract IEnumerator Cast(GameObject attacker, GameObject[] attackTargets);

}
