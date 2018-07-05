﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkillModule : PlayerModule
{
    private Queue<GameObject> availableArrowheads = new Queue<GameObject>();
    private GameObject defaultArrowHead;
    private PlayerTideComboManager playerTideComboManager;
    private PlayerPhysicsModule playerPhysicsModule;
    private PlayerAimModule playerAimModule;
    private bool dashWarmUp;

    public GameObject[] dashesImages , arrowheadPrefabs;
    public float dashForce, dashLimit;
    public bool isTimeStopped;

    protected override void Initialize()
    {
        playerTideComboManager = GetComponentInParent<PlayerTideComboManager>();
        playerPhysicsModule = GameObject.Find("Player").GetComponentInChildren<PlayerPhysicsModule>();
        playerAimModule = GameObject.Find("Player").GetComponentInChildren<PlayerAimModule>();
        InitializeArrowheads();
        defaultArrowHead = availableArrowheads.Peek();
        playerAimModule.currentArrowhead = defaultArrowHead;
    }

    public override void InitializeModuleID()
    {
        ModuleID = 3;
    }

    public override void ModuleUpdate()
    {
        GatherSource();

        if (Input.GetKeyDown(KeyCode.F) && PlayerResourcesManager.IsThereEnoughResource(3, 0) && !dashWarmUp)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Space) && !playerPhysicsModule.onGround)
        {
            Jumping();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchArrowHead();
        }
    }

    private IEnumerator Dash()
    {
        dashWarmUp = true;
        yield return new WaitUntil(() => IsMoving());
        dashWarmUp = false;
        playerTideComboManager.StartCombo();
        PlayerResourcesManager.Dashes -= 1;
        dashesImages[(int)PlayerResourcesManager.Dashes].SetActive(false);
        playerPhysicsModule.moveForce = dashForce;
        playerPhysicsModule.speedLimit = dashLimit;
        playerPhysicsModule.maxInput = 1f;
        yield return new WaitForSeconds(0.15f);
        playerPhysicsModule.moveForce = playerPhysicsModule.defaultMoveForce;
        while (playerPhysicsModule.speedLimit > playerPhysicsModule.runLimit + 0.2f)
        {
            playerPhysicsModule.speedLimit
                = Mathf.Lerp(playerPhysicsModule.speedLimit, playerPhysicsModule.runLimit, Time.deltaTime * 5f);

            playerPhysicsModule.maxInput = Mathf.Lerp(playerPhysicsModule.maxInput, 0.5f, Time.deltaTime * 5f);
            yield return new WaitForSeconds(0.03f);
        }
        yield break;
    }
    private void InitializeArrowheads()
    {
        foreach (GameObject arrowhead in arrowheadPrefabs)
        {
            availableArrowheads.Enqueue(arrowhead);
        }
    }
    private void SwitchArrowHead()
    {
        availableArrowheads.Enqueue(availableArrowheads.Dequeue());
        playerAimModule.currentArrowhead = availableArrowheads.Peek();
    }

    private void GatherSource()
    {
        RaycastHit[] hits;
        int layerMask = 1 << 10;
        hits = Physics.SphereCastAll(transform.position, 3f, transform.forward, 1f, layerMask);
        foreach (RaycastHit rh in hits)
        {
            SourcePoint sP = rh.collider.gameObject.GetComponent<SourcePoint>();
            sP.objectToChase = gameObject;
            sP.movementSpeed = 20f;
        }
    }
    private void Jumping()
    {
       if (PlayerResourcesManager.IsThereEnoughResource(2, 0)  && isTimeStopped)
        {
            Vector3 spawnPosition = gameObject.transform.position - new Vector3(0, 1.2f, 0) + transform.forward * playerPhysicsModule.speedLimit / 2.4f;
            if (!IsMoving())
            {
                spawnPosition = gameObject.transform.position - new Vector3(0, 1f, 0);
            }
            PlayerResourcesManager.JumpsLeft -= 1;
            Instantiate(Resources.Load("Prefabs/SourcePlatform") as GameObject, spawnPosition, Quaternion.identity);
        }
    }

    private bool IsMoving()
    {
        return (playerPhysicsModule.horizontalInput != 0) || (playerPhysicsModule.verticalInput != 0);
    }

 
}
