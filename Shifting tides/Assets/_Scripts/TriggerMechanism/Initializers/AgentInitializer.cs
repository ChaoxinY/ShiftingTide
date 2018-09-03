using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentInitializer : TriggerBoundMechanism
{  
    public List<Collider> objectColliders = new List<Collider>();
    public float initializeAnimationDuration;

    private Agent objectAgent;
    private AgentAnimatorManager objectAnimatorManager;

    protected override void Initialize()
    {
        base.Initialize();
        objectAgent = GetComponentInParent<Agent>();
        objectAnimatorManager = GetComponentInParent<AgentAnimatorManager>();
    }

    public override IEnumerator OnTriggerFunction()
    {
        StartCoroutine(InitializeObject());
        yield break;
    }

    protected virtual IEnumerator InitializeObject() {

        objectAnimatorManager.Active = true;
        yield return new WaitForSeconds(initializeAnimationDuration);
        foreach (Collider collider in objectColliders)
        {
            Debug.Log("Collider");
            collider.enabled = true;
        }
        objectAgent.enabled = true;   
        yield break;
    }
}
