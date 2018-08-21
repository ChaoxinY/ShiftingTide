using UnityEngine;
using System.Collections;

public class StatueBrute : HostileTerrestrial
{
    private bool charging;
    private StatueBruteHitBoxManager statueBruteHitBoxManager;


    protected override void Initialize()
    {
        base.Initialize();
        statueBruteHitBoxManager = GetComponent<StatueBruteHitBoxManager>();
    }

    private void Update()
    {
        if (charging&&agentAnimatorManager.AnimatorSpeed!= 0)
        {
            Vector3 targetDir = GameObject.Find("Player").transform.position - transform.position;
            targetDir.y = 0;
            Vector3 newRotation = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 5f, 0);
            transform.rotation = Quaternion.LookRotation(newRotation);
        }
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
        yield return StartCoroutine(SpearCharge());
        yield break;
    }

    private IEnumerator SpearCharge() {

        agentAnimatorManager.Moving = false;
        agent.updateRotation = false;
        agent.updatePosition = false;
        Rigidbody rigidbodyAgent = GetComponent<Rigidbody>();
        rigidbodyAgent.isKinematic = false;
        rigidbodyAgent.velocity = Vector3.zero;
        agentAnimatorManager.PlayAttackAnimation();
        StartCoroutine(statueBruteHitBoxManager.SpearChargeHitBoxAnimation());
        yield return new WaitForSeconds(0.8f);
        charging = true;
        yield return new WaitUntil(() => !StaticToolMethods.GetAnimatorStateInfo(0,agentAnimatorManager.agentAnimator).IsName("Attacking"));
        rigidbodyAgent.isKinematic = true;
        charging = false;
        agent.nextPosition = transform.position;
        //agent.enabled = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
       
    }
}
