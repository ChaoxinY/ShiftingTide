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
        SpawnTimeStopZone(other.transform.position);
    }

    private void SpawnTimeStopZone(Vector3 contactPosition)
    {
        GameObject spawnedOnHitEffect = Instantiate(TimeStopZone, contactPosition, Quaternion.identity);
    }
}
