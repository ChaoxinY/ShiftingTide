using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    private TriggerJoint triggerJoint;
    // BoundMechanisme TriggerBoundMechinsme boundMechanism
    public GameObject boundMechanism;

    void Start () {
        Initialize();
    }

    public virtual void Initialize() {
        triggerJoint = gameObject.GetComponent<TriggerJoint>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Collider>()) {
            Debug.Log("hit");
            triggerJoint.triggerConditionMet = true;
            if (triggerJoint.isBoundMechnismeActive == false)
            {
                triggerJoint.isBoundMechnismeActive = true;
                boundMechanism.SetActive(true);
            }
        }   
    }
}
