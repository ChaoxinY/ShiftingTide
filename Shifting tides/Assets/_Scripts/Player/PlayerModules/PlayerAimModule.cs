using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerAimModule : PlayerModule
{
    private PlayerParticleSystemManager playerParticleSystemManager;
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerCamera plyCamera;
    private Camera cameraMain;
    private Transform nearestTarget, playerTransForm;
    private Vector3 lastDirection;
    private bool lockedOn;

    public Image cursor;
    public Sprite lockOnCursor, lockOffCursor;
    public GameObject bow, bowMesh, shootTarget, currentArrowhead;
    public AudioSource[] bowSoundSources;
    public Transform PlayerChestBone;
    public Vector3 targetOffset;
    public bool isAiming;
    [HideInInspector]
    public int arrowChargingState;

    protected override void Initialize()
    {
        playerParticleSystemManager = GameObject.Find("Player").GetComponentInChildren<PlayerParticleSystemManager>();
        playerAnimatorManager = GameObject.Find("Player").GetComponent<PlayerAnimatorManager>();
        plyCamera = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        cameraMain = Camera.main;
        playerTransForm = GameObject.Find("Player").transform;
        lockedOn = false;
        cursor.sprite = lockOffCursor;
        InitializeModuleID();
        playerParticleSystemManager.SetCurrentArrowParticleSystem();
    }

    public override void InitializeModuleID()
    {
        ModuleID = 2;
    }

    public override void ModuleStartUp()
    {
        cursor.gameObject.SetActive(true);
        plyCamera.pivotOffset = new Vector3(0, 0.5f, 0);
        plyCamera.camOffset = new Vector3(0.55f, 0.5f, -0.9f);
        plyCamera.ResetSmoothOffsets();
        plyCamera.ResetTargetOffsets();
    }

    public override void ModuleRemove()
    {
        isAiming = false;
        //playerParticleSystemManager.StopAllShootingParticleSystems();
        cursor.gameObject.SetActive(false);
        plyCamera.pivotOffset = new Vector3(0, 0.5f, 0);
        plyCamera.camOffset = new Vector3(0, 0.5f, -3);
        plyCamera.ResetSmoothOffsets();
        plyCamera.ResetTargetOffsets();
    }

    public override void ModuleUpdate()
    {
        bow.transform.LookAt(shootTarget.transform);
        if (Input.GetMouseButton(1) && CheckIfCurrentArrowIsAvailable(currentArrowhead.name))
        {
            ChargeUpArrow();
        }

        if (Input.GetMouseButtonUp(1) && isAiming)
        {
            ShootArrow();
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (lockedOn)
            {
                cursor.GetComponent<Image>().sprite = lockOffCursor;
                lockedOn = false;
                nearestTarget = null;
            }
            else { LockOnTarget(); }
        }


        if (lockedOn)
        {
            CheckIfTargetIsInVision();
            shootTarget.transform.position = nearestTarget.position;
        }

        else
        {
            shootTarget.transform.position =
            cameraMain.transform.position + cameraMain.transform.forward * 30f
            + cameraMain.transform.right * targetOffset.x
            + cameraMain.transform.up * targetOffset.y;
        }
    }

    public void LateUpdate()
    {       
        cursor.transform.position = Camera.main.WorldToScreenPoint(shootTarget.transform.position);
        if (isAiming) {
            AimRotate();
        }
    }

    private void ChargeUpArrow()
    {

        if (!isAiming) isAiming = !isAiming;

        playerAnimatorManager.Aiming = isAiming;

        if (playerParticleSystemManager.isPlayingChargingAnimation == false)
        {
            bowSoundSources[1].Play();
            playerParticleSystemManager.PlayChargingAnimation();
        }

        if (playerParticleSystemManager.isPlayingChargedUpAnimation == false)
        {
            playerParticleSystemManager.PlayChargedUpAnimation();
        }

    }

    private void ShootArrow()
    {
        if (isAiming) isAiming = !isAiming;
        playerAnimatorManager.Aiming = isAiming;
        ConsumeCurrentArrowResource();
        GameObject Arrow = Instantiate(currentArrowhead, bow.transform.position, bow.transform.rotation);
        Arrow.GetComponent<ArrowBehaviour>().ApplyArrowStageValues(arrowChargingState);
        Arrow.GetComponent<Rigidbody>().AddForce(Arrow.transform.forward * Arrow.GetComponent<ArrowBehaviour>().arrowSpeed, ForceMode.Impulse);
        playerParticleSystemManager.PlayerFireAnimation();
        bowSoundSources[0].Play();
    }

    private void ConsumeCurrentArrowResource()
    {
        switch (currentArrowhead.name)
        {
            case "DefaultArrow":
                PlayerResourcesManager.Arrows -= 1;
                break;
            case "TimeZoneArrow":
                PlayerResourcesManager.SourceReserve -= 10;
                break;
            case "CloudArrow":
                PlayerResourcesManager.SourceFusedArrows -= 1;
                break;
        }

    }

    public bool CheckIfCurrentArrowIsAvailable(string currentArrowheadName)
    {

        bool arrowAvailable = false;

        switch (currentArrowheadName)
        {
            case "DefaultArrow":
                arrowAvailable = PlayerResourcesManager.IsThereEnoughResource(4, 0);
                break;
            case "TimeZoneArrow":
                arrowAvailable = PlayerResourcesManager.IsThereEnoughResource(1, 0);
                break;
            case "CloudArrow":
                arrowAvailable = PlayerResourcesManager.IsThereEnoughResource(5, 0);
                break;
        }

        return arrowAvailable;
    }

    public void AimRotate()
    {
        Vector3 forward = cameraMain.transform.TransformDirection(Vector3.forward);
        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Always rotates the player according to the camera horizontal rotation in aim mode.
        Quaternion targetRotation = Quaternion.Euler(0, plyCamera.angleH, 0);
        Quaternion ChestRotation = PlayerChestBone.rotation;
        ChestRotation *= Quaternion.Euler(0, -plyCamera.angleV, -plyCamera.angleV/3);

        float minSpeed = Quaternion.Angle(playerTransForm.rotation, targetRotation) * 0.5f;

        // Rotate entire player to face camera.
        lastDirection = forward;
        playerTransForm.rotation = Quaternion.Lerp(playerTransForm.rotation, targetRotation, minSpeed * Time.deltaTime);
        PlayerChestBone.rotation = Quaternion.Lerp(PlayerChestBone.rotation, ChestRotation, 1);
    }

    private RaycastHit[] LookForTarget()
    {
        int layerMask = 1 << 13;
        RaycastHit[] hits;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);
        hits = Physics.BoxCastAll(transform.position, new Vector3(10, 5, 1f), transform.forward, Quaternion.LookRotation(transform.forward), 20f, layerMask);
        return hits;
    }

    private void CheckIfTargetIsInVision()
    {
        Transform lastTarget = nearestTarget;

        RaycastHit[] hits;
        hits = LookForTarget();
        bool TargetInSight = false;
        foreach (RaycastHit rh in hits)
        {
            if (rh.collider.gameObject.transform == lastTarget)
            {
                TargetInSight = true;
                return;
            }
        }

        if (!TargetInSight)
        {
            Debug.Log("Target OutofSight");
            lockedOn = false;
            cursor.GetComponent<Image>().sprite = lockOffCursor;
        }
    }

    private void LockOnTarget()
    {
        RaycastHit[] hits;
        hits = LookForTarget();
        float closestDistanceSqr = Mathf.Infinity;
        if (hits.Length == 0)
        {
            cursor.GetComponent<Image>().sprite = lockOffCursor;
            lockedOn = false;
            Debug.Log(hits.Length.ToString());
            return;
        }

        foreach (RaycastHit rh in hits)
        {
            Transform transform = rh.collider.gameObject.GetComponent<Transform>();
            if (transform != null)
            {
                Vector3 directionToTarget = rh.collider.gameObject.transform.position - gameObject.transform.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    nearestTarget = rh.collider.gameObject.transform;
                }
            }
        }

        lockedOn = true;
        cursor.GetComponent<Image>().sprite = lockOnCursor;
    }

    public Vector3 LastDirection
    {
        get { return lastDirection; }
        set { lastDirection = value; }
    }


}
