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
                baseDamage = 20;
                break;
            case 2:
                arrowSpeed = 60f;
                baseDamage = 45;
                break;
            case 3:
                arrowSpeed = 80f;
                baseDamage = 70;
                break;
            case 4:
                arrowSpeed = 150f;
                baseDamage = 100;
                break;

        }
    }
    protected override void OnCollisionEnter(Collision other)
    {
        Vector3 hitSpeed = other.relativeVelocity;
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[0], hitSpeed);
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHit(other);
        }
        SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);
    }

    protected override void EnemyHit(Collision other)
    {
        Vector3 hitSpeed = other.relativeVelocity;
        HostileResourceManager hostileResourceManager = other.gameObject.GetComponent<HostileResourceManager>();
        //CritHit
        if (other.collider.name == "CritSpot")
        {
            onHitSoundSource.clip = onHitSounds[2];
            onHitSoundSource.Play();
            playerTideComboManager.AddCombo();
            SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[1], hitSpeed);
            SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[3], hitSpeed);
            playerAimModule.critHit = true;
            playerAimModule.EnemyHit = true;
            if (hostileResourceManager.CurrentArmor != 0)
            {
                hostileResourceManager.CurrentArmor -= hostileResourceManager.maxArmor * 0.33f;
                return;
            }
            hostileResourceManager.GotHitOnCritSpot(baseDamage, other.contacts[0].point, hitSpeed);
            return;
        }
        //NormalHit
        playerAimModule.EnemyHit = true;
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[0], hitSpeed);
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[2], hitSpeed);
        onHitSoundSource.clip = onHitSounds[1];
        onHitSoundSource.Play();
        if (hostileResourceManager.CurrentArmor != 0)
        {
            hostileResourceManager.CurrentArmor -= hostileResourceManager.maxArmor * 0.2f;
        }
        hostileResourceManager.GotHit(baseDamage, other.contacts[0].point, hitSpeed);
    }
}
