using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Hero")]

//Base class for a hero, derived from base entity, might make other hero classes derived from this.
public class Player : Actor {

    public int baseMP;
    public int currentMP;
    public int level;
    public int xp;

    //Holds the spells
    public List<Spell> Spellbook;
    public ItemBag ItemBag = new ItemBag();

    //May make new class instead
    public enum Class
    {
        WARRIOR,
        THIEF,
        WIZARD,
        HEALER
    }

    public Class playerClass;


}
