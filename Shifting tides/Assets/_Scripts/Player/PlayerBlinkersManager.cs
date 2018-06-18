using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBlinkersManager : ParticleSystemHandler
{
    public PlayerBlinkerParticleSystemHandler[] blinkers; 
    private List<PlayerBlinkerParticleSystemHandler> activedBlinkers =  new List<PlayerBlinkerParticleSystemHandler>();
    private IEnumerator blinkerAnimationCoroutine;

    protected override void InitializeParticleSystem()
    {
      
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
    }

    private IEnumerator BlinkerAnimation() {

        foreach (PlayerBlinkerParticleSystemHandler blinker in blinkers)
        {
            blinker.PlayParticleAnimation();
            activedBlinkers.Add(blinker);
            Debug.Log(activedBlinkers.Count);
            yield return new WaitForSeconds(1.5f);
        }
        yield break;
    }

}
