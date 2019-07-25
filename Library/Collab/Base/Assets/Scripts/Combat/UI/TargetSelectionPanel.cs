using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionPanel: MonoBehaviour {

    public List<GameObject> buttons;

    public void SetSelectionBox(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void SetButtons(PlayerStateMachine player, List<GameObject> targets, Spell spell )
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i >= targets.Count)
            {
                buttons[i].SetActive(false);
            }
            else
            {
                buttons[i].SetActive(true);
                GameObject target = targets[i];
                buttons[i].GetComponent<Text>().text = target.GetComponent<ActorStateMachine>().ReturnActor().name;
                buttons[i].GetComponent<Button>().onClick.AddListener(() => player.MagicalAttack(target, spell));

            }
        }
    }

    public void DisableButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.SetActive(false);
        }
    }
    public List<GameObject> GetButtons()
    {
        return buttons;
    }
}
