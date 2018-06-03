using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructor : TriggerBoundMechanism {

    public float destructionDelay;
    public bool selfDestructAfterSpawn;

    public override void TriggerFunction()
    {
           StartCoroutine(SelfDestruct());
    }

    protected IEnumerator SelfDestruct() {     
        yield return StartCoroutine(PauseOnTimeStop());
        Destroy(gameObject, destructionDelay);
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow") {
            TriggerFunction();
        }

    }

}
