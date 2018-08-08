using UnityEngine;
using System.Collections;

public class PlayerDashParticleSystemHandler : ParticleSystemHandler
{
    public GameObject DashCharm;
    public Transform CharmPosition;

    protected override void InitializeParticleSystem()
    {
        particleSystemToManage = GetComponent<ParticleSystem>();
    }

    public override void PlayParticleAnimation()
    {
        base.PlayParticleAnimation();
        Instantiate(DashCharm, CharmPosition.position,Quaternion.LookRotation(transform.forward));
    }
}

