using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorStateMachine : MonoBehaviour {

    public Actor actor;
    public GameObject currentAttackTarget;
    public bool paused = false;
    [HideInInspector] public BattleStateMachine BSM;
    [HideInInspector] public Vector2 startingLocation;

    public abstract void UpdateCharge();

    public abstract void Damage(int damage, bool magical);

    public Actor ReturnActor()
    {
        return actor;
    }
}
