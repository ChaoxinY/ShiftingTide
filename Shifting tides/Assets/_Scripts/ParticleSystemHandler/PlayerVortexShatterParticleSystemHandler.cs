using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVortexShatterParticleSystemHandler : ParticleSystemHandler
{
    private PlayerAimModule playerAimModule;
    private ParticleSystem.EmissionModule emissionModule;

    public int minBurstCount;
    public int maxBurstCount;

    protected override void InitializeParticleSystem()
    {
        playerAimModule = GameObject.Find("Player").GetComponent<PlayerAimModule>();
        particleSystemToManage = GameObject.Find("VortexShatter").GetComponent<ParticleSystem>();
        emissionModule = particleSystemToManage.emission;
        currentProfielValues[0] = minBurstCount;
        GeneratedDefaultProfiel();
    }
    protected override void LateUpdate()
    {
        currentProfielValues[0] = Mathf.Clamp(playerAimModule.arrowSpeed / 5f, minBurstCount, maxBurstCount);
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0.01f, currentProfielValues[0]);
        emissionModule.SetBurst(0, burst);

    }
    public override void StopParticleAnimation()
    {
        base.StopParticleAnimation();
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0.01f, currentProfielValues[0]);
        emissionModule.SetBurst(0, burst);

    }
}
