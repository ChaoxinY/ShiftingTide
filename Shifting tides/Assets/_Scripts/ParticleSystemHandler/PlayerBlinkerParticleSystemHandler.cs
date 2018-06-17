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
        particles = new ParticleSystem.Particle[particleSystemToManage.main.maxParticles];
    }
    public override void PlayParticleAnimation()
    {
        base.PlayParticleAnimation();
        mainModule.loop = false;
        prolongParticleLifeTimeCoroutine = ProlongLifeTime();
        StartCoroutine(prolongParticleLifeTimeCoroutine);
    }
    public override void StopParticleAnimation()
    {
        base.StopParticleAnimation();
        particleSystemToManage.Clear();
        StopCoroutine(prolongParticleLifeTimeCoroutine);
        Debug.Log(prolongParticleLifeTimeCoroutine.Current);
    }
    private IEnumerator ProlongLifeTime()
    {
        yield return new WaitForSeconds(lifeTime * 0.6f);
    
        while (true) {
            int numParticlesAlive = particleSystemToManage.GetParticles(particles);       
            for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].remainingLifetime += 0.5f;
        }

        particleSystemToManage.SetParticles(particles, numParticlesAlive);
        yield return new WaitForSeconds(0.5f);
        }

    }
}
