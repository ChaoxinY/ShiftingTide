using UnityEngine;
using System.Collections;

public class TresureGuardResourceManager : MobResourceManager
{
    protected override IEnumerator OnDeathFeedBack()
    {
        yield return StartCoroutine(OnHitDrops(minOnDeathDrop, maxOnDeathDrop));
        SpawnSourcePoint(0);
        playerUi.StopTrackingStatus();
        Destroy(gameObject);
    }
}
