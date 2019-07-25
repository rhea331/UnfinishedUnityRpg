using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing")]

public class Healing : Spell {

    public int power;

    public override IEnumerator Cast(GameObject attacker, GameObject[] attackTargets)
    {
        Debug.Log("Player " + attacker.name + " has used the spell " + spellName+ ".");
        //have attacker perform animation
        yield return new WaitForSeconds(1f);
        //single target spell, assumes first one is target
        PlayerStateMachine PSM = attacker.GetComponent<PlayerStateMachine>();
        float healing = power * (1 + (PSM.player.magic / 100f));
        GameObject attackTarget = attackTargets[0];
        attackTarget.GetComponent<PlayerStateMachine>().Heal((int)healing);
        yield return new WaitForSeconds(2f);
        attacker.GetComponent<PlayerStateMachine>().Finished();
    }

}
