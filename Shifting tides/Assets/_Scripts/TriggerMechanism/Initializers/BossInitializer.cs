using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossInitializer : AgentInitializer
{
    public BossResourceManager bossResourceManager;

    public override IEnumerator OnTriggerFunction()
    {
        bossResourceManager.enabled = true;
        yield return StartCoroutine(InitializeObject());
        Destroy(gameObject);
        yield break;
    }
}