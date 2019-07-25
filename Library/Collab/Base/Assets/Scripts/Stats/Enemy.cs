using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Actor {

    public abstract IEnumerator ChooseAction(GameObject thisGameObject, List<GameObject> Allies, List<GameObject> Enemies);
}
