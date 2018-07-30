using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkillModule : PlayerModule
{
    private Queue<GameObject> availableArrowheads = new Queue<GameObject>();
    private GameObject defaultArrowHead;
    private PlayerParticleSystemManager playerParticleSystemManager;
    private PlayerTideComboManager playerTideComboManager;
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerPhysicsModule playerPhysicsModule;
    private PlayerAimModule playerAimModule;
    private Ui ui;

    public GameObject[] dashesImages, arrowheadPrefabs;
    public float dashForce, dashLimit;
    public bool isTimeStopped;

    protected override void Initialize()
    {
        playerParticleSystemManager = GetComponentInParent<PlayerParticleSystemManager>();
        playerTideComboManager = GetComponentInParent<PlayerTideComboManager>();
        playerAnimatorManager = GameObject.Find("Player").GetComponent<PlayerAnimatorManager>();
        playerPhysicsModule = GameObject.Find("Player").GetComponentInChildren<PlayerPhysicsModule>();
        playerAimModule = GameObject.Find("Player").GetComponentInChildren<PlayerAimModule>();
        ui = GameObject.Find("UI").GetComponentInChildren<Ui>();
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

        if (Input.GetKeyDown(KeyCode.F) && PlayerResourcesManager.IsThereEnoughResource(3, 0) && IsMoving())
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Space) && !playerPhysicsModule.onGround)
        {
            CreatePlatform();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
          SwitchArrowHead();
        }
    }
    public void SwitchArrowHead()
    {
        int bowChargeState = playerAimModule.arrowChargingState;
        GameObject startingArrowHead = availableArrowheads.Peek();
        do
        {
            availableArrowheads.Enqueue(availableArrowheads.Dequeue());
            playerAimModule.currentArrowhead = availableArrowheads.Peek();
        }
        while (!playerAimModule.CheckIfCurrentArrowIsAvailable(availableArrowheads.Peek().name));

        if (playerAimModule.currentArrowhead == startingArrowHead)
        {

            return;
        }

        playerParticleSystemManager.StopAllShootingParticleSystems();

        playerParticleSystemManager.SetCurrentArrowParticleSystem();
        ui.DisplayCurrentArrowHead(playerAimModule.currentArrowhead);
        if (bowChargeState > 0)
        {
            playerParticleSystemManager.PlayChargingAnimation();
            playerParticleSystemManager.InherentBlinkerCount(bowChargeState);
        }

    }
    private IEnumerator Dash()
    {
        playerAnimatorManager.PlayDashAnimation();
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
    private void CreatePlatform()
    {
        if (PlayerResourcesManager.IsThereEnoughResource(2, 0) && isTimeStopped)
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
