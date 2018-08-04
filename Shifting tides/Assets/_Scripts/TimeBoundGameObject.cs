using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeBoundGameObject : MonoBehaviour
{
    public bool isTimeStopped;

    protected GameManager gameMng;
    protected MeshRenderer meshRenderer;
    protected bool isRunningLocalUpdate; 

    private void Start()
    {      
        Initialize();        
    }
    protected virtual void Initialize()
    {
        gameMng = GameObject.Find("GameManager").GetComponent<GameManager>();
        meshRenderer = GetComponent<MeshRenderer>();        
    }

    protected virtual void FixedUpdate()
    {
        if (!isRunningLocalUpdate)
        {
            ShiftLocalUpdateState();
            StartCoroutine(LocalUpdateController());
        }
    }

    protected IEnumerator LocalUpdateController() {

        yield return StartCoroutine(LocalUpdate());
        ShiftLocalUpdateState();
    }

    protected virtual IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(PauseOnTimeStop());
        yield break;
    }

    protected virtual IEnumerator PauseOnTimeStop() {

        yield break;
    }

    protected void ShiftLocalUpdateState() {
        isRunningLocalUpdate = !isRunningLocalUpdate;
    }
}

