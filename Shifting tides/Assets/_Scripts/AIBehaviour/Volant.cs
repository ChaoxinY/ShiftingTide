using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volant : Agent
{
    public Transform[] activityBounds;

    protected override void InitializeLists()
    {
        base.InitializeLists();
        spontaneousBehaviours.Add("Roaming");
        patternedBehaviours.Add("AirCircling");
        patternedBehaviours.Add("Patroling");
        restingBehaviours.Add("AirRest");
        restingBehaviours.Add("GroundRest");
    }

    protected IEnumerator AirCircling() {

        yield break;
    }
    protected IEnumerator AirRest()
    {

        yield break;
    }
    protected IEnumerator Dashing()
    {

        yield break;
    }
    protected IEnumerator GroundRest()
    {

        yield break;
    }
    protected IEnumerator Patroling()
    {

        yield break;
    }
    protected IEnumerator Roaming()
    {

        yield break;
    }
  
}
