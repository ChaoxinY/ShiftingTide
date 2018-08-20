
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerCamera : MonoBehaviour
{
    [HideInInspector]
    public Camera cameraMain;
    public Transform player;

    public Vector3 pivotOffset, camOffset,refVelocity = Vector3.zero;

    [HideInInspector]
    public float mouseX, mouseY, cameraMouseY, angleH, angleV, targetFOV;
    public float smoothSpeed, mouseSensitivity;

    private const float MAX_Y = 60;
    private const float MIN_Y = -60.0f;
    private const float cameraMouseMIN_Y = -20.0f;
    private float targetDistance;
    private Vector3 smoothPivotOffset, smoothCamOffset, targetPivotOffset, targetCamOffset, relCameraPos;
    private RaycastHit hits;
    private float relCameraPosMag;
    private PlayerAimModule playerAimModule;
    private PlayerPhysicsModule playerPhysicsModule;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraMain = Camera.main;
        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;
        transform.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
        angleH = player.eulerAngles.y;
        targetFOV = 60f;
        ResetTargetOffsets();
        playerAimModule = player.gameObject.GetComponentInChildren<PlayerAimModule>();
        playerPhysicsModule = player.gameObject.GetComponentInChildren<PlayerPhysicsModule>();
    }
 
    private void FixedUpdate()
    {
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * mouseSensitivity;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * mouseSensitivity;
        angleV = Mathf.Clamp(angleV, MIN_Y, MAX_Y);

        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
        Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
        transform.rotation = aimRotation;

        Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
        Vector3 noCollisionOffset = targetCamOffset;

        for (float zOffset = targetCamOffset.z; zOffset <= 0; zOffset += 0.4f)
        {
            noCollisionOffset.z = zOffset;
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 0)
            {
                break;
            }
        }
        smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smoothSpeed * Time.deltaTime);
        smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smoothSpeed * Time.deltaTime);

        transform.position = Vector3.SmoothDamp(transform.position, player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset, ref refVelocity, Time.deltaTime * 3f);
    }

    void LateUpdate()
    {
        //inside combat state class   
        transform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(transform.GetComponent<Camera>().fieldOfView, DetermineCurrentFOV(), Time.deltaTime * 1.2f);
    }

    public float DetermineCurrentFOV()
    {
        float targetFOV = 80f;
        if (!playerAimModule.isAiming)
        {
            if (playerPhysicsModule.maxInput >= 0f && playerPhysicsModule.maxInput <= 0.4f)
            {
                return targetFOV;
            }
            else if (playerPhysicsModule.maxInput > 0.4f && playerPhysicsModule.maxInput <= 0.7f)
            {
                return targetFOV = 70f;
            }
            else if (playerPhysicsModule.maxInput > 0.7f)
            {
                return targetFOV = 120f;
            }
        }       
        return targetFOV;
    }

    public IEnumerator DoShake(int timesToShake, float shakeInterval, float shakeAmout)
    {
        Vector3 camPos = cameraMain.transform.position;
        for (int i = 0; i < timesToShake; i++)
        {
            float shakeAmtX = Random.Range(-shakeAmout, shakeAmout);
            float shakeAmtY = Random.Range(-shakeAmout, shakeAmout);
            camPos.x += shakeAmtX;
            camPos.y += shakeAmtY;
            Debug.Log(camPos);
            cameraMain.transform.position = camPos;
            yield return new WaitForSeconds(shakeInterval);
        }
        yield break;
    }

    public void ResetTargetOffsets()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }

    public void ResetSmoothOffsets()
    {
        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
    }

    private bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
    {
        float playerFocusHeight = player.GetComponentInChildren<CapsuleCollider>().center.y;
        return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight, offset);
    }

    private bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
    {
        RaycastHit hit;

        // If a raycast from the check position to the player hits something...
        if (Physics.Raycast(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, out hit, relCameraPosMag))
        {
            if (hit.transform.gameObject.GetComponent<Collider>())
            {
                // ... if it is not the player...
                if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
                {
                    // This position isn't appropriate.
                    return false;
                }
            }
        }
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return true;
    }

    // Check for collision from player to camera.
    private bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position, out hit, maxDistance))
        {
            if (hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

}
