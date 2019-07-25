using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorStateMachine : MonoBehaviour {

    [HideInInspector] public Actor actor;
    [HideInInspector] public BattleStateMachine BSM;
    [HideInInspector] public Vector2 startingLocation;

    public float currentCoolDown;
    public int maxCoolDown;

    public GameObject currentAttackTarget;

    public bool paused = false;


    public abstract void Damage(int damage, bool magical);

    public virtual void Heal(int healing)
    {
        actor.Heal(healing);
    }
}
