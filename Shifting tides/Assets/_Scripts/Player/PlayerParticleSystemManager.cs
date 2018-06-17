﻿using UnityEngine;
using System.Collections;

public class PlayerParticleSystemManager : MonoBehaviour
{
    //0 :charing vortex ,1 : cone radiator  2: conde shatter 3： cone radiator core 
    public ParticleSystemHandler[] managedSystems;
    public bool isPlayingChargingAnimation;

    public void PlayChargingAnimation()
    {
        managedSystems[0].PlayParticleAnimation();
        managedSystems[1].PlayParticleAnimation();
        managedSystems[3].PlayParticleAnimation();
        isPlayingChargingAnimation = true;
    }

    public IEnumerator PlayerFireAnimation()
    {
        managedSystems[0].StopParticleAnimation();
        managedSystems[1].StopParticleAnimation();
        managedSystems[3].StopParticleAnimation();
        managedSystems[2].PlayParticleAnimation();
        yield return new WaitForSeconds(0.1f);
        managedSystems[2].StopParticleAnimation();
        isPlayingChargingAnimation = false;
        yield break;
    }

}
