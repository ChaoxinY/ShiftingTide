using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoundMechanism : StandardInteractiveGameObject
{
    private bool triggered;

    protected override void Initialize()
    {
        base.Initialize();
    }

    public virtual void OnTriggerFunction() { }
    public virtual void MechanismFunction() { }

    public bool Triggered
    {
        get
        {
            return triggered;
        }
        set
        {
            triggered = value;
            OnTriggerFunction();
        }
    }
}
