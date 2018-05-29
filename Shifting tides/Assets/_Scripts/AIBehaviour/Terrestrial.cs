using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Terrestrial : Agent
{
    private Queue<Vector3> patrolRoute;
    private Vector3 lastPatrolPointVisted;

    protected NavMeshAgent agent;

    public Transform[] patrolPoints;
    public Text[] texts;
    public float standardSpeed, currentSpeed;

    protected override void Initialize()
    {
        base.Initialize();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = standardSpeed;
        //Initialize partrol route
        patrolRoute = new Queue<Vector3>();
        InitializeWaypoints();
    }

    protected override IEnumerator LocalUpdate()
    { 
        texts[0].text = desire[0].ToString();
        texts[1].text = desire[1].ToString();
        texts[2].text = desire[2].ToString();
        yield return StartCoroutine(base.LocalUpdate());
        yield break;
    }
    protected override void InitializeLists()
    {
        base.InitializeLists();
        spontaneousBehaviours.Add("Roaming");
        spontaneousBehaviours.Add("ChangePatrolRoute");
        patternedBehaviours.Add("Patroling");
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

    private void InitializeWaypoints() {
        for (int i = 0; i < 2; i++)
        {
            patrolRoute.Enqueue(patrolPoints[i].position);
        }
        foreach (Transform point in patrolPoints)
        {
            wayPoints.Add(point);
        }

    } 

    protected IEnumerator Patroling()
    {
        lastPatrolPointVisted = patrolRoute.Peek();
        MoveTowardsTarget(patrolRoute.Dequeue());
        patrolRoute.Enqueue(lastPatrolPointVisted);
        nearestWaypoint = lastPatrolPointVisted;
        yield return new WaitUntil(() => Arrived(gameObject.transform.position, destination, 1f));
        yield return StartCoroutine(FinishStandandrMovementBehaviour(15,25));
    }

    private IEnumerator Roaming()
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

    private IEnumerator ChangePatrolRoute()
    {
        patrolRoute.Clear();
        List<Vector3> availablePatrolPoints = new List<Vector3>();
        foreach (Transform point in patrolPoints) {

            if (lastPatrolPointVisted != point.position) {
                availablePatrolPoints.Add(point.position);
            }
        }
        patrolRoute.Enqueue(availablePatrolPoints[Random.Range(0,availablePatrolPoints.Count)]);
        patrolRoute.Enqueue(lastPatrolPointVisted);
        yield return new WaitForSeconds(0.4f);
        StandardBehaviourFinished = true;     
        yield break;
    }

    protected void MoveTowardsTarget(Vector3 currentTarget)
    {
        destination = currentTarget + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        agent.SetDestination(destination);
    }

    //protected IEnumerator FinishStandandrMovementBehaviour() {
    //    isResting = true;
    //    yield return new WaitForSeconds(Random.Range(15,25 * (1 + desire[2])));
    //    isResting = false;
    //    StandardBehaviourFinished = true;
    //    yield break;
    //}

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
