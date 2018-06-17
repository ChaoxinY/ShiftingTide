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
        foreach (PlayerBlinkerParticleSystemHandler blinker in blinkers)
        {
            blinker.StopParticleAnimation();           
        }
    }

    private IEnumerator BlinkerAnimation() {

        foreach (PlayerBlinkerParticleSystemHandler blinker in blinkers)
        {
            blinker.PlayParticleAnimation();
            yield return new WaitForSeconds(2.5f);
        }
        yield break;
    }
}
