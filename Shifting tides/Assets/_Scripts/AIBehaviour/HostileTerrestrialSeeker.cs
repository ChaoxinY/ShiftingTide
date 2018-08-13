﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HostileTerrestrialSeeker : Terrestrial
{
    private GameObject objectToChase;
    private float distanceToPray;
    private bool InCombat;

    public float visionRange, sightRange, chaseRange, attackRange;

    protected override IEnumerator LocalUpdate()
    {
        yield return base.LocalUpdate();
        if (!InCombat)
        {
            StartCoroutine(Searching());
        }
    }

    protected virtual IEnumerator Searching()
    {  
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.SphereCast(transform.position, visionRange, fwd, out hit, sightRange))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                UpdatePrayDistance(hit.collider.gameObject);
                objectToChase = hit.collider.gameObject;
                SwitchToCombatMode();
                yield break;
            }
        }
    }

    protected virtual IEnumerator Hunting()
    {
        GameObject player = GameObject.Find("Player");
        UpdatePrayDistance(player);
        Chase(player);
        MoveTowardsTarget(currentTarget);
        if (distanceToPray <= attackRange)
        {
            yield return StartCoroutine(EngageClassBehavior());
        }
        if (distanceToPray > chaseRange)
        {
            overwritingBehaviour = null;
            InCombat = false;
        }
        yield break;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            SwitchToCombatMode();
        }
    }

    protected virtual void SwitchToCombatMode() {
        Debug.Log("Alart");
        overwritingBehaviour = "Hunting";
        InCombat = true;
    }

    protected virtual IEnumerator EngageClassBehavior()
    {
        Debug.Log("I hit you");
        agent.speed = 0;
        yield return new WaitForSeconds(0.5f);
        agent.speed = standardSpeed;
        yield break;
    }

    private void UpdatePrayDistance(GameObject hunted)
    {
        distanceToPray = Vector3.Distance(gameObject.transform.position, hunted.transform.position);
    }
}
