using UnityEngine;
using System.Collections;

public static class StaticToolMethods
{
    public static AnimatorStateInfo GetAnimatorStateInfo(int layerIndex,Animator animatorTocheck)
    {
        AnimatorStateInfo currentAnimatorStateInfo = animatorTocheck.GetCurrentAnimatorStateInfo(layerIndex);
        return currentAnimatorStateInfo;
    }
}
