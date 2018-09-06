using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Yi : HostileTerrestrial
{
    private YiAgentAnimatorManager yiAgentAnimatorManager;
    private YiHitBoxManager yiHitBoxManager;
    private List<IEnumerator> moveConditions = new List<IEnumerator>();
    private List<IEnumerator> availableAttackMoves = new List<IEnumerator>();
    private bool attacking;

    protected override void Initialize()
    {
        yiAgentAnimatorManager = GetComponent<YiAgentAnimatorManager>();
        yiHitBoxManager = GetComponent<YiHitBoxManager>();
    }

    private void RefreshMoveContionCoroutines()
    {
        moveConditions.Add(HeadSlamConditionCheck());
        moveConditions.Add(TailSwipeConditionCheck());
    }
    private void Update()
    {
        if (attacking && agentAnimatorManager.AnimatorSpeed != 0)
        {
            Vector3 targetDir = GameObject.Find("Player").transform.position - transform.position;
            targetDir.y = 0;
            Vector3 newRotation = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 5f, 0);
            transform.rotation = Quaternion.LookRotation(newRotation);
        }
    }
    protected override IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(DetermineCurrentAttack());
        StartCoroutine(Hunting());
        yield break;
    }

    private IEnumerator DetermineCurrentAttack()
    {
        RefreshMoveContionCoroutines();
        foreach (IEnumerator moveCondition in moveConditions)
        {
            yield return StartCoroutine(moveCondition);
        }
        if (availableAttackMoves.Count != 0)
        {
            yield return StartCoroutine(availableAttackMoves[StaticToolMethods.GenerateARandomNumber(availableAttackMoves.Count)]);
            availableAttackMoves.Clear();
        }
        yield break;
    }

    protected override IEnumerator Hunting()
    {
        GameObject player = GameObject.Find("Player");
        SetCurrentTarget(player);
        MoveTowardsTarget(currentTarget);
        UpdateTargetDistance(player);
        yiAgentAnimatorManager.Moving = true;
        yield break;
    }

    private IEnumerator HeadSlamConditionCheck()
    {
        GameObject player = GameObject.Find("Player");
        UpdateTargetDistance(player);
        Debug.Log(distanceToPray);
        if (distanceToPray > 10 && distanceToPray < 15)
        {
            availableAttackMoves.Add(HeadSlam());
        }
        yield break;
    }

    private IEnumerator HeadSlam()
    {
        agent.speed = 0;
        attacking = true;
        yiAgentAnimatorManager.Moving = false;
        yiAgentAnimatorManager.PlayHeadSlamAnimation();       
        StartCoroutine(yiHitBoxManager.HeadSlamHitBoxAnimation());
        yield return new WaitForSeconds(0.6f);
        attacking = false;
        yield return new WaitUntil(() => !StaticToolMethods.GetAnimatorStateInfo(0, agentAnimatorManager.agentAnimator).IsName("HeadSlam"));
        agent.speed = standardSpeed;       
        yiAgentAnimatorManager.Moving = true;
        yield break;
    }

    private IEnumerator TailSwipeConditionCheck()
    {
        if (distanceToPray < 5)
        {
            availableAttackMoves.Add(TailSwipe());
        }
        yield break;
    }

    private IEnumerator TailSwipe()
    {

        yield break;
    }
}
