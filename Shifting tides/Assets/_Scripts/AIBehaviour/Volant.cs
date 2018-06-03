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

    public Transform[] activityBounds;
    public float movementSpeed;

    protected override void InitializeLists()
    {
        base.InitializeLists();
        spontaneousBehaviours.Add("Roaming");
        patternedBehaviours.Add("AirCircling");
        patternedBehaviours.Add("Patroling");
        restingBehaviours.Add("AirRest");
        restingBehaviours.Add("GroundRest");
    }

    private void Update()
    {
        if (isPathing)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * movementSpeed);
            Vector3 targetDir = currentTarget - transform.position;
            Vector3 newVector = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 2.5f, 0);
            transform.rotation = Quaternion.LookRotation(newVector);
        }
    }

    protected IEnumerator Pathing()
    {

        isPathing = true;
        foreach (var pathPoint in path.ToList<Vector3>())
        {
            currentTarget = path.Dequeue();
            yield return new WaitUntil(() => Arrived(gameObject.transform.position, currentTarget, 0.05f));
        }
        yield return new WaitUntil(() => path.Count == 0);
        isPathing = false;
        yield break;

    }

    protected IEnumerator AirCircling()
    {
        float circleDiameter = DetermineCircleDiameter();
        float circleRadius = UnityEngine.Random.Range(circleDiameter /3, circleDiameter / 2);
        Vector3 StartOfTheCircle = transform.position;
        Vector3 EndOfTheCircle = transform.position + transform.forward * circleDiameter;
        List<Vector3> CirclePoints = new List<Vector3>();
        List<Vector3> mirroredCirclePoints = new List<Vector3>();

        CirclePoints = CreateBezierCurvedPath(StartOfTheCircle, EndOfTheCircle, transform.right,transform.forward,48, circleRadius);

        foreach (Vector3 point in CirclePoints) {

            path.Enqueue(point);
        }

        mirroredCirclePoints = CreateBezierCurvedPath(EndOfTheCircle, StartOfTheCircle, -transform.right, transform.forward, 48, circleRadius);

        foreach (Vector3 point in mirroredCirclePoints)
        {
            path.Enqueue(point);
        }
        yield return StartCoroutine(Pathing());
        StartCoroutine(FinishStandandrMovementBehaviour(10, 25));
        yield break;
    }

    private List<Vector3> CreateBezierCurvedPath(Vector3 startPointCurve, Vector3 endPointCurve, Vector3 curveDirection, Vector3 objectFowardDirection,
        float totalCurveCut,float curvePeak)
    {
        List<Vector3> path = new List<Vector3>();
        //Bezier curve
        Vector3 midPointCurve;
        midPointCurve = startPointCurve + curveDirection * curvePeak + objectFowardDirection * curvePeak;
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

    protected IEnumerator AirRest()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator Dashing()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator GroundRest()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator Patroling()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator Roaming()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }

}
