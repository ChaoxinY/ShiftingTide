using UnityEngine;
using System.Collections;

public class TresureGuardResourceManager : HostileResourceManager
{
    protected override IEnumerator OnDeathFeedBack()
    {
        yield return StartCoroutine(OnHitDrops(minOnDeathDrop, maxOnDeathDrop));
        SpawnSourcePoint(0);
        Destroy(gameObject);
    }

}
