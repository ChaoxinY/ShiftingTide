using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceSpawner : Spawner
{
    public int leftBound;
    public int rightBound;
    public bool isCollideSpawner;
    public int maxPointsOut;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow" && isCollideSpawner && !MaxSpawnedPointsReached())
        {
            Vector3 spawnPosition = collision.contacts[0].point - Vector3.one;
            Vector3 endPosition = collision.contacts[0].point - collision.relativeVelocity / 5;
            SpawnSourcePoint(spawnPosition, endPosition);
        }
    }
    public bool MaxSpawnedPointsReached()
    {
        bool maxReached = false;
        Component[] childSources = gameObject.GetComponentsInChildren(typeof(SourcePoint));
        if (childSources.Length == maxPointsOut)
        {
            maxReached = true;
        }
        return maxReached;
    }
   
    public override void TriggerFunction()
    {
        if (!isCollideSpawner && !MaxSpawnedPointsReached())
            SpawnSourcePoint(transform.position, positionToSpawn.position);
    }

    protected override void Initialize()
    {
        base.Initialize();
        Triggered = true;
    }

    private void SpawnSourcePoint(Vector3 spawnPosition, Vector3 endPosition)
    {
        GameObject sourcePoint = Instantiate(Resources.Load("Prefabs/Source") as GameObject, spawnPosition, Quaternion.identity);
        sourcePoint.GetComponent<SourcePoint>().OnSpawnInit(rightBound, endPosition, 1, leftBound);
        sourcePoint.transform.SetParent(gameObject.transform);
    }

}


