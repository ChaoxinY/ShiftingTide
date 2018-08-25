using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollManager : MonoBehaviour
{
    public List<Rigidbody> ragdollRigidbodies;
    public GameObject weaponInHand;

    private void Start()
    {
        Component[] childRigidBodies = gameObject.GetComponentsInChildren(typeof(Rigidbody));
        foreach (Rigidbody childRigidBody in childRigidBodies) {
            if (childRigidBody != GetComponent<Rigidbody>())
            {
                ragdollRigidbodies.Add(childRigidBody);
            }
        }
    }

    public void EnableRagdoll()
    {
        Destroy(GetComponent<Rigidbody>());
        GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GameObject weaponCopy = Instantiate(weaponInHand, weaponInHand.transform.position, weaponInHand.transform.rotation);
        weaponCopy.GetComponent<Rigidbody>().isKinematic = false;
        weaponCopy.GetComponent<Collider>().enabled = true;
        Destroy(weaponCopy.GetComponent<HostileHitbox>());
        Destroy(weaponInHand);
        foreach (Rigidbody childRigidBody in ragdollRigidbodies)
        {
            childRigidBody.isKinematic = false;
            childRigidBody.transform.GetComponent<Collider>().enabled = true;
        }
    }
    public void ApplyRagdollForce(Vector3 impactPoint,Vector3 impactForce) {

        Rigidbody closestRigidBody = ClosestRagdollTransform(impactPoint).gameObject.GetComponent<Rigidbody>();
        closestRigidBody.AddForce(-impactForce/70, ForceMode.Force);
    }

    public Transform ClosestRagdollTransform(Vector3 impactPoint) {

        float distanceToBodyPart = Mathf.Infinity;
        Transform closestTransform = null;

        foreach (Rigidbody childRigidBody in ragdollRigidbodies)
        {
            if (Vector3.Distance(impactPoint, childRigidBody.transform.position) < distanceToBodyPart)
            {
                distanceToBodyPart = Vector3.Distance(impactPoint, childRigidBody.transform.position);
                closestTransform = childRigidBody.transform;
            }
        }

        return closestTransform;
    }
}
