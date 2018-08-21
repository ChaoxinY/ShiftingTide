using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HostilHitboxManager : MonoBehaviour
{
    protected List<HostileHitBox> hitBoxesToManage = new List<HostileHitBox>();
    protected AgentAnimatorManager agentAnimatorManager;

    private void Start()
    {
        Component[] childHostileHitBoxes = gameObject.GetComponentsInChildren(typeof(HostileHitBox));
        foreach (HostileHitBox childHostileHitBox in childHostileHitBoxes)
        {
            hitBoxesToManage.Add(childHostileHitBox);
        }
        agentAnimatorManager = GetComponent<AgentAnimatorManager>();
    }
}
