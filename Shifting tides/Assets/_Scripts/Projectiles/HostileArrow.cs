using UnityEngine;
using System.Collections;

public class HostileArrow : ArrowBehaviour
{
    private PlayerStatusManager playerStatusManager;

    protected override void Initialize()
    {
        base.Initialize();
        gravity = 0;
        playerStatusManager = GameObject.Find("Player").GetComponent<PlayerStatusManager>();
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" ) {
            onHitSoundSource.clip = onHitSounds[1];
            onHitSoundSource.Play();
            playerStatusManager.ApplyDamage(25f);
            SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized ,other.gameObject);
            return;
        }
        DefaultHit();
        SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized,other.gameObject);
    }
    protected override void SetupArrowPlaceholder(Vector3 contactPoint, Vector3 hitSpeed, GameObject TargetHit)
    {
        //Vector3 nomalizedHitspeed = Vector3.Normalize(hitSpeed);
        Vector3 spawnPosition = contactPoint + hitSpeed.normalized * penetrationStrength;
        Vector3 arrowPlaceholderRotation = transform.eulerAngles;
        GameObject arrowDummy = Instantiate(arrowPlaceholder, spawnPosition, arrowPlaceholder.transform.rotation = Quaternion.Euler(arrowPlaceholderRotation));
        arrowDummy.GetComponent<DestroyGameObjectAfterTime>().enabled = true;

        GameObject DummyParent = new GameObject();
        DummyParent.transform.SetParent(TargetHit.transform);
        arrowDummy.transform.SetParent(DummyParent.transform);

        Destroy(gameObject);
    }

}
