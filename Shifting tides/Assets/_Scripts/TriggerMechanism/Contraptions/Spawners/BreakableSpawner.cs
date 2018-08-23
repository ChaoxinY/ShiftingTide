using UnityEngine;
using System.Collections;

public class BreakableSpawner : Spawner
{
    public float selfDestructDelay;

    private void OnCollisionEnter(Collision collision)
    {
        MechanismFunction();
    }

    public override void MechanismFunction()
    {
        base.MechanismFunction();
        Debug.Log("Spawned");
        Destroy(gameObject, selfDestructDelay);
    }

}
