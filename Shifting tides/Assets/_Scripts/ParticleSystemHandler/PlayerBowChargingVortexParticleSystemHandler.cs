using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowChargingVortexParticleSystemHandler : ParticleSystemHandler
{

    ParticleSystem.MainModule mainModule;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.ShapeModule shapeModule;
    ParticleSystem.NoiseModule noiseModule;
    private ParticleSystem.MinMaxCurve currentLifeTime, targetLifeTime, currentEmissionRate, targetEmissionRate,
        currentNoisePositionAmount, targetNoisePoistionAmount;
    private float currentSimulationSpeed, targetSimulationSpeed, currentRandomPositionAmount, targetRandomPositionAmount,
         currentNoiseFrequency, targetNoiseFrequency, lerpSpeed;
    private bool stageChanged;


    protected override void LateUpdate()
    {
        if (stageChanged)
        {
            currentLifeTime = Mathf.Lerp(currentLifeTime.constant, targetLifeTime.constant, lerpSpeed);
            mainModule.startLifetime = currentLifeTime;
            currentSimulationSpeed = Mathf.Lerp(currentSimulationSpeed, targetSimulationSpeed, lerpSpeed);
            mainModule.simulationSpeed = currentSimulationSpeed;

            currentEmissionRate = Mathf.Lerp(currentEmissionRate.constant, targetEmissionRate.constant, lerpSpeed);
            emissionModule.rateOverTime = currentEmissionRate;

            currentRandomPositionAmount = Mathf.Lerp(currentRandomPositionAmount, targetRandomPositionAmount, lerpSpeed);
            shapeModule.randomPositionAmount = currentRandomPositionAmount;

            currentNoisePositionAmount = Mathf.Lerp(currentNoisePositionAmount.constant, targetNoisePoistionAmount.constant, lerpSpeed);
            noiseModule.positionAmount = currentNoisePositionAmount;

            currentNoiseFrequency = Mathf.Lerp(currentNoiseFrequency, targetNoiseFrequency, lerpSpeed);
            noiseModule.frequency = currentNoiseFrequency;
        }
    }

    private IEnumerator ModuleValueManager()
    {
        yield return new WaitForSeconds(1f);
        stageChanged = true;
        ChangeToStateTwoValues();
        yield return new WaitForSeconds(0.7f);
        ChangeToStateThreeValues();
        yield return new WaitForSeconds(1.5f);
        ChangeToStateFourValues();
        yield break;
    }

    private void ChangeToStateTwoValues()
    {
        targetLifeTime.constant = 9.5f;
        targetSimulationSpeed = 1.2f;
        targetEmissionRate = 10;
        targetRandomPositionAmount = 0.7f;
        targetNoisePoistionAmount = 0.4f;
        targetNoiseFrequency = 0.6f;
        lerpSpeed = Time.deltaTime * 1.2f;
    }
    private void ChangeToStateThreeValues()
    {
        targetLifeTime = 9.5f;
        targetSimulationSpeed = 1.5f;
        targetEmissionRate = 10;
        targetRandomPositionAmount = 0.2f;
        targetNoisePoistionAmount = 0.2f;
        targetNoiseFrequency = 0.4f;
        lerpSpeed = Time.deltaTime * 1.5f;
    }
    private void ChangeToStateFourValues()
    {
        targetLifeTime = 9.5f;
        targetSimulationSpeed = 4f;
        targetEmissionRate = 10;
        targetRandomPositionAmount = 0f;
        targetNoisePoistionAmount = 0f;
        targetNoiseFrequency = 0;
        lerpSpeed = Time.deltaTime * 1.5f;
    }

    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GameObject.Find("ChargingVortex").GetComponent<ParticleSystem>();
        mainModule = particleSystemToManage.main;
        emissionModule = particleSystemToManage.emission;
        shapeModule = particleSystemToManage.shape;
        noiseModule = particleSystemToManage.noise;

        //0;currentLifeTime , 1:currentSimulationSpeed
        currentLifeTime = mainModule.startLifetime;
        currentSimulationSpeed = mainModule.simulationSpeed;
        currentEmissionRate = emissionModule.rateOverTime;
        currentRandomPositionAmount = shapeModule.randomPositionAmount;
        currentNoisePositionAmount = noiseModule.positionAmount;
        currentNoiseFrequency = noiseModule.frequency;
        lerpSpeed = Time.deltaTime;

    }

    protected override void GeneratedDefaultProfiel()
    {
        throw new NotImplementedException();
    }

    protected override void ResetCurrentProfiel()
    {
        throw new NotImplementedException();
    }
}