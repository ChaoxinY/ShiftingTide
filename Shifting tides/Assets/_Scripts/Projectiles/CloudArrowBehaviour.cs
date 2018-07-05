using UnityEngine;
using System.Collections;

public class CloudArrowBehaviour : ArrowBehaviour
{
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
            hostileResourceManager.CurrentArmor -= hostileResourceManager.maxArmor * 0.33f;
            return;
        }
        hostileResourceManager.CurrentArmor -= hostileResourceManager.maxArmor * 0.2f;
        playerTideComboManager.ResetCombo();
    }
}
