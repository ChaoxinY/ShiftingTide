using UnityEngine;
using System.Collections;

public class Initializer : TriggerBoundMechanism
{
    public GameObject gameObjectToInitialize;

    public override void OnTriggerFunction()
    {
        gameObjectToInitialize.SetActive(true);
        Destroy(gameObject);
    }

}
