using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Volant : Agent
{
    private Vector3 targetRot;
    private Queue<Vector3> path = new Queue<Vector3>();
    private bool isPathing;

    public float movementSpeed;

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

    private void Update()
    {
   
        if (isPathing)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * movementSpeed);
            Vector3 targetDir = currentTarget - transform.position;
            Vector3 newVector = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 5f, 0);
            transform.rotation = Quaternion.LookRotation(newVector);
            
        }
    }

    private void DebugPath() {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = transform.position;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    protected IEnumerator Pathing()
    {

        isPathing = true;
        foreach (var pathPoint in path.ToList<Vector3>())
        {
            currentTarget = pathPoint;
            yield return new WaitUntil(() => Arrived(gameObject.transform.position, currentTarget, 0.5f));
           // DebugPath();
        }
        path.Clear();
        yield return new WaitUntil(() => path.Count == 0);
        isPathing = false;
        yield break;

    }
    protected IEnumerator AirRest()
    {
        overwritingBehaviour = null;
        yield break;
    }
    protected IEnumerator Dashing()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected override IEnumerator Patroling()
    {
        Vector3 StartOfTheCircle = patrolRoute.Dequeue();
        Vector3 EndOfTheCircle = patrolRoute.Dequeue();
        lastPatrolPointVisted = EndOfTheCircle;
        patrolRoute.Enqueue(StartOfTheCircle);
        patrolRoute.Enqueue(EndOfTheCircle);

        float pathDistance = Vector3.Distance(StartOfTheCircle, EndOfTheCircle);
        float shiftValue = pathDistance/2;
        List<Vector3> CirclePoints = new List<Vector3>();
        List<Vector3> mirroredCirclePoints = new List<Vector3>();
        path.Enqueue(StartOfTheCircle);
        CirclePoints = CreateBezierCurvedPath(StartOfTheCircle, EndOfTheCircle, Vector3.right, Vector3.forward, 96, pathDistance, shiftValue);
        foreach (Vector3 point in CirclePoints)
        {

            path.Enqueue(point);
        }
        mirroredCirclePoints = CreateBezierCurvedPath(EndOfTheCircle, StartOfTheCircle, -Vector3.right, -Vector3.forward, 96, pathDistance, shiftValue);
        foreach (Vector3 point in mirroredCirclePoints)
        {
            path.Enqueue(point);
        }
        yield return StartCoroutine(Pathing());
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected override IEnumerator Roaming()
    {
        path.Enqueue(wayPoints[UnityEngine.Random.Range(0, wayPoints.Count)].position);
        yield return StartCoroutine(Pathing());
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }

    private List<Vector3> CreateBezierCurvedPath(Vector3 startPointCurve, Vector3 endPointCurve, Vector3 curveDirection, Vector3 objectFowardDirection,
    float totalCurveCut, float curvePeak,float curvePeakShitValue)
    {
        List<Vector3> path = new List<Vector3>();
        //Bezier curve
        Vector3 midPointCurve;
        midPointCurve = startPointCurve + curveDirection * curvePeak - objectFowardDirection * (curvePeak- curvePeakShitValue);
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

    private float DetermineCircleDiameter()
    {
        float diameter = 40f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 40f))
        {
            diameter = UnityEngine.Random.Range(0, hit.distance);
        }

        return diameter;
    }

}
