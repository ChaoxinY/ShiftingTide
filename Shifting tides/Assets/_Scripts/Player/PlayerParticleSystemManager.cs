using UnityEngine;
using System.Collections;

public class PlayerParticleSystemManager : MonoBehaviour
{
    //0 :charing vortex ,1 : cone radiator  2: conde shatter 3： cone radiator core 
    //4 : Blinkers 
    public ParticleSystemHandler[] managedSystems;
    public bool isPlayingChargingAnimation,isPlayingChargedUpAnimation;

    public void StopAllShootingParticleSystems() {
        for (int i = 0; i < 5; i ++) {
            managedSystems[i].StopParticleAnimation();
            isPlayingChargingAnimation = false;
            isPlayingChargedUpAnimation = false;
        }
    }

    public void PlayChargingAnimation()
    {
        managedSystems[0].PlayParticleAnimation();
        managedSystems[1].PlayParticleAnimation();
        managedSystems[3].PlayParticleAnimation();
        isPlayingChargingAnimation = true;
    }

    public void PlayChargedUpAnimation() {
        managedSystems[4].PlayParticleAnimation();
        isPlayingChargedUpAnimation = true;
    }

    public IEnumerator PlayerFireAnimation()
    {
        managedSystems[0].StopParticleAnimation();
        managedSystems[1].StopParticleAnimation();
        managedSystems[3].StopParticleAnimation();
        if (isPlayingChargedUpAnimation)
        {
            managedSystems[4].StopParticleAnimation();
            isPlayingChargedUpAnimation = false;
        }
        managedSystems[2].PlayParticleAnimation();
        yield return new WaitForSeconds(0.1f);
        managedSystems[2].StopParticleAnimation();
        isPlayingChargingAnimation = false;
        yield break;
    }

}
