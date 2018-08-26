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

    public virtual IEnumerator OnTriggerFunction() { yield break; }
    public virtual IEnumerator MechanismFunction() { yield break; }

    public bool Triggered
    {
        get
        {
            return triggered;
        }
        set
        {
            triggered = value;
            StartCoroutine(OnTriggerFunction());
        }
    }
}
