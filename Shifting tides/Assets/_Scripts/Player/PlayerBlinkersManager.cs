using UnityEngine;
using System.Collections;

public class PlayerBlinkersManager : ParticleSystemHandler
{
    public PlayerBlinkerParticleSystemHandler[] blinkers;
    
    protected override void InitializeParticleSystem()
    {
      
    }

    public override void PlayParticleAnimation()
    {
        StartCoroutine(BlinkerAnimation());
    }

    public override void StopParticleAnimation()
    {
       StartCoroutine(StopBlinkerAnimation());
    }

    private IEnumerator BlinkerAnimation() {

        foreach (PlayerBlinkerParticleSystemHandler blinker in blinkers)
        {
            blinker.PlayParticleAnimation();
            yield return new WaitForSeconds(1.5f);
        }
        yield break;
    }

    private IEnumerator StopBlinkerAnimation()
    {

        foreach (PlayerBlinkerParticleSystemHandler blinker in blinkers)
        {
            blinker.StopParticleAnimation();
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
}
