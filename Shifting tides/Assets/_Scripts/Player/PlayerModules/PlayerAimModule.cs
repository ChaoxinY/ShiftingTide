using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerAimModule : PlayerModule
{
    private Camera cameraMain;
    private PlayerParticleSystemManager playerParticleSystemManager;
    private PlayerCamera plyCamera;
    private Transform nearestTarget;
    private Vector3 lastDirection;
    private bool lockedOn;

    public Image cursor;
    public Sprite lockOnCursor, lockOffCursor;
    public GameObject bow, bowMesh, shootTarget, currentArrowhead;
    public AudioSource[] bowSoundSources;
    public Vector3 targetOffset;
    public bool isAiming;
    [HideInInspector]
    public int arrowChargingState;

    protected override void Initialize()
    {
        cameraMain = Camera.main;
        playerParticleSystemManager = GameObject.Find("Player").GetComponentInChildren<PlayerParticleSystemManager>();
        plyCamera = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();       
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
        playerParticleSystemManager.StopAllShootingParticleSystems();
        cursor.gameObject.SetActive(false);
        plyCamera.pivotOffset = new Vector3(0, 0.5f, 0);
        plyCamera.camOffset = new Vector3(0, 0.5f, -3);
        plyCamera.ResetSmoothOffsets();
        plyCamera.ResetTargetOffsets();
    }

    public override void ModuleUpdate()
    {
        bow.transform.LookAt(shootTarget.transform);
        cursor.transform.position = Camera.main.WorldToScreenPoint(shootTarget.transform.position);

        if (Input.GetMouseButton(1) && PlayerResourcesManager.IsThereEnoughResource(4, 0))
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
  

    private void ChargeUpArrow()
    {
       
        AimRotate();

        if (playerParticleSystemManager.isPlayingChargingAnimation == false)
        {
            bowSoundSources[1].Play();
            playerParticleSystemManager.PlayChargingAnimation();
        }

        if (playerParticleSystemManager.isPlayingChargedUpAnimation == false)
        {
            playerParticleSystemManager.PlayChargedUpAnimation();
        }

        if (!isAiming) isAiming = !isAiming;
    }

    private void ShootArrow()
    {
        PlayerResourcesManager.Arrows -= 1;
        GameObject Arrow = Instantiate(currentArrowhead, bow.transform.position, bow.transform.rotation);
        Arrow.GetComponent<ArrowBehaviour>().ApplyArrowStageValues(arrowChargingState);
        Arrow.GetComponent<Rigidbody>().AddForce(Arrow.transform.forward * Arrow.GetComponent<ArrowBehaviour>().arrowSpeed, ForceMode.Impulse);
        if (isAiming) isAiming = !isAiming;
        playerParticleSystemManager.PlayerFireAnimation();
        bowSoundSources[0].Play();
    }

    private void AimRotate()
    {
        Vector3 forward = cameraMain.transform.TransformDirection(Vector3.forward);
        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Always rotates the player according to the camera horizontal rotation in aim mode.
        Quaternion targetRotation = Quaternion.Euler(0, plyCamera.angleH, 0);

        float minSpeed = Quaternion.Angle(transform.rotation, targetRotation) * 0.02f;

        // Rotate entire player to face camera.
        lastDirection = forward;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, minSpeed * Time.deltaTime);
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
