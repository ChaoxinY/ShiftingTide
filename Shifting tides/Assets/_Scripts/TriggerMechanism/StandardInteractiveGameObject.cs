using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardInteractiveGameObject : TimeBoundGameObject {

    protected override IEnumerator PauseOnTimeStop()
    {
        if (gameMng.isTimeStoped)
        {
            yield return new WaitUntil(() => !gameMng.isTimeStoped);
        }
        yield break;
    }

}
