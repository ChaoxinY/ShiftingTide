using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightingVortexParticleSystemHandler : ParticleSystemHandler
{
    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GetComponent<ParticleSystem>();
    }
}
