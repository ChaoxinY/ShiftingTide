using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volant : Agent
{
    private Vector3 path, targetRot;
    
    public Transform[] activityBounds;

    protected override void InitializeLists()
    {
        base.InitializeLists();
        spontaneousBehaviours.Add("Roaming");
        patternedBehaviours.Add("AirCircling");
        patternedBehaviours.Add("Patroling");
        restingBehaviours.Add("AirRest");
        restingBehaviours.Add("GroundRest");
    }
    
    // do this in local update 
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(targetRot);
        transform.position = Vector3.MoveTowards(transform.position, path, Time.deltaTime * 7f);
    }


    protected IEnumerator AirCircling() {

        int CirclesToSpin = Random.Range(0, 5);
        while(CirclesToSpin < 5) {            
            path = transform.position + Vector3.forward * 5f;
            targetRot = Vector3.RotateTowards(transform.position,path,Time.deltaTime*3f,0.0f);
            yield return new WaitUntil(()=> Arrived(gameObject.transform.position, path, 0.5f));
            CirclesToSpin++;           
        }
        StartCoroutine(FinishStandandrMovementBehaviour(0,1));
        yield break;
    }
    protected IEnumerator AirRest()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator Dashing()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator GroundRest()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator Patroling()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
    protected IEnumerator Roaming()
    {
        StartCoroutine(FinishStandandrMovementBehaviour(0, 1));
        yield break;
    }
  
}
