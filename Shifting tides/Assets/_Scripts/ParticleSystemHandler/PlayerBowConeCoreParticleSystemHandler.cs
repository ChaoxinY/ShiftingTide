using UnityEngine;
using System.Collections;
using System;

public class PlayerBowConeCoreParticleSystemHandler : ParticleSystemHandler
{
    private ParticleSystem.MainModule mainModule;
    private IEnumerator prolongParticleLifeTimeCoroutine;
    private float lifeTime;

    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GetComponent<ParticleSystem>();
        mainModule = particleSystemToManage.main;
        lifeTime = mainModule.startLifetime.constant;
        currentProfielValues[0] = mainModule.simulationSpeed;
        GeneratedDefaultProfiel();
    }

    public override void PlayParticleAnimation()
    {
        mainModule.simulationSpeed = currentProfielValues[0];
        base.PlayParticleAnimation();
        prolongParticleLifeTimeCoroutine = ProlongLifeTime();
        StartCoroutine(prolongParticleLifeTimeCoroutine);
    }

    public override void StopParticleAnimation()
    {
        base.StopParticleAnimation();
        particleSystemToManage.Clear();
        StopCoroutine(prolongParticleLifeTimeCoroutine);
    }

    private IEnumerator ProlongLifeTime()
    {     
        yield return new WaitForSeconds(lifeTime);
        mainModule.simulationSpeed = 0;        
    }
}

