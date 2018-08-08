using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultParticleSystemManager : ParticleSystemHandler
{
    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GetComponent<ParticleSystem>();
    }
}
