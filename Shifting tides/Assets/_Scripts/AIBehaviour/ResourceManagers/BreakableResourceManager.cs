using UnityEngine;
using System.Collections;

public class BreakableResourceManager : MobResourceManager
{
    public bool dropsSource;
    public int SourceType;

    protected override IEnumerator OnDeathFeedBack()
    {
        yield return StartCoroutine(OnHitDrops(minOnDeathDrop, maxOnDeathDrop));
        if (dropsSource)
        {
            SpawnSourcePoint(SourceType);
        }
        playerUi.StopTrackingStatus();
        Destroy(gameObject);
        yield break;
    }
}
