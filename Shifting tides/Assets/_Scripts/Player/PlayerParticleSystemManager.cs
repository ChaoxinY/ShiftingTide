using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerParticleSystemManager : MonoBehaviour
{  
    public ParticleSystemHandler[] managedSystems;
    //0 :charing vortex ,1 : cone radiator  2: conde shatter 3： cone radiator core 
    //4 : Blinkers 
    public PlayerArrowParticleSystemManager[] managedPlayerArrowParticle;
    public PlayerArrowParticleSystemManager currentPlayerArrowParticle;
    public PlayerAimModule playerAimModule;
    public bool isPlayingChargingAnimation, isPlayingChargedUpAnimation;

    public void StopAllShootingParticleSystems()
    {   
        isPlayingChargingAnimation = false;
        isPlayingChargedUpAnimation = false;
     
        currentPlayerArrowParticle.StopAllShootingParticleSystems();
    }

    public void InherentBlinkerCount(int amount)
    {
        isPlayingChargedUpAnimation = true;
        foreach (ParticleSystemHandler particleSystemHandler in currentPlayerArrowParticle.managedSystems)
        {
            if (particleSystemHandler is PlayerBlinkersManager)
            {               
                PlayerBlinkersManager playerBlinkersManager = particleSystemHandler.gameObject.GetComponent<PlayerBlinkersManager>();
                playerBlinkersManager.SpawnBlinkers(amount);
            }
        }
    }

   
    public void PlayChargingAnimation()
    {
        isPlayingChargingAnimation = true;
        currentPlayerArrowParticle.PlayChargingAnimation();
    }

    public void PlayChargedUpAnimation()
    {
        isPlayingChargedUpAnimation = true;
        currentPlayerArrowParticle.PlayChargedUpAnimation();
    }

    public void PlayDashAnimation() {
        managedSystems[0].PlayParticleAnimation();
    }


   public void PlayerFireAnimation()
    {
        StartCoroutine(currentPlayerArrowParticle.PlayerFireAnimation());
        isPlayingChargingAnimation = false;
    }

    public void SetCurrentArrowParticleSystem()
    {
        switch (playerAimModule.currentArrowhead.name)
        {

            case "DefaultArrow":
                currentPlayerArrowParticle = managedPlayerArrowParticle[0];
                break;
            case "TimeZoneArrow":
                currentPlayerArrowParticle = managedPlayerArrowParticle[1];
                break;
            case "CloudArrow":
                currentPlayerArrowParticle = managedPlayerArrowParticle[2];
                break;
        }

    }
}
