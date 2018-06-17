using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleSystemHandler : MonoBehaviour
{

    protected ParticleSystem particleSystemToManage;
    protected float[] currentProfielValues = new float[100];
    protected float[] defaultProfielValues = new float[100];
    protected float[] targetProfielValues = new float[100];

    public virtual void PlayParticleAnimation()
    {
        particleSystemToManage.Play();
    }

    public virtual void StopParticleAnimation()
    {
        particleSystemToManage.Stop();
        ResetCurrentProfiel();
    }

    public virtual void PauseParticleAnimation() { }

    private void Start()
    {
        InitializeParticleSystem();
        GeneratedDefaultProfiel();
    }

    protected abstract void InitializeParticleSystem();

    protected void GeneratedDefaultProfiel()
    {
        defaultProfielValues = (float[])currentProfielValues.Clone();
    }

    protected void ResetCurrentProfiel()
    {
        Array.Clear(currentProfielValues, 0, currentProfielValues.Length);
        currentProfielValues = (float[])defaultProfielValues.Clone();
    }

    protected virtual void LateUpdate()
    {

    }

}
