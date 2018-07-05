using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArrowBehaviour : Projectile
{
    public GameObject[] bleedEffect;
    public AudioClip[] onHitSounds;
    [HideInInspector]
    public float baseDamage;
    public float penetrationStrength;
    
    private GameObject arrowPlaceholder;
    private AudioSource onHitSoundSource;
    private PlayerTideComboManager playerTideComboManager;

    protected override void Initialize()
    {
        base.Initialize();
        arrowPlaceholder = Resources.Load("Prefabs/Arrows/ArrowPlaceholder") as GameObject;
        gravity = -25.81f;
        rbObject = gameObject.GetComponent<Rigidbody>();
        onHitSoundSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerTideComboManager = GameObject.Find("Player").GetComponent<PlayerTideComboManager>();
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

    protected virtual void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider.name);
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerResourcesManager.Health -= 10;
                SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);
                break;
            case "Enemy":
                EnemyHit(other);
                SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);
                break;
            default:
                DefaultHit();
                SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized);
                break;
        }    
    }

    private void DefaultHit() {
        onHitSoundSource.clip = onHitSounds[0];
        onHitSoundSource.Play();
        playerTideComboManager.ResetCombo();
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
            playerTideComboManager.AddCombo();
            return;
        }
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[2],hitSpeed);

        //SoundManager
        onHitSoundSource.clip = onHitSounds[1];
        onHitSoundSource.Play();

        other.gameObject.GetComponentInParent<HostileResourceManager>().GotHit(baseDamage);
        playerTideComboManager.ResetCombo();
              
    }

    protected void SpawnOnHitEffect(Transform transformHit, ContactPoint contact,GameObject prefabToSpawn,Vector3 hitSpeed)
    {
        GameObject spawnedOnHitEffect = Instantiate(prefabToSpawn, contact.point + (hitSpeed.normalized*1.3f), Quaternion.LookRotation(hitSpeed.normalized));
        spawnedOnHitEffect.transform.SetParent(transformHit);
    }

    // This method calls CopyPositionAndRotationForArrowPlaceholder.
    // Then it instantiates the ArrowPlaceholder with the values from CopyPositionAndRotationForArrowPlaceholder and destroys the current arrow.
    protected void SetupArrowPlaceholder(Vector3 contactPoint, Vector3 hitSpeed, GameObject movingTargetHit = null)
    {
        //Vector3 nomalizedHitspeed = Vector3.Normalize(hitSpeed);
        Vector3 spawnPosition = contactPoint + hitSpeed.normalized * penetrationStrength;
        Vector3 arrowPlaceholderRotation  = transform.eulerAngles;
        GameObject arrowDummy = Instantiate(arrowPlaceholder, spawnPosition, arrowPlaceholder.transform.rotation = Quaternion.Euler(arrowPlaceholderRotation));
        if (movingTargetHit != null)
        {
            arrowDummy.transform.SetParent(movingTargetHit.transform);
        }
        Destroy(gameObject);
    }
}
