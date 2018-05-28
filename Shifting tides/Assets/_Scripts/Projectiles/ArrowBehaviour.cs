using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : Projectile
{
    private Vector3 arrowPlaceholderRotation;
    private GameObject arrowPlaceholder;
    private Quaternion localRotation;

    public float penetrationStrength;

    protected override void Initialize()
    {
        base.Initialize();
        arrowPlaceholder = Resources.Load("Prefabs/ArrowPlaceholder") as GameObject;
        gravity = -10.81f;
        rbObject = gameObject.GetComponent<Rigidbody>();
    }

    protected override IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(base.LocalUpdate());
        //float angle = Mathf.LerpAngle(transform.eulerAngles.x, 90f, Time.fixedDeltaTime - (rbObject.velocity.y / 300));
        //transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
        //Vector3 velocity = rbObject.velocity;
        Quaternion rotation = new Quaternion();
        if(rbObject.velocity!= Vector3.zero)
        rotation.SetLookRotation(rbObject.velocity, transform.up);
        transform.localRotation = rotation;             
        yield break;
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerResourcesManager.Health -= 10;
                break;
            
        }
        SetupArrowPlaceholder(other.contacts[0].point, rbObject.velocity * penetrationStrength,other.gameObject);

    }
    // This method calls CopyPositionAndRotationForArrowPlaceholder.
    // Then it instantiates the ArrowPlaceholder with the values from CopyPositionAndRotationForArrowPlaceholder and destroys the current arrow.
    private void SetupArrowPlaceholder(Vector3 contactPoint, Vector3 hitSpeed, GameObject movingTargetHit = null)
    {
        //Vector3 nomalizedHitspeed = Vector3.Normalize(hitSpeed);
        Vector3 spawnPosition = contactPoint - hitSpeed;
        arrowPlaceholderRotation = transform.eulerAngles;
        GameObject arrowDummy = Instantiate(arrowPlaceholder, spawnPosition, arrowPlaceholder.transform.rotation = Quaternion.Euler(arrowPlaceholderRotation));
        if (movingTargetHit != null)
        {
            arrowDummy.transform.SetParent(movingTargetHit.transform);
        }
        Destroy(gameObject);
    }
}
