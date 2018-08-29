using UnityEngine;
using System.Collections;

public class BreakableResourceManager : HostileResourceManager
{
    protected override IEnumerator OnDeathFeedBack()
    {
        yield return StartCoroutine(OnHitDrops(minOnHitDrop, maxOnHitDrop));
        //SpawnSourcePoint(minOnDeathDrop);
        Destroy(gameObject);
        yield break;
    }
}
