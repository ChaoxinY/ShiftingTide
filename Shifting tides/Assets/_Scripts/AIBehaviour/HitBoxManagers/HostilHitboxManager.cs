using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HostilHitboxManager : MonoBehaviour
{
    protected List<HostileHitbox> hitBoxesToManage = new List<HostileHitbox>();
    protected AgentAnimatorManager agentAnimatorManager;

    private void Start()
    {
        Component[] childHostileHitBoxes = gameObject.GetComponentsInChildren(typeof(HostileHitbox));
        foreach (HostileHitbox childHostileHitBox in childHostileHitBoxes)
        {
            hitBoxesToManage.Add(childHostileHitBox);
        }
        agentAnimatorManager = GetComponent<AgentAnimatorManager>();
    }
}
