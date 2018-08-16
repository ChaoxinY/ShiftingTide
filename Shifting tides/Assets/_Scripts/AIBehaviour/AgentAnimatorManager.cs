using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimatorManager : MonoBehaviour
{

    private Animator agentAnimator;
    private bool moving;
    private float animatorSpeed;

    private void Start()
    {
        agentAnimator = GetComponent<Animator>();
    }

    public AnimatorStateInfo GetAnimatorStateInfo(int layerIndex)
    {
        AnimatorStateInfo currentAnimatorStateInfo = agentAnimator.GetCurrentAnimatorStateInfo(layerIndex);
        return currentAnimatorStateInfo;
    }

    public void PlayAttackAnimation()
    {
        agentAnimator.SetTrigger("Attacked");
    }

    public bool Moving
    {
        get { return moving; }
        set
        {
            moving = value;
            agentAnimator.SetBool("Moving", moving);
        }
    }

    public float AnimatorSpeed
    {
        get { return animatorSpeed; }
        set
        {
            animatorSpeed = value;
            agentAnimator.speed = animatorSpeed;
        }
    }
}
