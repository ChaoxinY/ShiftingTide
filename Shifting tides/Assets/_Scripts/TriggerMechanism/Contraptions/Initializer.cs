using UnityEngine;
using System.Collections;

public class Initializer : TriggerBoundMechanism
{
    public GameObject gameObjectToInitialize;

    public override IEnumerator OnTriggerFunction()
    {
        gameObjectToInitialize.SetActive(true);
        Destroy(gameObject);
        yield break;
    }

}
