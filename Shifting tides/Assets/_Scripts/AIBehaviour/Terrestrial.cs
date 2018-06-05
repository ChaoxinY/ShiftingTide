using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Terrestrial : Agent 
{
    protected NavMeshAgent agent;

    public float standardSpeed, currentSpeed;

    protected override void Initialize()
    {
        base.Initialize();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = standardSpeed;
    }

    protected override void AddBehaviours()
    {
        base.AddBehaviours();
        spontaneousBehaviours.Add("Roaming");
        restingBehaviours.Add("Circling");
    }
    protected override IEnumerator PauseOnTimeStop()
    {
        if (gameMng.isTimeStoped)
        {
            RestrictVelocity();
            yield return new WaitUntil(() => !gameMng.isTimeStoped);
        }
        else if (!gameMng.isTimeStoped)
        {
            ResumeVelocity();
        }
    }

    protected override IEnumerator Patroling()
    {
        lastPatrolPointVisted = patrolRoute.Peek();
        MoveTowardsTarget(patrolRoute.Dequeue());
        patrolRoute.Enqueue(lastPatrolPointVisted);
        nearestWaypoint = lastPatrolPointVisted;
        yield return new WaitUntil(() => Arrived(gameObject.transform.position, destination, 1f));
        yield return StartCoroutine(FinishStandandrMovementBehaviour(15,25));
    }

    protected override IEnumerator Roaming()
    {
        Vector3 currentTarget = wayPoints[Random.Range(0, wayPoints.Count)].position;
        nearestWaypoint = currentTarget;
        MoveTowardsTarget(currentTarget);
        yield return new WaitUntil(() => Arrived(gameObject.transform.position, destination, 1f));
        yield return StartCoroutine(FinishStandandrMovementBehaviour(15,25));
    }
    private IEnumerator Circling()
    {
        Debug.Log(destination);
        MoveTowardsTarget(transform.position);      
        yield return new WaitUntil(() => Arrived(gameObject.transform.position, destination, 1f));
        overwritingBehaviour = null;
        yield break;
    }

    protected void MoveTowardsTarget(Vector3 currentTarget)
    {
        destination = currentTarget + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        agent.SetDestination(destination);
    }

    protected void RestrictVelocity()
    {
        currentSpeed = agent.speed;
        agent.speed = 0;
    }

    protected void ResumeVelocity()
    {
        agent.speed = currentSpeed;
    }

    protected void ResetVelocity()
    {
        agent.speed = standardSpeed;
    }

}
