using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public abstract class Item : ScriptableObject {
    public enum TargetType
    {
        SELF,
        ALLIES,
        ENEMIES
    }
    public TargetType targets;
    public int id;
    public string itemName;
    public int totalStack;
    public int price;
    public Sprite sprite;

    public int CompareTo(object comparedEntity)
    {
        return id.CompareTo(((Item)comparedEntity).id);
    }

    public abstract IEnumerator Use(GameObject user, GameObject[] targets);
}
