using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleSystemHandler : MonoBehaviour {

    protected ParticleSystem particleSystemToManage;
    private float[] currentProfielValues;
    private float[] defaultProfielValues;
    private float[] targetProfielValues;

    private void Start()
    {
        InitializeParticleSystem();
        GeneratedDefaultProfiel();
    }

    protected abstract void InitializeParticleSystem();

    protected abstract void GeneratedDefaultProfiel();

    protected abstract void ResetCurrentProfiel();

    protected virtual void LateUpdate() {


    }

}
