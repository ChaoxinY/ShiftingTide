using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Terrestrial : Agent 
{
    public NavMeshAgent agent;
    public float standardSpeed, currentSpeed;

    public AgentAnimatorManager agentAnimatorManager;

    protected override void Initialize()
    {
        base.Initialize();
        agent = gameObject.GetComponent<NavMeshAgent>();
        //agentAnimatorManager = GetComponent<AgentAnimatorManager>();
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
        agentAnimatorManager.Moving = true;
        lastPatrolPointVisted = patrolRoute.Peek();
        MoveTowardsTarget(patrolRoute.Dequeue());
        patrolRoute.Enqueue(lastPatrolPointVisted);
        nearestWaypoint = lastPatrolPointVisted;
        yield return new WaitUntil(() => agent.remainingDistance == 0);
        Debug.Log("Arrived patrol");
        agentAnimatorManager.Moving = false;
        yield return StartCoroutine(FinishStandandrMovementBehaviour(10,20));
    }
    
    protected override IEnumerator Roaming()
    {
        agentAnimatorManager.Moving = true;
        Vector3 currentTarget = wayPoints[Random.Range(0, wayPoints.Count)].position;
        nearestWaypoint = currentTarget;
        MoveTowardsTarget(currentTarget);
        yield return new WaitUntil(() => agent.remainingDistance == 0);
        agentAnimatorManager.Moving = false;
        Debug.Log("Arrived");
        yield return StartCoroutine(FinishStandandrMovementBehaviour(10,20));
    }
    private IEnumerator Circling()
    {
        Debug.Log("Circling");
        agentAnimatorManager.Moving = true;
        MoveTowardsTarget(transform.position);      
        yield return new WaitUntil(() => agent.remainingDistance == 0);
        agentAnimatorManager.Moving = false;
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
