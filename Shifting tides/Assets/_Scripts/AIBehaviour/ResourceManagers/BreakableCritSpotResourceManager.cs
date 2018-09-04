using UnityEngine;
using System.Collections;

public class BreakableCritSpotResourceManager : HostileResourceManager
{
    public bool dropsSource;
    public int SourceType;
    private HostileResourceManager linkedResourceManager;

    protected override void Initialize()
    {
        base.Initialize();
        linkedResourceManager = transform.root.GetComponent<HostileResourceManager>();
    }

    protected override IEnumerator OnDeathFeedBack()

    {
        yield return StartCoroutine(OnHitDrops(minOnDeathDrop, maxOnDeathDrop));
        if (dropsSource)
        {
            SpawnSourcePoint(SourceType);
        }
        Destroy(gameObject);
        yield break;
    }

    public override float CurrentHealth
    {
        get
        {
            return base.CurrentHealth;
        }
        set
        {
            float healthdamage = currentHealth - value;
            base.CurrentHealth = value;
            linkedResourceManager.CurrentHealth -= healthdamage;
            Debug.Log(CurrentHealth);
            Debug.Log(healthdamage);
            if (currentHealth <= 0)
            {
                StartCoroutine(OnDeathFeedBack());
            }

        }
    }
}
