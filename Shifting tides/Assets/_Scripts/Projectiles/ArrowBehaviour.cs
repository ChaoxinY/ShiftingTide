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
    public float baseDamage,arrowSpeed;
    public float penetrationStrength;

    protected PlayerTideComboManager playerTideComboManager;
    protected GameObject arrowPlaceholder;
    protected AudioSource onHitSoundSource;
    protected PlayerAimModule playerAimModule;

    protected override void Initialize()
    {
        base.Initialize();
        arrowPlaceholder = Resources.Load("Prefabs/Arrows/ArrowPlaceholder") as GameObject;
        gravity = -25.81f;
        rbObject = gameObject.GetComponent<Rigidbody>();
        onHitSoundSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerTideComboManager = GameObject.Find("Player").GetComponent<PlayerTideComboManager>();
        playerAimModule = GameObject.Find("Player").GetComponentInChildren<PlayerAimModule>();
    }

    protected override IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(base.LocalUpdate());
        //Quaternion rotation = new Quaternion();
        //rotation.SetLookRotation(rbObject.velocity);
        transform.rotation = Quaternion.LookRotation(rbObject.velocity);
        yield break;
    }

    public virtual void ApplyArrowStageValues(int stage)
    {

    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {      
            case "Enemy":
                EnemyHit(other);
                SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);               
                break;
            case "HeavyShield":
                baseDamage = 0;
                break;
            default:
                DefaultHit();
                SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized, other.gameObject);
                break;
        }
    }

   
    protected virtual void EnemyHit(Collision other)
    {
        Vector3 hitSpeed = other.relativeVelocity;
        HostileResourceManager hostileResourceManager = other.gameObject.GetComponent<HostileResourceManager>();
        if (hostileResourceManager == null) {
            hostileResourceManager = other.gameObject.GetComponentInParent<HostileResourceManager>();
        }
      
        if (other.collider.name == "CritSpot")
        {
            Debug.Log(hostileResourceManager);
            onHitSoundSource.clip = onHitSounds[2];
            onHitSoundSource.Play();
            hostileResourceManager.GotHitOnCritSpot(baseDamage,other.contacts[0].point, hitSpeed);
            SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[1], hitSpeed);
            SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[3], hitSpeed);
            playerTideComboManager.AddCombo();
            playerAimModule.critHit = true;
            playerAimModule.EnemyHit = true;
            return;
        }
        playerAimModule.EnemyHit = true;
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[0], hitSpeed);
        SpawnOnHitEffect(other.gameObject.transform, other.contacts[0], bleedEffect[2], hitSpeed);
        onHitSoundSource.clip = onHitSounds[1];
        onHitSoundSource.Play();
        hostileResourceManager.GotHit(baseDamage, other.contacts[0].point, hitSpeed);
       // playerTideComboManager.ResetCombo();
    }

    protected virtual void DefaultHit()
    {
        onHitSoundSource.clip = onHitSounds[0];
        onHitSoundSource.Play();
        // playerTideComboManager.ResetCombo();
    }

    protected void SpawnOnHitEffect(Transform transformHit, ContactPoint contact, GameObject prefabToSpawn, Vector3 hitSpeed)
    {
        GameObject spawnedOnHitEffect = Instantiate(prefabToSpawn, contact.point + (hitSpeed.normalized * 1.3f), Quaternion.LookRotation(hitSpeed.normalized));
        //spawnedOnHitEffect.transform.SetParent(transformHit);
    }

    // This method calls CopyPositionAndRotationForArrowPlaceholder.
    // Then it instantiates the ArrowPlaceholder with the values from CopyPositionAndRotationForArrowPlaceholder and destroys the current arrow.
    protected virtual void SetupArrowPlaceholder(Vector3 contactPoint, Vector3 hitSpeed, GameObject TargetHit )
    {

        //Vector3 nomalizedHitspeed = Vector3.Normalize(hitSpeed);
        Vector3 spawnPosition = contactPoint + hitSpeed.normalized * penetrationStrength;
        Vector3 arrowPlaceholderRotation = transform.eulerAngles;
        GameObject arrowDummy = Instantiate(arrowPlaceholder, spawnPosition, arrowPlaceholder.transform.rotation = Quaternion.Euler(arrowPlaceholderRotation));

        GameObject DummyParent = new GameObject
        {
            name = "ArrowDummy"
        };

        if (TargetHit.GetComponentInParent<RagdollManager>()|| TargetHit.GetComponent<RagdollManager>())
        {           
            Transform transformParent = TargetHit.GetComponent<RagdollManager>().ClosestRagdollTransform(contactPoint);
            Debug.Log(transformParent.gameObject.name);
            DummyParent.transform.SetParent(transformParent);
            arrowDummy.transform.SetParent(DummyParent.transform);
            Destroy(gameObject);
            return; 
        }

        DummyParent.transform.SetParent(TargetHit.transform);
        arrowDummy.transform.SetParent(DummyParent.transform);
        Destroy(gameObject);
    }

   

}
