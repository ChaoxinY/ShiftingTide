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
        if (blinkerAnimationCoroutine != null)
        {
            StopCoroutine(blinkerAnimationCoroutine);
        }
        playerAimModule.arrowChargingState = 0;        
    }

    public IEnumerator SpawnBlinkers(int amount) {
        Debug.Log(amount);
        for (int i = 0; i < amount; i++) {
            blinkers[i].PlayParticleAnimation();
            activedBlinkers.Add(blinkers[i]);
            yield return new WaitForSeconds(0.45f);
        }
        playerAimModule.arrowChargingState = amount;
        // if blinker amount < 4, play the rest of the blinker animation.
    }

    private IEnumerator BlinkerAnimation() {

        yield return new WaitForSeconds(0.5f/(1+ blinkerCooldownReduction));
        blinkers[0].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[0]);
        playerAimModule.arrowChargingState = 1;

        yield return new WaitForSeconds(0.6f/(1 + blinkerCooldownReduction));
        blinkers[1].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[1]);
        playerAimModule.arrowChargingState = 2;

        yield return new WaitForSeconds(0.4f/(1 + blinkerCooldownReduction));
        blinkers[2].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[2]);
        playerAimModule.arrowChargingState = 3;

        yield return new WaitForSeconds(0.3f/(1 + blinkerCooldownReduction));
        blinkers[3].PlayParticleAnimation();
        activedBlinkers.Add(blinkers[3]);
        playerAimModule.arrowChargingState = 4;

        yield break;
    }

}
