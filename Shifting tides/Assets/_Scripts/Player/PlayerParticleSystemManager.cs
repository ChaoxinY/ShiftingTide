using UnityEngine;
using System.Collections;

public class PlayerParticleSystemManager : MonoBehaviour
{   
    //0 :charing vortex ,1 : cone radiator  2: conde shartter
    public ParticleSystemHandler[] managedSystems;
    public bool isPlayingChargingAnimation;

    public void  PlayChargingAnimation() {
        managedSystems[0].PlayParticleAnimation();
        managedSystems[1].PlayParticleAnimation();
        isPlayingChargingAnimation = true;
    }

    public void PlayerFireAnimation() {

        managedSystems[0].StopParticleAnimation();
        managedSystems[1].StopParticleAnimation();
        managedSystems[2].PlayParticleAnimation();
        isPlayingChargingAnimation = false;
    }

}
