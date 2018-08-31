using UnityEngine;
using System.Collections;

public class ColliderTrigger : MonoBehaviour
{
    public TriggerBoundMechanism boundMechanism;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() && boundMechanism != null)
        {
            boundMechanism.Triggered = true;
        }
    }
}
