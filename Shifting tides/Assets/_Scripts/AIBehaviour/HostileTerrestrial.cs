﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HostileTerrestrial : Terrestrial
{  
    public float visionRange, sightRange, chaseRange, attackRange;

    private float distanceToPray;
    private bool InCombat = false;

    protected override IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(base.LocalUpdate());
        if (!InCombat)
        {
            StartCoroutine(Searching());
        }
        else {
            yield return new WaitForSeconds(0.3f);
            InCombat = false;
        }
        StartCoroutine(RestingBehaviourCheck());
        yield return StartCoroutine(OverWrittingBehaviourCheck());
        yield return StartCoroutine(StandardBehaviourCheck());
    }

    protected virtual IEnumerator Searching()
    {
        Debug.Log("Searching");
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.SphereCast(transform.position, visionRange, fwd, out hit, sightRange))
        {
            Debug.Log("Hit");
            if (hit.collider.gameObject.tag == "Player")
            {
                UpdateTargetDistance(hit.collider.gameObject);              
                SwitchToCombatMode();
                yield break;
            }
        }
    }

    //Conflict with circling
    protected virtual IEnumerator Hunting()
    {
        InCombat = true;
        overwrittingBehaviourFinished = false;
        GameObject player = GameObject.Find("Player");
        UpdateTargetDistance(player);
        SetCurrentTarget(player);       
        MoveTowardsTarget(currentTarget);
        agentAnimatorManager.Moving = true;
        if (distanceToPray <= attackRange)
        {
            yield return StartCoroutine(EngageClassBehavior());
        }
        if (distanceToPray >= chaseRange) {

            InCombat = false;
            overwritingBehaviour = null;
        }
        overwrittingBehaviourFinished = true;
        yield break;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            SwitchToCombatMode();
        }
    }

    protected virtual IEnumerator OnHitFeedback()
    {
        agent.speed = agent.speed / 5;
        yield return new WaitForSeconds(0.8f);
        agent.speed = standardSpeed;
        yield break;
    }


    public virtual void SwitchToCombatMode()
    {
        Debug.Log("Alart");
        overwrittingBehaviourFinished = true;
        overwritingBehaviour = "Hunting";
    }

    protected virtual IEnumerator EngageClassBehavior()
    {
        yield break;
    }

    private IEnumerator CheckIfTargetIsStillInRange()
    {
        GameObject player = GameObject.Find("Player");
        UpdateTargetDistance(player);
        if (distanceToPray > chaseRange)
        {
            overwritingBehaviour = null;
            InCombat = false;
        }
        yield break;
    }

    private void UpdateTargetDistance(GameObject hunted)
    {
        distanceToPray = Vector3.Distance(gameObject.transform.position, hunted.transform.position);
    }
}
