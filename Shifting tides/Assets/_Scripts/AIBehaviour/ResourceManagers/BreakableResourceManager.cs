﻿using UnityEngine;
using System.Collections;

public class BreakableResourceManager : HostileResourceManager
{
    public override IEnumerator OnDeathFeedBack()
    {
        StartCoroutine(OnHitDrops(1, 2));
        SpawnSourcePoint(1);
        yield break;
    }
}
