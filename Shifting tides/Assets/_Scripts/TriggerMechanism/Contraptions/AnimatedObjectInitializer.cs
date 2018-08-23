using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedObjectInitializer : TriggerBoundMechanism
{
    public Agent objectAgent;
    public Animator objectAnimator;
    public Collider objectCollider;
    public float initializeAnimationDuration;

    public override void OnTriggerFunction()
    {
        StartCoroutine(InitializeObject());
    }

    private IEnumerator InitializeObject() {
       
        objectAnimator.enabled = true;
        yield return new WaitForSeconds(initializeAnimationDuration);
        objectAgent.enabled = true;
        objectCollider.enabled = true;
        yield break;
    }
}
