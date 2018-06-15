using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class TriggerBoundMechanism : StandardInteractiveGameObject
{   
    //Bound trigger
    public TriggerJoint boundTrigger;

    protected override void Initialize()
    {
        base.Initialize();
        TriggerFunction();
        if(boundTrigger != null)
        boundTrigger = boundTrigger.GetComponent<TriggerJoint>();
    }
 
    public virtual void TriggerFunction() { }
}
