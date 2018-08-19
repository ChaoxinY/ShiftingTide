using UnityEngine;
using System.Collections;
using System;

public class TimeZoneArrowBehaviour : ArrowBehaviour
{
    public GameObject TimeStopZone;

    public override void ApplyArrowStageValues(int stage)
    {
        switch (stage)
        {
            case 0:
                arrowSpeed = 1f;              
                break;
            case 1:
                arrowSpeed = 50f;
                break;
            case 2:
                arrowSpeed = 60f;
                break;
            case 3:
                arrowSpeed = 80f;
                break;
            case 4:
                arrowSpeed = 150f;
                break;
        }
        baseDamage = 0;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        Vector3 hitSpeed = other.relativeVelocity;
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[0], hitSpeed);
        GameObject timeZone = Instantiate(TimeStopZone, other.contacts[0].point, Quaternion.identity);
        timeZone.GetComponentInChildren<SphereCollider>().enabled = false;
        SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);
    }
}
