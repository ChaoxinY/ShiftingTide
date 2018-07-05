using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerParticleSystemManager : MonoBehaviour
{
    //0 :charing vortex ,1 : cone radiator  2: conde shatter 3： cone radiator core 
    //4 : Blinkers 
    public ParticleSystemHandler[] managedSystems;
    public List<ParticleSystemHandler> activeSystems = new List<ParticleSystemHandler>();
    public bool isPlayingChargingAnimation,isPlayingChargedUpAnimation;

    public void StopAllShootingParticleSystems() {

        if (activeSystems.Count == 0 ) {
            return;
        }

        foreach (ParticleSystemHandler activeSystem in activeSystems) {
            activeSystem.StopParticleAnimation();
        }
        isPlayingChargingAnimation = false;
        isPlayingChargedUpAnimation = false;
        activeSystems.Clear();
    }

    public void PlayChargingAnimation()
    {
        managedSystems[0].PlayParticleAnimation();
        managedSystems[1].PlayParticleAnimation();
        managedSystems[3].PlayParticleAnimation();
        AddToList(activeSystems, managedSystems[0], managedSystems[1], managedSystems[3]);
        isPlayingChargingAnimation = true;
    }

    public void PlayChargedUpAnimation() {
        managedSystems[4].PlayParticleAnimation();
        AddToList(activeSystems, managedSystems[4]);
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
        AddToList(activeSystems, managedSystems[2]);
        yield return new WaitForSeconds(0.1f);
        managedSystems[2].StopParticleAnimation();
        isPlayingChargingAnimation = false;
        activeSystems.Clear();
        yield break;
    }

    private void AddToList(List<ParticleSystemHandler> activeSystem, params ParticleSystemHandler[] elements) {

        activeSystem.AddRange(elements);
    }

}
