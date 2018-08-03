using UnityEngine;
using System.Collections;

public class CloudArrowBehaviour : ArrowBehaviour
{
    public override void ApplyArrowStageValues(int stage)
    {

        switch (stage)
        {
            case 0:
                arrowSpeed = 1f;
                baseDamage = 0;
                break;
            case 1:
                arrowSpeed = 50f;
                baseDamage += 10;
                break;
            case 2:
                arrowSpeed = 75f;
                baseDamage += 15;
                break;
            case 3:
                arrowSpeed = 100f;
                baseDamage += 35;
                break;
            case 4:
                arrowSpeed = 125f;
                baseDamage += 60;
                break;

        }
    }
    protected override void OnCollisionEnter(Collision other)
    {
        Vector3 hitSpeed = other.relativeVelocity;
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[0], hitSpeed);
        if (other.gameObject.tag == "Enemy") {
            EnemyHit(other);
        }
        SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);
    }

    protected override void EnemyHit(Collision other)
    {
        HostileResourceManager hostileResourceManager = other.gameObject.GetComponent<HostileResourceManager>();
        if (other.collider.name == "CritSpot")
        {
            playerTideComboManager.AddCombo();
            if (hostileResourceManager.CurrentArmor != 0)
            {
                hostileResourceManager.CurrentArmor -= hostileResourceManager.maxArmor * 0.33f;
                return;
            }
            hostileResourceManager.GotHitOnCritSpot(baseDamage);
            return;
        }
        playerTideComboManager.ResetCombo();
        if (hostileResourceManager.CurrentArmor != 0)
        {
            hostileResourceManager.CurrentArmor -= hostileResourceManager.maxArmor * 0.2f;
        }
        hostileResourceManager.GotHit(baseDamage);
    }
}
