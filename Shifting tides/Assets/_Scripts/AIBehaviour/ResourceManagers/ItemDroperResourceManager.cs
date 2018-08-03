using UnityEngine;
using System.Collections;

public class ItemDroperResourceManager : HostileResourceManager
{
    public GameObject ItemToDrop;

    public override void OnDeathDrops()
    {
        Instantiate(ItemToDrop, transform.position, Quaternion.identity);
        StartCoroutine(OnHitDrops(1, 2));
        SpawnSourcePoint(1);
    }

}
