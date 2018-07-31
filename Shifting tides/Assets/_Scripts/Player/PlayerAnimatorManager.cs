﻿using UnityEngine;
using UnityEngine.Animations;
using System.Collections;

public class PlayerAnimatorManager : MonoBehaviour
{

    private Animator animatorPlayer;
    private float inputSpeed;
    private bool onGround,aiming,input;

    private void Start()
    {
        animatorPlayer = GetComponent<Animator>();
    }

    public void PlayDashAnimation()
    {
        animatorPlayer.SetTrigger("Dashed");
    }

    public void PlayJumpAnimation() {

        animatorPlayer.SetTrigger("Jumped");
    }


    public float InputSpeed
    {
        get { return inputSpeed; }
        set
        {
            inputSpeed = value;
            animatorPlayer.SetFloat("Speed", inputSpeed);        
        }
    }

    public bool OnGround {
        get { return onGround; }
        set
        {
            onGround = value;
            animatorPlayer.SetBool("OnGround", onGround);
        }

    }
    public bool Aiming
    {
        get { return aiming; }
        set
        {
            aiming = value;
            animatorPlayer.SetBool("Aiming", aiming);
        }

    }
    public bool Input
    {
        get { return input; }
        set
        {
            input = value;
            animatorPlayer.SetBool("Input", input);
        }

    }
}