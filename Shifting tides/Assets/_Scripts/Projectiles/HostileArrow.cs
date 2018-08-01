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
        SetupArrowPlaceholder(other.contacts[0].point, other.relativeVelocity.normalized,other.gameObject);
    }

}
