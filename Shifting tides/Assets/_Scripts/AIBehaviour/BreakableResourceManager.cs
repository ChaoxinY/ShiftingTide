using UnityEngine;
using System.Collections;

public class BreakableResourceManager : HostileResourceManager
{

    public override void GotHit(float baseDamage)
    {
        CurrentHealth -= baseDamage;
        StartCoroutine(OnHitDrops(0, 0));      
    }

    public override void OnDeathDrops()
    {
        StartCoroutine(OnHitDrops(1, 2));
        Destroy(gameObject,0.5f);
    }
}
