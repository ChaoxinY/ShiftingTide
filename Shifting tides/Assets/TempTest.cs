using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTest : MonoBehaviour {

    public ParticleSystem ps;

    void Start()
    {
       // ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        ParticleSystem.MainModule main = ps.main;
        main.startLifetime = Mathf.Lerp(main.startLifetime.constant, 10, 0.5f);
    }
}
