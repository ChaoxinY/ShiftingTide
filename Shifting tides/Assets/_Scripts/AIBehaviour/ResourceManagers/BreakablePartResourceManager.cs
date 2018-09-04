using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BreakablePartResourceManager : BreakableResourceManager
{
    public List<GameObject> gameObjectsToInitialize = new List<GameObject>();

    protected override IEnumerator OnDeathFeedBack()
    {
        yield return StartCoroutine(OnHitDrops(minOnDeathDrop, maxOnDeathDrop));
        if (dropsSource)
        {
            SpawnSourcePoint(SourceType);
        }
        playerUi.StopTrackingStatus();
        foreach (GameObject gameObject in gameObjectsToInitialize) {
            gameObject.SetActive(true);
        }
        Destroy(gameObject);
        yield break;
    }

}
