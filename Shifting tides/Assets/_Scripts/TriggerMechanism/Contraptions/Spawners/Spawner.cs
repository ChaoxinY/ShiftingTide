﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : TriggerBoundMechanism
{   
    public GameObject objectToSpawn;
    public Transform positionToSpawn;
    public float spawnInterval;
    public int durabilitiy;
    public bool isReuseable, repeatTriggerFunction, breakable;

    //public GameObject mesh;
    private int timesActivated;

    protected override IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(base.LocalUpdate());
        if (Triggered == true)
        {
            if (!repeatTriggerFunction)
            {
                TriggerFunction();
                this.enabled = false;
            }
            yield return new WaitForSeconds(spawnInterval);

            TriggerFunction();           
            timesActivated += 1;

            if (timesActivated >= durabilitiy)
            {

                if (breakable)
                {
                    //just for fun make the mesh explode aswell
                    Destroy(gameObject);
                }
                else if (isReuseable)
                {
                    timesActivated = 0;
                    Triggered = false;
                }
            }
        }
    }

    public override void TriggerFunction()
    {
        GameObject spawnObject = Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.rotation);

    }
}
