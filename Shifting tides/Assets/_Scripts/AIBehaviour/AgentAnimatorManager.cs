﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimatorManager : MonoBehaviour
{
    public Animator agentAnimator;

    private bool moving;
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
