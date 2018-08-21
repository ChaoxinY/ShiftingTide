using UnityEngine;
using System.Collections;

public class HostileHitBox : MonoBehaviour
{
    public Collider hitBoxCollider;
    public float hitBoxDamage;

    private void Start()
    {
        hitBoxCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            hitBoxCollider.enabled = false;
            PlayerResourcesManager.Health -= hitBoxDamage;
        }
    }
}
