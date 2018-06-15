using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVortexShatterParticleSystemHandler : ParticleSystemHandler {

    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GameObject.Find("VortexShatter").GetComponent<ParticleSystem>();
    }

}
