using UnityEngine;
using System.Collections;

public class YiAgentAnimatorManager : AgentAnimatorManager
{
    public void PlayHeadSlamAnimation()
    {
        agentAnimator.SetTrigger("HeadSlamed");
    }

    public void PlayTailSwipAnimation()
    {
        agentAnimator.SetTrigger("TailSwiped");
    }

}
