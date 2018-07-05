using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Terrestrial : Agent 
{
    public NavMeshAgent agent;

    public float standardSpeed, currentSpeed;

    protected override void Initialize()
    {
        base.Initialize();
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = standardSpeed;
    }

    protected override void AddBehaviours()
    {
        base.AddBehaviours();
        restingBehaviours.Add("Circling");
    }
    protected override IEnumerator PauseOnTimeStop()
    {
        if (isTimeStopped)
        {
            RestrictVelocity();
            yield return new WaitUntil(() => !isTimeStopped);
        }
        else if (!isTimeStopped)
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
        yield return new WaitUntil(() => Arrived(gameObject.transform.position, destination, 2f));
        Debug.Log("Arrived patrol");
        yield return StartCoroutine(FinishStandandrMovementBehaviour(10,20));
    }
    
    protected override IEnumerator Roaming()
    {
        Vector3 currentTarget = wayPoints[Random.Range(0, wayPoints.Count)].position;
        nearestWaypoint = currentTarget;
        MoveTowardsTarget(currentTarget);
        yield return new WaitUntil(() => Arrived(gameObject.transform.position, destination, 2f));
        Debug.Log("Arrived");
        yield return StartCoroutine(FinishStandandrMovementBehaviour(10,20));
    }
    private IEnumerator Circling()
    {
        Debug.Log("Circling");
        MoveTowardsTarget(transform.position);      
        yield return new WaitUntil(() => Arrived(gameObject.transform.position, destination, 0.5f));
        overwritingBehaviour = null;
        yield break;
    }

    protected void MoveTowardsTarget(Vector3 currentTarget)
    {
        destination = currentTarget + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        Debug.Log(destination);
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
