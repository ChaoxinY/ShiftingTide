using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HostileTerrestrialSeeker : Terrestrial
{
    private GameObject objectToChase;
    private float distanceToPray;

    public float visionRange, sightRange, chaseRange, attackRange;

    protected override void Initialize()
    {
        base.Initialize();
 
    }

    IEnumerator Searching()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.SphereCast(transform.position, visionRange, fwd, out hit, sightRange) || Physics.SphereCast(transform.position, 3.5f, fwd, out hit, 0.1f))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                UpdatePrayDistance(hit.collider.gameObject);
                objectToChase = hit.collider.gameObject;
                yield break;
            }
        }
    }

    IEnumerator Hunting(GameObject hunted)
    {
        UpdatePrayDistance(hunted);
        Chase(hunted);
        MoveTowardsTarget(hunted.transform.position);
        if (distanceToPray <= attackRange)
        {

            EngageClassBehavior();
            //yield return new WaitForSeconds(0.5f);
            UpdatePrayDistance(hunted);
            // finishedBehaviour = false;
            yield break;
        }
        if (distanceToPray >= chaseRange)
        {

            ResetVelocity();
            yield break;
        }
    }
    private void UpdatePrayDistance(GameObject hunted)
    {
        distanceToPray = Vector3.Distance(gameObject.transform.position, hunted.transform.position);
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            objectToChase = GameObject.Find("Player");
            GotHit(collision.gameObject);
        }
    }
    protected void GotHit(GameObject arrow)
    {
     
    }
    protected virtual void EngageClassBehavior()
    {

    }

}
