using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Class that holds data for what attack a player is going to do
public class PlayerAction {
    public enum ActionState
    {
        PHYSICAL,
        MAGICAL,
        ITEM,
        FLEE
    }

    public ActionState actionType;
    public GameObject[] attackTargets;
    public Spell spell;
    public Item item;
}
