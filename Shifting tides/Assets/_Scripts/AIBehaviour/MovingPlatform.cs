using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : StandardInteractiveGameObject
{
    public float movementSpeed;
    public List<Transform> rotationPoints = new List<Transform>();
    public Queue<Vector3> rotationPath = new Queue<Vector3>();
    private Vector3 currentPoint;

    protected override void Initialize()
    {
        base.Initialize();
        foreach (Transform rotationPoint in rotationPoints) {
            rotationPath.Enqueue(rotationPoint.position);
        }
        SetUpPath();
    }

    private void Update()
    {
        if (!isTimeStopped)
        {
            transform.position = Vector3.MoveTowards(transform.position,currentPoint,Time.deltaTime*movementSpeed);
            if (transform.position == currentPoint) {
                SetUpPath();
            }
        }
    }

    private void SetUpPath() {
        Vector3 pathPoint = rotationPath.Dequeue();
        currentPoint = pathPoint;
        rotationPath.Enqueue(currentPoint);
    }
}
