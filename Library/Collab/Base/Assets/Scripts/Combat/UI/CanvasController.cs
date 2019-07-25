using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*CanvasController
 * Controls the UI
 * Can setup Player Box with player's data, sets up buttons for attacking and using magical spells
 * Should move game logic from here to BSM later on.
 */
public class CanvasController : MonoBehaviour {

    public PlayerBoxController PlayerBox;
    public SelectionBoxController SelectionBox;
    public ItemSelectionController ItemSelectionBox;
    public GameObject HeroRows;
    public Text winText;
    public Spell activeSpell;

    public BattleStateMachine BSM;
    //The current active player using the UI
    private PlayerStateMachine activePlayer;

    //Enables player box with player data
    public void EnablePlayerBox(PlayerStateMachine PSM)
    {
        activePlayer = PSM;
        PlayerBox.EnablePlayerBox(PSM, this);
    }

    //Disables the player box
    public void DisablePlayerBox()
    {
        activePlayer = null;
        PlayerBox.DisablePlayerBox();
        PlayerBox.RemoveListeners();
    }

    public void EnableHeroRows(bool enable)
    {
        HeroRows.SetActive(enable);
    }

    //Disables Selection Box
    public void DisableSelectionBox()
    {
        SelectionBox.SetSelectionBox(false);
        SelectionBox.DisableButtons();
    }

    //Physical attack button, will setup selection box and buttons.
    public void PhysicalAttackSelection()
    {
        PlayerBox.DisablePlayerBox();
        SelectionBox.SetSelectionBox(true);
        List<GameObject> buttons = SelectionBox.GetButtons();
        List<GameObject> enemies = BSM.Enemies;
        //Currently 4 butons only, will need to change this if more enemies are present
        for (int i = 0; i < buttons.Count; i++)
        {
            //Each button is activated and setup with the enemy name
            if (i < enemies.Count)
            {
                buttons[i].SetActive(true);
                buttons[i].GetComponent<Text>().text = enemies[i].GetComponent<EnemyStateMachine>().enemy.name;
                GameObject enemy = enemies[i]; //because for some reason putting it underneath doesn't work               
                buttons[i].GetComponent<Button>().onClick.AddListener(delegate { activePlayer.PhysicalAttack(enemy); });
            }
            else
            {
                buttons[i].SetActive(false);
            }

        }
    }

    //Magical attack, currently only uses the first spell of the spellbook, otherwise identical to physical attack selection.
    //Could possibly combine the two functions and have a bool.
    public void MagicAttackSelection()
    {
        PlayerBox.DisablePlayerBox();
        HeroRows.SetActive(false);
        ItemSelectionBox.gameObject.SetActive(true);
        ItemSelectionBox.SetButtons(activePlayer.player.Spellbook, this);
    }



    //When the player wants to use an item
    public void ItemsSelection()
    {

    }

    //When the player wants to flee
    public void FleeSelection()
    {

    }

    //If the heroes win
    public void ShowWinText()
    {
        winText.text = "You Win!";
    }

    public void SetSpell(Spell spell)
    {
        activeSpell = spell;
        ItemSelectionBox.DisableButtons();
        ItemSelectionBox.gameObject.SetActive(false);

        SelectionBox.SetSelectionBox(true);
        SelectionBox.SetButtons(activePlayer, BSM.Enemies, activeSpell);
        HeroRows.SetActive(true);
    }
}
