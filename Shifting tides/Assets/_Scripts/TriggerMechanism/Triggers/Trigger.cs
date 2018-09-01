using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    // BoundMechanisme TriggerBoundMechinsme boundMechanism
    public TriggerBoundMechanism boundMechanism;

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Debug.Log(boundMechanism);
        if (other.gameObject.GetComponent<Rigidbody>() && boundMechanism != null) {
            Debug.Log("called");
            boundMechanism.Triggered = true;
        }   
    }
}
