using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing")]

public class Healing : Spell {

    public int power;

    public override IEnumerator Cast(GameObject attacker, GameObject[] attackTargets)
    {
        //have attacker perform animation
        yield return new WaitForSeconds(1f);

        //single target spell, assumes first one is target
        PlayerStateMachine PSM = attacker.GetComponent<PlayerStateMachine>();


        GameObject attackTarget = attackTargets[0];
        Debug.Log(PSM.player.actorName + " has used the spell " + spellName + " on " + attackTarget.GetComponent<PlayerStateMachine>().player.actorName + ".");

        float healing = power * (1 + (PSM.player.magic / 100f));
        attackTarget.GetComponent<PlayerStateMachine>().Heal((int)healing);

        yield return new WaitForSeconds(2f);

        attacker.GetComponent<PlayerStateMachine>().Finished();
    }

}
