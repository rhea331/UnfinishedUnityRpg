using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Spells/RangedProjectile")]

//A basic ranged projectile spell, attacks one target and deals damage.
public class RangedProjectile : Spell {

    public GameObject rangedProjectile;
    public int power;

    public override IEnumerator Cast(GameObject attacker, GameObject[] attacktargets)
    {
        PlayerStateMachine PSM = attacker.GetComponent<PlayerStateMachine>();
        Debug.Log(PSM.player.actorName + " has used the spell " + spellName + ".");
        PSM.player.currentMP -= manaCost;
        PSM.UI.SetMP(PSM.player.currentMP, PSM.player.baseMP);
        //load prefab
        GameObject InstantiatedPrefab = Instantiate(rangedProjectile, attacker.transform);
        InstantiatedPrefab.GetComponent<SpriteRenderer>().sprite = sprite;

        //Attacks one target, currently assuming attacktargets only has one target in it.

        GameObject attacktarget = attacktargets[0];

        //set prefab to move towards enemy.
        Vector3 targetPosition = attacktarget.transform.position;
        while (InstantiatedPrefab.transform.position != targetPosition)
        {
            InstantiatedPrefab.transform.position = Vector3.MoveTowards(InstantiatedPrefab.transform.position, targetPosition, Time.deltaTime * 5);
            yield return null;
        }

        //hits enemy for damage
        float damage = power * (1+(PSM.player.magic / 100f));
        Debug.Log(damage);
        attacktarget.GetComponent<EnemyStateMachine>().Damage((int)damage, true);

        //destroy prefab

        Destroy(InstantiatedPrefab);

        attacker.GetComponent<PlayerStateMachine>().Finished();
    }

}
