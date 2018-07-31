using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnowStormParticleSystemHandler : ParticleSystemHandler
{
    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GetComponent<ParticleSystem>();
    }
}
