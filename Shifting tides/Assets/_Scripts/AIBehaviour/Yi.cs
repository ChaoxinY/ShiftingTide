using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Yi : HostileTerrestrial
{
    private YiAgentAnimatorManager yiAgentAnimatorManager;
    private YiHitBoxManager yiHitBoxManager;
    private List<IEnumerator> moveConditions = new List<IEnumerator>();
    private List<IEnumerator> availableAttackMoves = new List<IEnumerator>();

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
        yiAgentAnimatorManager.Moving = true;
        yield break;
    }

    private IEnumerator HeadSlamConditionCheck()
    {       
        if (distanceToPray <= 5)
        {
            availableAttackMoves.Add(HeadSlam());
        }
        yield break;
    }

    private IEnumerator HeadSlam()
    {
        yiAgentAnimatorManager.Moving = false;
        yiAgentAnimatorManager.PlayHeadSlamAnimation();
        StartCoroutine(yiHitBoxManager.HeadSlamHitBoxAnimation());
        yield return new WaitUntil(() => !StaticToolMethods.GetAnimatorStateInfo(0, agentAnimatorManager.agentAnimator).IsName("HeadSlam"));
        yiAgentAnimatorManager.Moving = true;
        yield break;
    }

    private IEnumerator TailSwipeConditionCheck()
    {
        if (distanceToPray > 5 && distanceToPray < chaseRange)
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
