using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/HealingItem")]

public class HealingItem : Item
{
    public int power;
    public override IEnumerator Use(GameObject user, GameObject[] targets)
    {
        yield return new WaitForSeconds(1f);
        PlayerStateMachine PSM = user.GetComponent<PlayerStateMachine>();
        PSM.Heal(power);
        yield return new WaitForSeconds(1f);
        user.GetComponent<PlayerStateMachine>().Finished();
    }
}
