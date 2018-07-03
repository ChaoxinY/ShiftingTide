using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardInteractiveGameObject : TimeBoundGameObject {

    public bool isTimeStoped = false;

    protected override IEnumerator PauseOnTimeStop()
    {
        if (isTimeStoped)
        {
            yield return new WaitUntil(() => !isTimeStoped);
        }
        yield break;
    }

}
