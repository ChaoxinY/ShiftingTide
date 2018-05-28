﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Agent : StandardInteractiveGameObject
{

    protected const float boredomInfluencePoint = 0.2f, conscientiousnessInfluencePoint = 0.2f, ExhaustionInfluencePoint = 0.05f, RestInfluencePoint = 0.4f;
    protected List<string> spontaneousBehaviours;
    protected List<string> patternedBehaviours;
    protected List<string> restingBehaviours;

    protected Material originMaterial;
    protected Vector3 currentTarget, destination,nearestWaypoint;
    protected string overwritingBehaviour;
    protected string standardBehaviour;
    protected bool StandardBehaviourFinished = true;
    protected bool isResting = false;

    public List<Transform> wayPoints;
    public float currentHealth, maxHealth;
    public float[] desire, factorInfluencePoint = { 0, 0, 0, 0, 0, 0 };
    public bool predictable, energetic, perferRandomBehaviours, perferPatternedBehaviours, customized;    
  
    protected override IEnumerator LocalUpdate()
    {
        yield return StartCoroutine(base.LocalUpdate());
        if (isResting)
        {
            desire[2] = Mathf.Lerp(desire[2], 40, ExhaustionInfluencePoint + factorInfluencePoint[4]);
            DetermineRestingBehaviour();
            if (Vector3.Distance(transform.position, nearestWaypoint) > 5) {
                isResting = false;
                nearestWaypoint = Vector3.zero;
                StandardBehaviourFinished = true;
            }
        }
        if (overwritingBehaviour != null)
        {
            yield return StartCoroutine(overwritingBehaviour);
        }
        if (StandardBehaviourFinished)
        {
            StandardBehaviourFinished = false;
            yield return StartCoroutine(DetermineNextAgentBehaviour());
            StartCoroutine(standardBehaviour);
        }
        yield return new WaitForSeconds(0.5f);

        yield break;
    }

    protected override void Initialize()
    {
        gameMng = GameObject.Find("GameManager").GetComponent<GameManager>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        InitializeLists();
        SetUpAgentFactorInfluencePoint();
        StartCoroutine(DetermineNextAgentBehaviour());
    }
    protected virtual void InitializeLists()
    {
        spontaneousBehaviours = new List<string>();
        patternedBehaviours = new List<string>();
        restingBehaviours = new List<string>();
        //Dont initialze list in scripts if its value is assign in editor 
        //wayPoints = new List<Transform>();
    }

    private void SetUpAgentFactorInfluencePoint()
    {
        if (customized)
        {
            return;
        }

        if (energetic)
        {
            factorInfluencePoint[4] = -0.03f;
            factorInfluencePoint[5] = 0.12f;
        }

        if (predictable)
        {
            if (perferRandomBehaviours)
            {
                factorInfluencePoint[1] = 0.19f;
                factorInfluencePoint[2] = -0.19f;
                return;
            }
            else if (perferPatternedBehaviours)
            {
                factorInfluencePoint[2] = 0.19f;
                factorInfluencePoint[1] = -0.19f;
            }
        }

    }

    private void DetermineRestingBehaviour()
    {
        int NumbersCorrect = 0;
        for (int i = 0; i < 5; i++) {          
                int agentGuessedNumber = Random.Range(0, 99);
                if (agentGuessedNumber < desire[2]) NumbersCorrect++;
            }
        if (NumbersCorrect >= 2) {
            EnterRestingState();
        }
    }
    protected IEnumerator DetermineNextAgentBehaviour()
    {

        //Rethinking current choice
        int[] NumbersCorrect = { 0, 0 };

        for (int i = 0; i < 20; i++)
            for (int j = 0; j < desire.Length-1; j++)
            {
                int agentGuessedNumber = Random.Range(0, 99);
                if (agentGuessedNumber < desire[j]) NumbersCorrect[j]++;
            }
        //Determine which slot has the highest number
        int highestDesire = 0;

        for (int i = 1; i < NumbersCorrect.Length; i++)
        {
            int maxValue = NumbersCorrect[0];
            if (NumbersCorrect[i] > maxValue)
            {
                maxValue = NumbersCorrect[i];
                highestDesire = i;
            }
        }
        // Debug.Log("Highest desire：" + highestDesire);
        //Executing behaviour fit for current desire.
        switch (highestDesire)
        {
            case 0:
                EnterSpontaneousState();
                break;
            case 1:
                EnterPredictableState();
                break;
        }
        yield break;
    }

    protected void EnterSpontaneousState()
    {           
        standardBehaviour = spontaneousBehaviours[Random.Range(0, spontaneousBehaviours.Count)];
        Debug.Log(standardBehaviour);
        desire[0] = Mathf.Lerp(desire[0], 0, boredomInfluencePoint + factorInfluencePoint[0]);
        desire[1] = Mathf.Lerp(desire[1], 100, conscientiousnessInfluencePoint + factorInfluencePoint[2]);
    }

    protected void EnterPredictableState()
    {
        standardBehaviour = patternedBehaviours[Random.Range(0, patternedBehaviours.Count)];
        Debug.Log(standardBehaviour);
        desire[0] = Mathf.Lerp(desire[0], 100, boredomInfluencePoint + factorInfluencePoint[1]);
        desire[1] = Mathf.Lerp(desire[1], 0, conscientiousnessInfluencePoint + factorInfluencePoint[3]);
    }

    protected void EnterRestingState()
    {
        overwritingBehaviour = restingBehaviours[Random.Range(0, restingBehaviours.Count)];
        Debug.Log(overwritingBehaviour);
        desire[2] = Mathf.Lerp(desire[2], 0, RestInfluencePoint + factorInfluencePoint[5]);
    }

    protected bool Arrived(Vector3 gameObject, Vector3 target, float range)
    {
        bool Arrived = false;
        float distance = Vector3.Distance(gameObject, target);
        if (distance < range)
        {
            Arrived = true;
        }
        return Arrived;
    }
    protected void Chase(GameObject hunted)
    {
        //Debug.Log("Chase");
        currentTarget = hunted.transform.position;
        // MoveTowardsTarget(agent, currentTarget);
    }
    protected void Flee(GameObject hunter)
    {
        //Debug.Log("Flee");
        currentTarget = hunter.transform.position + new Vector3(UnityEngine.Random.Range(-50, 50), 0, UnityEngine.Random.Range(-50, 50));
        // MoveTowardsTarget(agent, currentTarget);
    }
    protected void ChangeToOriginMaterial()
    {
        meshRenderer.material = originMaterial;
    }
}



