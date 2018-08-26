using UnityEngine;
using System.Collections;

public class BreakableSpawner : Spawner
{
    public float selfDestructDelay;

    private void OnCollisionEnter(Collision collision)
    {
       StartCoroutine(MechanismFunction());
    }

    public override IEnumerator MechanismFunction()
    {
        yield return StartCoroutine(base.MechanismFunction());
        Destroy(gameObject, selfDestructDelay);
        yield break;
    }

}
