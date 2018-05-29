using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volant : Agent
{
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

    private void Update()
    {
        
    }


    protected IEnumerator AirCircling() {

        int CirclesToSpin = Random.Range(0, 5);
        while(CirclesToSpin < 5) {
           
            Vector3 path = transform.position + Vector3.forward * 5f;
            Vector3 targetRot = Vector3.RotateTowards(transform.position,path,Time.deltaTime*3f,0.0f);
            while (Arrived(gameObject.transform.position, path, 0.5f) == false )
            {
                Debug.Log("Running");
                transform.rotation = Quaternion.LookRotation(targetRot);
                transform.position = Vector3.MoveTowards(transform.position, path, Time.deltaTime * 2f);
            }
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
