using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BattleStateMachine
 * Game manager of the Battle Scene, takes care of overall battle condition and UI calls
 * Has two state machines, one for game logic and the other for heroes UI input.
 */
public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        END
    }

    public enum PlayerUIAction
    {
        WAIT,
        PREPARING,
        CHOOSING,
        SELECTING
    }

    // Main game logic state machine
    public PerformAction battleStates;

    //State Machine for handling player requests for UI.
    public PlayerUIAction playerUIStates;

    public PlayerStateMachine currentActivePlayer; //might remove

    //handles all entities requests to attack
    public Queue<ActorStateMachine> PerformList = new Queue<ActorStateMachine>();

    //handles all heroes requests to use UI 
    public Queue<PlayerStateMachine> HeroChoiceList = new Queue<PlayerStateMachine>();

    //List of active heroes/enemies, used for UI selection and spell targets.
    public List<GameObject> Heroes = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();

    // UI canvas
    public CanvasController canvas;

	// Use this for initialization
	void Start () {
        //Initializes a static class so entities can call later for damage text.
        DamageTextController.Initialize();

        //Grabbing all heroes and enemies.
        Heroes.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //Starting state for game logic
        battleStates = PerformAction.WAIT;

    }
	
	// Update is called once per frame
	void Update () {
        if (Enemies.Count <= 0)
        {
            battleStates = PerformAction.END;
        }

        //Main game logic state machine
        switch (battleStates)
        {
            //"Waiting State" Waiting until one of the entities wants to do an action
            case (PerformAction.WAIT):
                if (PerformList.Count > 0) 
                {
                    battleStates = PerformAction.TAKEACTION;
                }

                break;
            //Performing an action
            case (PerformAction.TAKEACTION): 

                //Grabs first waiting entity
                GameObject currentActionObject = PerformList.Dequeue().gameObject;

                //Set the Entity State Machine to ACTION. TODO: Have EnemyStateMachine and PlayerStateMachine both derive from the same class.
                if (currentActionObject.tag == "Enemy") 
                {
                    EnemyStateMachine ESM = currentActionObject.GetComponent<EnemyStateMachine>();
                    ESM.currentState = EnemyStateMachine.CurrentState.ACTION;
                }
                else if (currentActionObject.tag == "Hero")
                {
                    PlayerStateMachine PSM = currentActionObject.GetComponent<PlayerStateMachine>();
                    PSM.currentState = PlayerStateMachine.CurrentState.ACTION;
                }
                
                battleStates = PerformAction.PERFORMACTION;
 
                break;
            //Another waiting state, waiting until entity has finished action and set this state back to WAIT.
            case (PerformAction.PERFORMACTION): 

                break;
            // Battle is over
            case (PerformAction.END):
                canvas.ShowWinText();
                break;
        }

        //Player UI handling state machine
        switch (playerUIStates)
        {
            //Waiting until a hero wants to use the UI
            case (PlayerUIAction.WAIT): 
                if (HeroChoiceList.Count > 0)
                {
                    playerUIStates = PlayerUIAction.PREPARING;
                }
                break;

            //Player wants to use UI, setting up UI with player data
            case (PlayerUIAction.PREPARING): //setup boxes and selectors
                currentActivePlayer = HeroChoiceList.Dequeue();
                ShowPlayerBoxUI(currentActivePlayer);
                //Showing it is the currently active player
                currentActivePlayer.Selected(true);
                playerUIStates = PlayerUIAction.CHOOSING;
                break;
            //Player is currently awaiting input.
            case (PlayerUIAction.CHOOSING):
                break;
            case (PlayerUIAction.SELECTING):
                break;
        }
	}

    //Used when an enemy/hero wants to attack, gets put into queue
    public void CollectActions(ActorStateMachine input) 
    {
        PerformList.Enqueue(input);
    }

    //Used when a hero wants to use the UI, gets put into queue.
    public void CollectUIPlayerActions(PlayerStateMachine PSM)
    {
        HeroChoiceList.Enqueue(PSM);
    }

    //Pauses all entites, only affects them if they're in the WAIT state for now.
    public void PauseAllEntities(bool pause)
    {
        foreach (GameObject hero in Heroes)
        {
            PlayerStateMachine PSM = hero.GetComponent<PlayerStateMachine>();
            PSM.paused = pause;
        }
        foreach(GameObject enemy in Enemies)
        {
            EnemyStateMachine ESM = enemy.GetComponent<EnemyStateMachine>();
            ESM.paused = pause;
        }

    }

    //Shows the Player Box with the player data.
    public void ShowPlayerBoxUI(PlayerStateMachine PSM)
    {
        canvas.EnablePlayerBox(PSM);
    }

    //Disables the Player Box
    public void DisablePlayerBoxUI()
    {
        canvas.DisablePlayerBox();
        canvas.DisableSelectionBox();
    }

    //Removes an enemy from the list
    public void RemoveEnemy(GameObject enemy)
    {
        Enemies.Remove(enemy);
        enemy.SetActive(false);
    }
}
