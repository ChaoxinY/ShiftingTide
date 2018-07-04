using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBlinkersManager : ParticleSystemHandler
{
    public PlayerBlinkerParticleSystemHandler[] blinkers;
    public float blinkerCooldownReduction;

    private List<PlayerBlinkerParticleSystemHandler> activedBlinkers =  new List<PlayerBlinkerParticleSystemHandler>();
    private IEnumerator blinkerAnimationCoroutine;
    private PlayerAimModule playerAimModule;

    protected override void InitializeParticleSystem()
    {
        playerAimModule = GameObject.Find("Player").GetComponentInChildren<PlayerAimModule>();
    }

    public override void PlayParticleAnimation()
    {
        blinkerAnimationCoroutine = BlinkerAnimation();
        StartCoroutine(blinkerAnimationCoroutine);
    }

    public override void StopParticleAnimation()
    {
        foreach (PlayerBlinkerParticleSystemHandler blinker in activedBlinkers)
        {
            blinker.StopParticleAnimation();
        }
        activedBlinkers.Clear();
        StopCoroutine(blinkerAnimationCoroutine);
        playerAimModule.arrowSpeed = 0f;
        playerAimModule.arrowBaseDamage = 0;    
    }

    private IEnumerator BlinkerAnimation() {

        yield return new WaitForSeconds(0.5f/(1+ blinkerCooldownReduction));
        blinkers[0].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[0]);
        playerAimModule.arrowSpeed = 25f;
        playerAimModule.arrowBaseDamage += 10;

        yield return new WaitForSeconds(0.6f/(1 + blinkerCooldownReduction));
        blinkers[1].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[1]);
        playerAimModule.arrowSpeed = 35f;
        playerAimModule.arrowBaseDamage += 15;

        yield return new WaitForSeconds(0.4f/(1 + blinkerCooldownReduction));
        blinkers[2].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[2]);
        playerAimModule.arrowSpeed = 60f;
        playerAimModule.arrowBaseDamage += 25;

        yield return new WaitForSeconds(0.3f/(1 + blinkerCooldownReduction));
        blinkers[3].PlayParticleAnimation();
        playerAimModule.arrowSpeed = 100f;
        playerAimModule.arrowBaseDamage += 50;
        activedBlinkers.Add(blinkers[3]);      
        yield break;
    }

}
