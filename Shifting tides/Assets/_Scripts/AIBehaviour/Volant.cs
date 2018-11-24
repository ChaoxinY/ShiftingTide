using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Volant : Agent
{
    public float movementSpeed;
    public float medianPointOffset;
    public float curvePeakStrength;

    private Vector3 targetRot;
    private Queue<Vector3> path = new Queue<Vector3>();
    private bool isPathing;

    protected override void Initialize()
    {
        base.Initialize();
        lastPatrolPointVisted = patrolPoints[0].position;
    }

    protected override void AddBehaviours()
    {
        base.AddBehaviours();
        spontaneousBehaviours.Add("Roaming");   
        restingBehaviours.Add("AirRest");
    }

    protected override IEnumerator LocalUpdate() {
        yield return StartCoroutine(base.LocalUpdate());      
        StartCoroutine(RestingBehaviourCheck());
        yield return StartCoroutine(OverWrittingBehaviourCheck());
        yield return StartCoroutine(StandardBehaviourCheck());     
    }

    private void LateUpdate()
    {
        if (isPathing&& !isTimeStopped)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * movementSpeed);
            Vector3 targetDir = currentTarget - transform.position;
            Vector3 newVector = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 5f, 0);
            transform.rotation = Quaternion.LookRotation(newVector);
        }
    }

    protected override IEnumerator PauseOnTimeStop()
    {
        if (isTimeStopped)
        {        
            yield return new WaitUntil(() => !isTimeStopped);
        }
    }

    protected IEnumerator Pathing()
    {

        isPathing = true;
        foreach (var pathPoint in path.ToList<Vector3>())
        {
            currentTarget = pathPoint;
            yield return new WaitUntil(() => Arrived(gameObject.transform.position, currentTarget, 0.5f));
            //DebugPath();
        }
        path.Clear();
        yield return new WaitUntil(() => path.Count == 0);
        isPathing = false;
        yield break;

    }
    protected IEnumerator AirRest()
    {
        overwritingBehaviour = null;
        overwrittingBehaviourFinished = true;
        yield break;
    }
    //protected IEnumerator Dashing()
    //{
    //    StartCoroutine(FinishStandardMovementBehaviour(0, 1));
    //    yield break;
    //}
    protected override IEnumerator Patroling()
    {
        Vector3 StartOfTheCircle = patrolRoute.Dequeue();
        Vector3 EndOfTheCircle = patrolRoute.Dequeue();
        Debug.Log(StartOfTheCircle);
        Debug.Log(EndOfTheCircle);
        lastPatrolPointVisted = EndOfTheCircle;
        patrolRoute.Enqueue(patrolPoints[0].transform.position);
        patrolRoute.Enqueue(patrolPoints[1].transform.position);

        float pathDistance = Vector3.Distance(StartOfTheCircle, EndOfTheCircle);
        List<Vector3> CirclePoints = new List<Vector3>();
        List<Vector3> mirroredCirclePoints = new List<Vector3>();
        path.Enqueue(StartOfTheCircle);
        CirclePoints = CreateBezierCurvedPath(StartOfTheCircle, EndOfTheCircle, Vector3.right, Vector3.forward, 12, pathDistance, curvePeakStrength, medianPointOffset);
        foreach (Vector3 point in CirclePoints)
        {
            path.Enqueue(point);
        }
        mirroredCirclePoints = CreateBezierCurvedPath(EndOfTheCircle, StartOfTheCircle, -Vector3.right, -Vector3.forward, 12, pathDistance, curvePeakStrength, medianPointOffset);
        foreach (Vector3 point in mirroredCirclePoints)
        {
            path.Enqueue(point);
        }
        yield return StartCoroutine(Pathing());
        StartCoroutine(FinishStandardMovementBehaviour(0, 1));
        yield break;
    }
    protected override IEnumerator Roaming()
    {
        path.Enqueue(wayPoints[UnityEngine.Random.Range(0, wayPoints.Count)].position);
        yield return StartCoroutine(Pathing());
        StartCoroutine(FinishStandardMovementBehaviour(0, 1));

        yield break;
    }

    private List<Vector3> CreateBezierCurvedPath(Vector3 startPointCurve, Vector3 endPointCurve,
        Vector3 curveDirection, Vector3 objectFowardDirection,
    float totalCurveCut, float curvePeak,float curvePeakOffset,float medianPointOffset)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 midPointCurve;
        midPointCurve = startPointCurve + objectFowardDirection 
            * curvePeak * curvePeakOffset + curveDirection * - medianPointOffset;

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = midPointCurve;
        //cube.transform.localScale = new Vector3(2f, 2f, 2f);
        float CurveCut = totalCurveCut;
        for (int i = 0; i < CurveCut; i++)
        {
            Vector3 Q0 = Vector3.Lerp(startPointCurve, midPointCurve, 1f / CurveCut * i);
            Vector3 Q1 = Vector3.Lerp(midPointCurve, endPointCurve, 1f / CurveCut * i);
            Vector3 pointToAdd = Vector3.Lerp(Q0, Q1, 1f / CurveCut * i);
            path.Add(pointToAdd);
        }
        return path;
    }


    private void DebugPath()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = transform.position;
        cube.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }
}
