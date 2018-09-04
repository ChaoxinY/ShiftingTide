using UnityEngine;
using System.Collections;

public class ItemDroperResourceManager : MobResourceManager
{
    public GameObject ItemToDrop;

    protected override IEnumerator OnDeathFeedBack()
    {
        Instantiate(ItemToDrop, transform.position, Quaternion.identity);
        StartCoroutine(OnHitDrops(1, 2));
        SpawnSourcePoint(1);
        playerUi.StopTrackingStatus();
        Destroy(gameObject);
        yield break;
    }

}
