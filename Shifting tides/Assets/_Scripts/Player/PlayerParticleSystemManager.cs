using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerParticleSystemManager : MonoBehaviour
{  
    //Standard particle systems to manage
    public ParticleSystemHandler[] managedSystems;
    //Available arrow particle systems
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

    //Maintain the current blinker counts when swapping to an another  
    //arrow particle system.
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

    //Switch between different arrow particle systeem based on the 
    //currently selected arrowhead
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
