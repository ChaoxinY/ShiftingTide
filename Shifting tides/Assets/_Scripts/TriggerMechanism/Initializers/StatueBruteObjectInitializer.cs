using UnityEngine;
using System.Collections;

public class StatueBruteObjectInitializer : AgentInitializer
{
    protected override IEnumerator InitializeObject()
    {
        yield return StartCoroutine(base.InitializeObject());
        GetComponentInParent<StatueBrute>().SwitchToCombatMode();
        Destroy(gameObject);
    }
}
