using UnityEngine;
using System.Collections;

public class BreakableSpawner : Spawner
{
    public float selfDestructDelay;

    private void OnCollisionEnter(Collision collision)
    {
        TriggerFunction();
    }

    public override void TriggerFunction()
    {
        base.TriggerFunction();
        Debug.Log("Spawned");
        Destroy(gameObject, selfDestructDelay);
    }

}
