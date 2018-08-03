using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    // BoundMechanisme TriggerBoundMechinsme boundMechanism
    public TriggerBoundMechanism boundMechanism;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>()) {
            boundMechanism.Triggered = true;
        }   
    }
}
