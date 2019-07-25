using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : ActorStateMachine {
    public Enemy enemy;

    private bool performingAction = false;

    public enum CurrentState
    {
        PROCESSING,
        CHOOSING,
        WAITING,
        ACTION,
        DEAD,
    }

    public CurrentState currentState;

    // Use this for initialization
    void Start()
    {
        enemy.Initializie();
        actor = (Actor)enemy;
        startingLocation = transform.position;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = CurrentState.PROCESSING;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (CurrentState.PROCESSING): //Updating charge
                if (!paused) { UpdateCharge(); }
                break;
            case (CurrentState.CHOOSING): //Choosing what action to take
                //BSM.PauseAllEntities(true);
                ChooseAction();
                currentState = CurrentState.WAITING;
                break;
            case (CurrentState.WAITING): //Waiting for main Battle State Machine to start action
                break;
            case (CurrentState.ACTION): //Performing action
                BSM.PauseAllEntities(true);
                StartCoroutine(PerformingAction());
                break;
            case (CurrentState.DEAD):
                BSM.RemoveEnemy(this.gameObject);
                break;
        }
    }

    public void UpdateCharge()
    {
        currentCoolDown += enemy.speed * Time.deltaTime;
        if (currentCoolDown >= maxCoolDown)
        {
            currentState = CurrentState.CHOOSING;
        }
    }

    public void ChooseAction()
    {
        BSM.CollectActions(this);

    }

    private IEnumerator PerformingAction()
    {
        if (performingAction)
        {
            yield break;
        }

        performingAction = true;

        yield return enemy.ChooseAction(this.gameObject, BSM.Enemies, BSM.Heroes);

        performingAction = false;
        currentCoolDown = 0;
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        BSM.PauseAllEntities(false);
        currentState = CurrentState.PROCESSING;

    }

    public override void Damage(int damage, bool magical)
    {
        Debug.Log(enemy.actorName + " has been hit");
        int calculatedDamage =enemy.LowerHP(damage, magical);
        DamageTextController.CreateDamageText(calculatedDamage, transform.position.x, transform.position.y+1f);
        if (enemy.currentHP <= 0){ currentState = CurrentState.DEAD;}
    }
}
