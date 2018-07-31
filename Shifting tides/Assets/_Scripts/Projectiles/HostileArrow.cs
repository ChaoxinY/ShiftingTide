using UnityEngine;
using System.Collections;

public class HostileArrow : ArrowBehaviour
{
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" ) {
            onHitSoundSource.clip = onHitSounds[1];
            onHitSoundSource.Play();
            PlayerResourcesManager.Health -= 20;
            SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized ,other.gameObject);
            return;
        }
        DefaultHit();
        SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized);
    }
    protected override void SetupArrowPlaceholder(Vector3 contactPoint, Vector3 hitSpeed, GameObject movingTargetHit = null)
    {
        Vector3 spawnPosition = contactPoint + hitSpeed.normalized * penetrationStrength;
        Vector3 arrowPlaceholderRotation = transform.eulerAngles;
        GameObject arrowDummy = Instantiate(arrowPlaceholder, spawnPosition, arrowPlaceholder.transform.rotation = Quaternion.Euler(arrowPlaceholderRotation));
        arrowDummy.GetComponent<DestroyGameObjectAfterTime>().enabled = true;
        if (movingTargetHit != null)
        {
            arrowDummy.transform.SetParent(movingTargetHit.transform);
        }
        Destroy(gameObject);
    }

}
