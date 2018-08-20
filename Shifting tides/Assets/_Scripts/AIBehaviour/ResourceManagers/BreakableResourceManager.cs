using UnityEngine;
using System.Collections;

public class BreakableResourceManager : HostileResourceManager
{
    protected override IEnumerator OnDeathFeedBack()
    {
        yield return StartCoroutine(OnHitDrops(1, 2));
        SpawnSourcePoint(1);
        Destroy(gameObject);
        yield break;
    }
}
