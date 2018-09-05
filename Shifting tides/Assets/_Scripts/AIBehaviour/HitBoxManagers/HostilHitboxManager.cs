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

    protected HostileHitbox FindHostileHitBox(string hitBoxName) {

        HostileHitbox hitBoxFound = null;
        foreach (HostileHitbox hostileHitBox in hitBoxesToManage) {
            if (hostileHitBox.gameObject.name == hitBoxName) {
                hitBoxFound = hostileHitBox;
                break;
            }
        }
        return hitBoxFound;
    }
}
