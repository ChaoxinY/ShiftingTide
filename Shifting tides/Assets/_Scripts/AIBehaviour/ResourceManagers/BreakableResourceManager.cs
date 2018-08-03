using UnityEngine;
using System.Collections;

public class BreakableResourceManager : HostileResourceManager
{
    public override void OnDeathDrops()
    {
        StartCoroutine(OnHitDrops(1, 2));
        SpawnSourcePoint(1);
    }
}
