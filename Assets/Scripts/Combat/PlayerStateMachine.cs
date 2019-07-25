using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* PlayerStateMachine
 * Handles the Heroes game logic,
 * such as when it wants to use the UI,
 * or when it wants to attack.
 */
public class PlayerStateMachine : ActorStateMachine {

    public Player player;

    private PlayerAction playerAction;

    //The specific part of the UI that shows the name, HP, MP and charge
    public HeroRow UI;

    public enum CurrentState
    {
        PROCESSING,
        WAITING,
        ACTIVE,
        ACTION,
        PERFORMING,
        DEAD,
    }

    public enum ActionState
    {
        PHYSICAL,
        MAGIC,
        ITEM
    }
    //State machine for game logic

    public CurrentState currentState;

    //State machine for what it wants to do on its action.
    public ActionState actionState;

	// Use this for initialization
	void Start () {
        actor = (Actor)player;
        startingLocation = transform.position;
        //Initializes UI with actor data.
        UI.SetName(player.name);
        UI.SetHP(player.currentHP, player.baseHP);
        UI.SetMP(player.currentMP, player.baseMP);
        //Finds BattleStateMachine
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = CurrentState.PROCESSING;
    }
	
	// Update is called once per frame
	void Update () {
        //actor game logic state machine
        switch (currentState)
        {
            //Increases current charge
            case (CurrentState.PROCESSING):
                if (!paused)
                {
                    UpdateCharge();
                }                

                break;
            //Waiting State, waiting for an input from the player
            case (CurrentState.WAITING):

                break;
            //Waiting State, waiting until BSM starts action. Might remove.
            case (CurrentState.ACTIVE):

                break;
           //Hero is doing an action
            case (CurrentState.ACTION):
                BSM.PauseAllEntities(true);
                switch (playerAction.actionType)
                {
                    case (PlayerAction.ActionState.PHYSICAL):
                        StartCoroutine(PerformingPhysicalAction(playerAction.attackTargets[0]));
                        break;
                    case (PlayerAction.ActionState.MAGICAL):
                        StartCoroutine(playerAction.spell.Cast(this.gameObject, playerAction.attackTargets));
                        break;
                    case (PlayerAction.ActionState.ITEM):
                        StartCoroutine(playerAction.item.Use(this.gameObject, playerAction.attackTargets));
                        //player.ItemBag.Remove(playerAction.item);
                        break;
                    case (PlayerAction.ActionState.FLEE):
                        break;
                }
                currentState = CurrentState.PERFORMING;
                break;
            //Hero is performing an action, waiting
            case (CurrentState.PERFORMING):
                break;
            //Hero is dead, rip
            case (CurrentState.DEAD):
                break;

        }

	}

    //Increases the charge of the actor, if it reaches max, changes state.
    public void UpdateCharge()
    {
        currentCoolDown += player.speed * Time.deltaTime;
        UI.SetCharge(currentCoolDown, maxCoolDown);
        if (currentCoolDown >= maxCoolDown)
        {
            //Queue up for using UI.
            BSM.CollectUIPlayerActions(this);
            currentState = CurrentState.WAITING;
        }
    }

    //Coroutine for physical attack, will move towards attack target, deal damage, then move back
    private IEnumerator PerformingPhysicalAction(GameObject enemy)
    {
        Debug.Log(player.actorName + " is doing a physical attack.");

        //Moves towards target
        Vector2 target = enemy.transform.position;
        target.x += 1.5f;

        while (new Vector2(transform.position.x, transform.position.y) != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 5);
            yield return null;
        }

        //Attacks target
        Debug.Log(player.actorName + " has attacked " + enemy.GetComponent<ActorStateMachine>().actor.actorName);

        //Deals physical damage to target.
        enemy.GetComponent<EnemyStateMachine>().Damage(player.strength, false);

        yield return new WaitForSeconds(1f);

        //Moves back to starting position.
        while (new Vector2(transform.position.x, transform.position.y) != startingLocation)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingLocation, Time.deltaTime * 5);
            yield return null;
        }

        Finished();

    }


    public void SetAction(PlayerAction _playerAction)
    {
        Selected(false);
        playerAction = _playerAction;
        BSM.playerUIStates = BattleStateMachine.PlayerUIAction.WAIT;
        BSM.CollectActions(this);
        currentState = CurrentState.ACTIVE;
    }

    //Used when the actor is damaged
    public override void Damage(int damage, bool magical)
    {
        //Damage might change from actor's defences/spirit/buffs/debuffs.
        int calculatedDamage = player.LowerHP(damage, magical);
        if (player.currentHP <=0) { currentState = CurrentState.DEAD; }
        UI.SetHP(player.currentHP, player.baseHP);
        DamageTextController.CreateDamageText(calculatedDamage, transform.position.x, transform.position.y+1f);
    }

    public override void Heal(int healing)
    {
        player.Heal(healing);
        UI.SetHP(player.currentHP, player.baseHP);
        DamageTextController.CreateDamageText(-healing, transform.position.x, transform.position.y+1f);
    }

    //Shows if actor is selected or not.
    public void Selected(bool active)
    {
        this.gameObject.transform.Find("Selector").gameObject.SetActive(active);
    }

    public void Finished()
    {
        BSM.PauseAllEntities(false);
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        currentState = CurrentState.PROCESSING;

        currentCoolDown = 0;
    }
}
