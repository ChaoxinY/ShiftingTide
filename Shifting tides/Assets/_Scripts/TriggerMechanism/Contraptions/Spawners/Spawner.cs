﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : TriggerBoundMechanism
{
    //public GameObject mesh;
    private int timesActivated;

    public GameObject objectToSpawn;
    public Transform positionToSpawn;
    public float spawnInterval;
    public int durabilitiy;
    public bool isReuseable, repeatTriggerFunction, breakable;

    protected override IEnumerator LocalUpdate()
    {      
        yield return StartCoroutine(base.LocalUpdate());
        yield return new WaitForSeconds(spawnInterval);
        if (repeatTriggerFunction)
        {         
            TriggerFunction();
            if (isReuseable|| breakable)
            {
                timesActivated += 1;
            }
           
        }
        if (timesActivated == durabilitiy) {

            if (breakable)
            {   
                //just for fun make the mesh explode aswell
                Destroy(gameObject);
            }
            else if(isReuseable)
            {
                boundTrigger.isBoundMechnismeActive = false;
                gameObject.SetActive(false);
            }
        }
        ShiftLocalUpdateState();
    }

    public override void TriggerFunction()
    {
        GameObject spawnObject = Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.rotation);  
    }
}
