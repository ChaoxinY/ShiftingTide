using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardInteractiveGameObject : TimeBoundGameObject {

    protected override IEnumerator PauseOnTimeStop()
    {
        if (isTimeStopped)
        {
            yield return new WaitUntil(() => !isTimeStopped);
        }
        yield break;
    }

}
