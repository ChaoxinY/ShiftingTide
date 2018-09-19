﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerArrowParticleSystemManager : MonoBehaviour
{
    //0 :charing vortex ,1 : cone radiator  2: conde shatter 3： cone radiator core 
    //4 : Blinkers 
    //Particle systems that make up the whole arrow drawing/ firing particle effects.
    public ParticleSystemHandler[] managedSystems;
    public List<ParticleSystemHandler> activeSystems = new List<ParticleSystemHandler>();
    private PlayerParticleSystemManager playerParticleSystemManager;

    private void Start()
    {
        playerParticleSystemManager = GameObject.Find("Player").GetComponent<PlayerParticleSystemManager>();
    }

    public void StopAllShootingParticleSystems() {

        if (activeSystems.Count == 0 ) {
            return;
        }

        foreach (ParticleSystemHandler activeSystem in activeSystems) {
            activeSystem.StopParticleAnimation();
        }    
        activeSystems.Clear();
    }

    public void PlayChargingAnimation()
    {
        //AddToList(activeSystems, managedSystems[0], managedSystems[1], managedSystems[3]);
        activeSystems.Add(managedSystems[0]);
        activeSystems.Add(managedSystems[1]);
        activeSystems.Add(managedSystems[3]);
        managedSystems[0].PlayParticleAnimation();
        managedSystems[1].PlayParticleAnimation();
        managedSystems[3].PlayParticleAnimation();        
    }

    public void PlayChargedUpAnimation() {
        managedSystems[4].PlayParticleAnimation();
        AddToList(activeSystems, managedSystems[4]);
    }

    public IEnumerator PlayerFireAnimation()
    {   
        managedSystems[0].StopParticleAnimation();
        managedSystems[1].StopParticleAnimation();
        managedSystems[3].StopParticleAnimation();
        if (playerParticleSystemManager.isPlayingChargedUpAnimation)
        {
            managedSystems[4].StopParticleAnimation();
            playerParticleSystemManager.isPlayingChargedUpAnimation = false;
        }    
        managedSystems[2].PlayParticleAnimation();
        AddToList(activeSystems, managedSystems[2]);
        activeSystems.Clear();
        yield break;
    }

    private void AddToList(List<ParticleSystemHandler> activeSystem, params ParticleSystemHandler[] elements) {
        activeSystem.AddRange(elements);
    }

}
