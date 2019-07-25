using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Wolf")]
public class Wolf : Enemy {
    public override IEnumerator ChooseAction(GameObject thisGameObject,  List<GameObject> Allies, List<GameObject> Enemies)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random < 0.5f)
        {
            GameObject target = Enemies[Random.Range(0, Enemies.Count)];
            yield return Bite(thisGameObject, target);
        }
        else
        {
            GameObject target = Enemies[Random.Range(0, Enemies.Count)];
            yield return Claw(thisGameObject, target);
        }
        yield break;
    }
	

    private IEnumerator Bite(GameObject thisGameObject, GameObject target)
    {
        Debug.Log(actorName + " has used attack Bite on "+target.GetComponent<ActorStateMachine>().actor.actorName);
        Vector2 startingPosition = thisGameObject.transform.position;
        Vector2 targetPosition = target.transform.position;
        targetPosition.x -= 1.5f;
        while (new Vector2(thisGameObject.transform.position.x, thisGameObject.transform.position.y)!= targetPosition)
        {
            thisGameObject.transform.position = Vector3.MoveTowards(thisGameObject.transform.position, targetPosition, Time.deltaTime * 5);
            yield return null;
        }

        target.GetComponent<ActorStateMachine>().Damage(strength, false);

        yield return new WaitForSeconds(1f);

        while (new Vector2(thisGameObject.transform.position.x, thisGameObject.transform.position.y) != startingPosition)
        {
            thisGameObject.transform.position = Vector3.MoveTowards(thisGameObject.transform.position, startingPosition, Time.deltaTime * 5);
            yield return null;
        }

        yield break;
    }

    private IEnumerator Claw(GameObject thisGameObject, GameObject target)
    {
        Debug.Log(actorName + " has used attack Claw on " + target.GetComponent<ActorStateMachine>().actor.actorName);
        Vector2 startingPosition = thisGameObject.transform.position;
        Vector2 targetPosition = target.transform.position;
        targetPosition.x -= 1.5f;
        while (new Vector2(thisGameObject.transform.position.x, thisGameObject.transform.position.y) != targetPosition)
        {
            thisGameObject.transform.position = Vector3.MoveTowards(thisGameObject.transform.position, targetPosition, Time.deltaTime * 10);
            yield return null;
        }

        target.GetComponent<ActorStateMachine>().Damage(strength/2, false);
        yield return new WaitForSeconds(0.5f);
        target.GetComponent<ActorStateMachine>().Damage(strength/2, false);
        yield return new WaitForSeconds(1f);
        while (new Vector2(thisGameObject.transform.position.x, thisGameObject.transform.position.y) != startingPosition)
        {
            thisGameObject.transform.position = Vector3.MoveTowards(thisGameObject.transform.position, startingPosition, Time.deltaTime * 10);
            yield return null;
        }

        yield break;
    }
}
