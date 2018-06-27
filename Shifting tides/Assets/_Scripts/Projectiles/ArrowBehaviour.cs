﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : Projectile
{
    [HideInInspector]
    public float baseDamage;
    public GameObject[] bleedEffect;
    private AudioSource onHitSoundSource;
    public AudioClip[] onHitSounds;

    private Vector3 arrowPlaceholderRotation;
    private GameObject arrowPlaceholder;
    private Quaternion localRotation;

    public float penetrationStrength;

    protected override void Initialize()
    {
        base.Initialize();
        arrowPlaceholder = Resources.Load("Prefabs/ArrowPlaceholder") as GameObject;
        gravity = -25.81f;
        rbObject = gameObject.GetComponent<Rigidbody>();
        onHitSoundSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }

    protected override IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(base.LocalUpdate());
        Quaternion rotation = new Quaternion();
        if(rbObject.velocity!= Vector3.zero)
        rotation.SetLookRotation(rbObject.velocity, transform.up);
        transform.localRotation = rotation;
       // Debug.Log(baseDamage);
        yield break;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider.name);
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerResourcesManager.Health -= 10;
                break;
            case "Enemy":
                EnemyHit(other);
                break;
            default:
                onHitSoundSource.clip = onHitSounds[0];
                Debug.Log(onHitSoundSource.clip);
                onHitSoundSource.Play();
                break;

        }

      SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized,other.gameObject);

    }

    private void EnemyHit(Collision other)
    {
        Vector3 hitSpeed = other.relativeVelocity;
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[0],hitSpeed);
        if (other.collider.name == "CritSpot")
        {
            onHitSoundSource.clip = onHitSounds[2];
            onHitSoundSource.Play();
            other.gameObject.GetComponent<HostileResourceManager>().GotHitOnCritSpot(baseDamage);
            SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[1], hitSpeed);
            SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[3], hitSpeed);
            return;
        }
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[2],hitSpeed);
        onHitSoundSource.clip = onHitSounds[1];
        onHitSoundSource.Play();
        other.gameObject.GetComponentInParent<HostileResourceManager>().GotHit(baseDamage);
    }

    private void SpawnOnHitEffect(Transform transformHit, ContactPoint contact,GameObject prefabToSpawn,Vector3 hitSpeed)
    {
        GameObject spawnedOnHitEffect = GameObject.Instantiate(prefabToSpawn, contact.point + (hitSpeed.normalized*1.3f), Quaternion.LookRotation(hitSpeed.normalized));
        spawnedOnHitEffect.transform.SetParent(transformHit);
    }

    // This method calls CopyPositionAndRotationForArrowPlaceholder.
    // Then it instantiates the ArrowPlaceholder with the values from CopyPositionAndRotationForArrowPlaceholder and destroys the current arrow.
    private void SetupArrowPlaceholder(Vector3 contactPoint, Vector3 hitSpeed, GameObject movingTargetHit = null)
    {
        //Vector3 nomalizedHitspeed = Vector3.Normalize(hitSpeed);
        Vector3 spawnPosition = contactPoint + hitSpeed.normalized * penetrationStrength;
        arrowPlaceholderRotation = transform.eulerAngles;
        GameObject arrowDummy = Instantiate(arrowPlaceholder, spawnPosition, arrowPlaceholder.transform.rotation = Quaternion.Euler(arrowPlaceholderRotation));
        if (movingTargetHit != null)
        {
            arrowDummy.transform.SetParent(movingTargetHit.transform);
        }
        Destroy(gameObject);
    }
}
