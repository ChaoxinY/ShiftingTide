using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedObjectInitializer : TriggerBoundMechanism
{
    public Agent objectAgent;
    public Animator objectAnimator;
    public List<Collider> objectColliders = new List<Collider>();
    public float initializeAnimationDuration;

    public override void OnTriggerFunction()
    {
        StartCoroutine(InitializeObject());
    }

    private IEnumerator InitializeObject() {
       
        objectAnimator.enabled = true;
        yield return new WaitForSeconds(initializeAnimationDuration);
        foreach (Collider collider in objectColliders)
        {
            Debug.Log("Collider");
            collider.enabled = true;
        }
        objectAgent.enabled = true;      
        Destroy(gameObject);
        yield break;
    }
}
