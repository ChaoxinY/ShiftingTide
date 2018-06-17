using UnityEngine;
using System.Collections;

public class PlayerBlinkParticleSystemHandler : ParticleSystemHandler
{
    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GameObject.Find("Blink").GetComponent<ParticleSystem>();
    }
}
