using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*CanvasController
 * Controls the UI
 * Can setup Player Box with player's data, sets up buttons for attacking and using magical spells
 * Should move game logic from here to BSM later on.
 */
public class CanvasController : MonoBehaviour {

    public enum UIAction
    {
        NULL,
        ACTION,
        PHYSICALTARGET,
        MAGICSELECT,
        MAGICTARGET,
        ITEMSELECT,
        ITEMTARGET,
        FLEE
    }

    private UIAction currentUIAction = UIAction.ACTION;
    private UIAction previousUIAction = UIAction.NULL;

    public EventSystem eventsystem;
    public PlayerBoxPanel PlayerBox;
    public TargetSelectionPanel SelectionBox;
    public ItemSelectionPanel ItemSelectionBox;
    public GameObject HeroRows;
    public Text winText;

    public BattleStateMachine BSM;

    //The current active player using the UI
    private PlayerStateMachine activePlayer;
    //The playerAction that will be passed to activePlayer
    private PlayerAction playerAction;

    //Enables player box with player data
    public void EnablePlayerBox(PlayerStateMachine PSM)
    {
        activePlayer = PSM;
        playerAction = new PlayerAction();
        PlayerBox.EnablePlayerBox(PSM, this);

    }

    //Disables the player box
    public void DisablePlayerBox()
    {
        PlayerBox.DisablePlayerBox();
    }

    public void EnableHeroRows(bool enable)
    {
        HeroRows.SetActive(enable);
    }

    //Disables Selection Box
    public void DisableSelectionBox()
    {
        SelectionBox.SetSelectionBox(false);
    }

    //If the heroes win
    public void ShowWinText()
    {
        winText.text = "You Win!";
    }

    public void SetActionType(int _actionType)
    {
        PlayerBox.DisablePlayerBox();

        playerAction.actionType = (PlayerAction.ActionState)_actionType;
        previousUIAction = currentUIAction;
        switch (playerAction.actionType)
        {
            case (PlayerAction.ActionState.PHYSICAL):
                currentUIAction = UIAction.PHYSICALTARGET;
                SelectionBox.SetSelectionBox(true);
                SelectionBox.SetButtons(BSM.Enemies, this);
                break;
            case (PlayerAction.ActionState.MAGICAL):
                currentUIAction = UIAction.MAGICSELECT;
                PlayerBox.DisablePlayerBox();
                HeroRows.SetActive(false);
                ItemSelectionBox.gameObject.SetActive(true);
                ItemSelectionBox.SetButtons(activePlayer.player.Spellbook, this);
                break;
            case (PlayerAction.ActionState.ITEM):
                if (activePlayer.player.ItemBag.GetBag().Count > 0)
                {
                    currentUIAction = UIAction.ITEMSELECT;
                    PlayerBox.DisablePlayerBox();
                    HeroRows.SetActive(false);
                    ItemSelectionBox.gameObject.SetActive(true);
                    ItemSelectionBox.SetButtons(activePlayer.player.ItemBag, this);
                }
                else
                {
                    PlayerBox.EnablePlayerBox(activePlayer, this);
                }
                break;
            case (PlayerAction.ActionState.FLEE):
                currentUIAction = UIAction.FLEE;
                break;

        }
    }

    public void SetTarget(GameObject target)
    {
        BSM.DisablePlayerBoxUI();
        currentUIAction = previousUIAction = UIAction.ACTION;
        playerAction.attackTargets = new GameObject[] { target };
        activePlayer.SetAction(playerAction);
    }

    public void SetSpell(Spell _spell)
    {
        playerAction.spell = _spell;
        ItemSelectionBox.DisableButtons();
        ItemSelectionBox.gameObject.SetActive(false);

        List<GameObject> targets = new List<GameObject>();
        if (_spell.targetsAllies)
        {
            targets.AddRange(BSM.Heroes);
            targets.Remove(activePlayer.gameObject);
        }
        else
        {
            targets.AddRange(BSM.Enemies);
        }
        SelectionBox.SetSelectionBox(true);
        SelectionBox.SetButtons(targets, this);
        HeroRows.SetActive(true);
        previousUIAction = currentUIAction;
        currentUIAction = UIAction.MAGICTARGET;
    }

    public void SetItem(Item _item)
    {
        playerAction.item = _item;
        ItemSelectionBox.DisableButtons();
        ItemSelectionBox.gameObject.SetActive(false);
        List<GameObject> targets = new List<GameObject>();
        HeroRows.SetActive(true);
        switch (_item.targets)
        {
            case (Item.TargetType.SELF):
                previousUIAction = UIAction.NULL;
                currentUIAction = UIAction.ACTION;
                activePlayer.SetAction(playerAction);
                return;
            case (Item.TargetType.ALLIES):
                targets.AddRange(BSM.Heroes);
                targets.Remove(activePlayer.gameObject);
                break;
            case (Item.TargetType.ENEMIES):
                targets.AddRange(BSM.Enemies);
                break;
        }
        SelectionBox.SetSelectionBox(true);
        SelectionBox.SetButtons(targets, this);
        previousUIAction = currentUIAction;
        currentUIAction = UIAction.ITEMTARGET;
    }

    public void Back()
    {
        switch (previousUIAction)
        {
            case (UIAction.ACTION):
                ItemSelectionBox.DisableButtons();
                ItemSelectionBox.gameObject.SetActive(false);
                SelectionBox.SetSelectionBox(false);
                PlayerBox.gameObject.SetActive(true);
                HeroRows.SetActive(true);
                currentUIAction = previousUIAction;
                previousUIAction = UIAction.NULL;
                break;
            case (UIAction.MAGICSELECT):
                SelectionBox.SetSelectionBox(false);
                HeroRows.SetActive(false);
                ItemSelectionBox.gameObject.SetActive(true);
                ItemSelectionBox.SetButtons(activePlayer.player.Spellbook, this);
                currentUIAction = previousUIAction;
                previousUIAction = UIAction.ACTION;
                break;
            case (UIAction.ITEMSELECT):
                    SelectionBox.SetSelectionBox(false);
                    HeroRows.SetActive(false);
                    ItemSelectionBox.gameObject.SetActive(true);
                    ItemSelectionBox.SetButtons(activePlayer.player.ItemBag, this);
                    currentUIAction = previousUIAction;
                    previousUIAction = UIAction.ACTION;         
                break;
        }
    }
}
