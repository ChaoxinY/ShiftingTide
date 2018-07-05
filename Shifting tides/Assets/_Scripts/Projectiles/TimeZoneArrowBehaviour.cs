using UnityEngine;
using System.Collections;
using System;

public class TimeZoneArrowBehaviour : ArrowBehaviour
{
    public GameObject TimeStopZone;

    protected override void OnCollisionEnter(Collision other)
    {
        Vector3 hitSpeed = other.relativeVelocity;
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[0], hitSpeed);
        Instantiate(TimeStopZone, other.contacts[0].point, Quaternion.identity);
        SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);
    }
}
