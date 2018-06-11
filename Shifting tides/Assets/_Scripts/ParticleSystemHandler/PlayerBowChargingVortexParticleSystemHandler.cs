using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowChargingVortexParticleSystemHandler : ParticleSystemHandler
{

    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.NoiseModule noiseModule;
    private float lerpSpeed;
    private bool stageChanged;

    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GameObject.Find("ChargingVortex").GetComponent<ParticleSystem>();
        Debug.Log(particleSystemToManage);
        mainModule = particleSystemToManage.main;
        Debug.Log(mainModule);
        emissionModule = particleSystemToManage.emission;
        shapeModule = particleSystemToManage.shape;
        noiseModule = particleSystemToManage.noise;

        //0;currentLifeTime , 1:currentSimulationSpeed, 2: currentEmissionRate,3 : currentRandomPositionAmount, 4: currentNoisePositionAmount
        //5:currentNoiseFrequency;
        Debug.Log(mainModule.startLifetime.constant);
        currentProfielValues[0] = mainModule.startLifetime.constant;
        currentProfielValues[1] = mainModule.simulationSpeed;
        currentProfielValues[2] = emissionModule.rateOverTime.constant;
        currentProfielValues[3] = shapeModule.randomPositionAmount;
        currentProfielValues[4] = noiseModule.positionAmount.constant;
        currentProfielValues[5] = noiseModule.frequency;
        lerpSpeed = Time.deltaTime;
        GeneratedDefaultProfiel();
        StartCoroutine(ModuleValueManager());
    }

    protected override void LateUpdate()
    {
        if (stageChanged)
        {
            currentProfielValues[0] = Mathf.Lerp(currentProfielValues[0], targetProfielValues[0], lerpSpeed);
            mainModule.startLifetime = currentProfielValues[0];
            currentProfielValues[1] = Mathf.Lerp(currentProfielValues[1], targetProfielValues[1], lerpSpeed);
            mainModule.simulationSpeed = currentProfielValues[1];

            currentProfielValues[2] = Mathf.Lerp(currentProfielValues[2], targetProfielValues[2], lerpSpeed);
            emissionModule.rateOverTime = currentProfielValues[2];

            currentProfielValues[3] = Mathf.Lerp(currentProfielValues[3], targetProfielValues[3], lerpSpeed);
            shapeModule.randomPositionAmount = currentProfielValues[3];

            currentProfielValues[4] = Mathf.Lerp(currentProfielValues[4], targetProfielValues[4], lerpSpeed);
            noiseModule.positionAmount = currentProfielValues[4];

            currentProfielValues[5] = Mathf.Lerp(currentProfielValues[5], targetProfielValues[5], lerpSpeed);
            noiseModule.frequency = currentProfielValues[5];
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
        targetProfielValues[0] = 9.5f;
        targetProfielValues[1] = 1.2f;
        targetProfielValues[2] = 10;
        targetProfielValues[3] = 0.7f;
        targetProfielValues[4] = 0.4f;
        targetProfielValues[5] = 0.6f;
        lerpSpeed = Time.deltaTime * 1.2f;
    }
    private void ChangeToStateThreeValues()
    {
       targetProfielValues[0] = 9.5f;
       targetProfielValues[1] = 1.5f;
       targetProfielValues[2] = 10;
       targetProfielValues[3] = 0.2f;
       targetProfielValues[4] = 0.2f;
       targetProfielValues[5] = 0.4f;
        lerpSpeed = Time.deltaTime * 1.5f;
    }
    private void ChangeToStateFourValues()
    {
        targetProfielValues[0] = 9.5f;
        targetProfielValues[1] = 4f;
        targetProfielValues[2] = 10;
        targetProfielValues[3] = 0f;
        targetProfielValues[4] = 0f;
        targetProfielValues[5] = 0;
        lerpSpeed = Time.deltaTime * 1.5f;
    }



}