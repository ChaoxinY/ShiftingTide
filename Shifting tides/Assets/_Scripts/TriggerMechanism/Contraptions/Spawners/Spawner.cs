using System.Collections;
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
                MechanismFunction();
                this.enabled = false;
            }
            yield return new WaitForSeconds(spawnInterval);

            yield return StartCoroutine(MechanismFunction());           
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

    public override IEnumerator MechanismFunction()
    {
        Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.rotation);
        yield break;
    }
}
