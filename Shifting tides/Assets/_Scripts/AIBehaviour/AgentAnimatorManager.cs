using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimatorManager : MonoBehaviour
{
    public Animator agentAnimator;

    private bool moving,active;
    private float animatorSpeed;

    private void Start()
    {
        //agentAnimator = gameObject.GetComponent<Animator>();
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

    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            agentAnimator.SetBool("Active", active);
        }
    }


    public float AnimatorSpeed
    {
        get { return animatorSpeed; }
        set
        {
            animatorSpeed = value;
            agentAnimator.SetFloat("AnimatorSpeed", animatorSpeed);
        }
    }
}
