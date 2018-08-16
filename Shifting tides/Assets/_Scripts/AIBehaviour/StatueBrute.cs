using UnityEngine;
using System.Collections;

public class StatueBrute : HostileTerrestrial
{
    private bool charging;
    private void Update()
    {
        //if (charging)
        //{
        //    Vector3 targetDir = GameObject.Find("Player").transform.position - transform.position;
        //    Vector3 newRotation = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 5f, 0);
        //    transform.rotation = Quaternion.LookRotation(newRotation);
        //}
    }
    protected override IEnumerator LocalUpdate()
    {
        yield return base.LocalUpdate();
       
            //Vector3 direction = (GameObject.Find("Player").transform.position - transform.position).normalized;
            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    protected override IEnumerator EngageClassBehavior()
    {
        Debug.Log("I hit you");
        //Dynamic decision making
        //and then...
        StartCoroutine(SpearCharge());
        yield break;
    }

    private IEnumerator SpearCharge() {

        agentAnimatorManager.Moving = false;
        agent.updateRotation = false;
        agent.updatePosition = false;
        Rigidbody rigidbody1 = GetComponent<Rigidbody>();
        rigidbody1.isKinematic = false;
        agentAnimatorManager.PlayAttackAnimation();
        yield return new WaitForSeconds(0.4f);
        charging = true;
        yield return new WaitUntil(() => !agentAnimatorManager.GetAnimatorStateInfo(0).IsName("Attacking"));
        rigidbody1.isKinematic = true;
        charging = false;
        agent.nextPosition = transform.position;
        //agent.enabled = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
       
    }
}
