using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructor : TriggerBoundMechanism {

    public float destructionDelay;
    public bool selfDestructAfterSpawn;

    protected override IEnumerator LocalUpdate()
    {
        yield return base.LocalUpdate();
        if (selfDestructAfterSpawn) {
            Invoke("TriggerFunction", destructionDelay);
        }

        ShiftLocalUpdateState();
    }

    public override void TriggerFunction()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow") {
            TriggerFunction();
        }

    }

}
