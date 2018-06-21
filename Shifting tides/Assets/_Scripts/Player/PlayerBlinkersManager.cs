using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBlinkersManager : ParticleSystemHandler
{
    public PlayerBlinkerParticleSystemHandler[] blinkers;

    private List<PlayerBlinkerParticleSystemHandler> activedBlinkers =  new List<PlayerBlinkerParticleSystemHandler>();
    private IEnumerator blinkerAnimationCoroutine;
    private BasicMovement basicMovement;

    protected override void InitializeParticleSystem()
    {
        basicMovement = GameObject.Find("Player").GetComponent<BasicMovement>();
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
        basicMovement.arrowSpeed = 0f;
        basicMovement.arrowBaseDamage = 0;    
    }

    private IEnumerator BlinkerAnimation() {

        yield return new WaitForSeconds(0.8f);
        blinkers[0].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[0]);
        basicMovement.arrowSpeed = 25f;
        basicMovement.arrowBaseDamage += 10;
        yield return new WaitForSeconds(1.1f);
        blinkers[1].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[1]);
        basicMovement.arrowSpeed = 35f;
        basicMovement.arrowBaseDamage += 15;
        yield return new WaitForSeconds(0.8f);
        blinkers[2].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[2]);
        basicMovement.arrowSpeed = 50f;
        basicMovement.arrowBaseDamage = 25;
        yield return new WaitForSeconds(0.6f);
        blinkers[3].PlayParticleAnimation();
        basicMovement.arrowSpeed = 80f;
        basicMovement.arrowBaseDamage += 50;
        activedBlinkers.Add(blinkers[3]);      
        yield break;
    }

}
