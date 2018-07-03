using UnityEngine;
using System.Collections;

public class PlayerBlinkerParticleSystemHandler : ParticleSystemHandler
{
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.Particle[] particles;
    private IEnumerator prolongParticleLifeTimeCoroutine;
    private float lifeTime;

    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GetComponent<ParticleSystem>();
        mainModule = particleSystemToManage.main;
        lifeTime = mainModule.startLifetime.constant;
        currentProfielValues[0] = mainModule.simulationSpeed;
        particles = new ParticleSystem.Particle[particleSystemToManage.main.maxParticles];
    }
    public override void PlayParticleAnimation()
    {
        base.PlayParticleAnimation();
        prolongParticleLifeTimeCoroutine = ProlongLifeTime();
        mainModule.simulationSpeed = currentProfielValues[0];
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
        yield return new WaitForSeconds(lifeTime*0.9f);
        currentProfielValues[0] = 0;
        mainModule.simulationSpeed = currentProfielValues[0];
    }
}
