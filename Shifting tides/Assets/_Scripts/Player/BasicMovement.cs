using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicMovement : MonoBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 currentMotion, lastDirection, colExtents;
    private GameObject arrow;
    private Camera cameraMain;
    private GameManager gameMng;
    //0: bow , 1: The Source
    private bool[] skillObtained = new bool[10];
    private float h, v, inputSpeed, arrowSpeed, slopeAngle;

    public ParticleSystem shootingParticleSystem;
    public Animator aniPlayer;
    public GameObject bow, bowMesh, UI, gameManagerObject;
    public GameObject[] dashesImages;
    public Ui ui;
    public PlayerCamera plyCamera;
    public bool onGround, isAiming;
    public float defaultMoveForce, moveForce, speedLimit, runLimit, moveLimit, dashForce, DashLimit,
        jumpVel, gravity, maxInput, startArrowSpeed, maxArrowSpeed;

    void Start()
    {
        gameMng = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraMain = Camera.main;
        arrow = Resources.Load("Prefabs/Arrow") as GameObject;
        ResetArrowSpeed();
        rbPlayer = GetComponent<Rigidbody>();
        colExtents = GetComponent<Collider>().bounds.extents;
    }

    void Update()
    {
      
        currentMotion = Input.GetAxisRaw("Vertical") * moveForce * gameManagerObject.transform.forward + Input.GetAxisRaw("Horizontal") * moveForce * gameManagerObject.transform.right;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        inputSpeed = Vector2.ClampMagnitude(new Vector2(h, v), maxInput).magnitude;
        aniPlayer.SetFloat("Speed", inputSpeed);
        onGround = IsGrounded();
        if (!onGround)
        {
            gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
            gameObject.GetComponent<CapsuleCollider>().material.staticFriction = 0f;
        }
        else if (onGround && !IsMoving()) {
            gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 3.34f;
            gameObject.GetComponent<CapsuleCollider>().material.staticFriction = 3.6f;

        }
        else if (onGround)
        {

            gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 0.34f;
            gameObject.GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
        }
        if (!PlayerResourcesManager.IsThisResourceAtMax(5))
        {
            GatherSource();
        }

        if (!isAiming)
        {
            Rotate(h, v);
        }

        ManageInput();
    }

    void FixedUpdate()
    {
        gravity = rbPlayer.velocity.y >= -1 ? 0 : gravity -= 0.7f;
        rbPlayer.AddForce(currentMotion + Vector3.up * (gravity + slopeAngle), ForceMode.Acceleration);
        Vector2 horizontalVelocity = new Vector2(rbPlayer.velocity.x, rbPlayer.velocity.z);
        horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, speedLimit);
        rbPlayer.velocity = new Vector3(horizontalVelocity.x, rbPlayer.velocity.y, horizontalVelocity.y);
    }

    private void ResetArrowSpeed()
    {
        arrowSpeed = startArrowSpeed;
    }

    private void ManageInput() {

        if (Input.GetKeyDown(KeyCode.F) && PlayerResourcesManager.IsThereEnoughResource(3, 0) && skillObtained[1])
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameMng.EnableTimeStop();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumping();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ChangeMoveSpeedLimit(true, moveLimit);
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            ChangeMoveSpeedLimit(false, runLimit);
        }

        if (Input.GetMouseButton(1) && PlayerResourcesManager.IsThereEnoughResource(4, 0) && skillObtained[0])
        {
            ChargeUpArrow();
        }
        if (Input.GetMouseButtonUp(1) && isAiming)
        {
            ShootArrow();
        }

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

    private void Rotate(float horizontal, float vertical)
    {

        Vector3 desiredDirection;
        Vector3 cameraForward = cameraMain.transform.TransformDirection(Vector3.forward);

        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;

        Vector3 right = new Vector3(cameraForward.z, 0, -cameraForward.x);
        desiredDirection = cameraForward * vertical + right * horizontal;
        if (IsMoving() && desiredDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            Quaternion newRotation = Quaternion.Slerp(rbPlayer.rotation, targetRotation, 0.05f);
            rbPlayer.MoveRotation(newRotation);
            LastDirection = desiredDirection;
        }
        if (!IsMoving()) Repositioning();
        
    }

    private void Repositioning()
    {
        if (lastDirection != Vector3.zero)
        {
            LastDirection = new Vector3(LastDirection.x, 0, LastDirection.z);
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            Quaternion newRotation = Quaternion.Slerp(rbPlayer.rotation, targetRotation, 0.05f);
            rbPlayer.MoveRotation(newRotation);
        }
    }

    private bool IsMoving()
    {
        return (h != 0) || (v != 0);
    }

    private void Jumping()
    {
        if (onGround)
        {
            rbPlayer.AddForce((Vector3.up * jumpVel), ForceMode.Impulse);
        }
        else if (PlayerResourcesManager.IsThereEnoughResource(2, 0) && !onGround && gameMng.isTimeStoped)
        {
            Vector3 spawnPosition = gameObject.transform.position - new Vector3(0, 1.2f, 0) + transform.forward * speedLimit / 2.4f;
            if (!IsMoving())
            {
                spawnPosition = gameObject.transform.position - new Vector3(0, 1f, 0);
            }
            PlayerResourcesManager.JumpsLeft -= 1;
            Instantiate(Resources.Load("Prefabs/SourcePlatform") as GameObject, spawnPosition, Quaternion.identity);
        }
    }

    private void ChangeMoveSpeedLimit(bool isWalking, float targetLimit)
    {
        speedLimit = Mathf.Lerp(speedLimit, targetLimit, Time.deltaTime);
        maxInput = isWalking ? maxInput = Mathf.Lerp(maxInput, 0.2f, Time.deltaTime * 5f) : maxInput = Mathf.Lerp(maxInput, 0.7f, Time.deltaTime * 1.2f);
    }

    private IEnumerator Dash()
    {
        ui.dashCharges[(int)PlayerResourcesManager.Dashes - 1].enabled = false;
        PlayerResourcesManager.Dashes -= 1;
        moveForce = dashForce;
        speedLimit = DashLimit;
        maxInput = 1f;
        yield return new WaitForSeconds(0.15f);
        moveForce = defaultMoveForce;
        while (speedLimit > runLimit + 0.2f)
        {
            speedLimit = Mathf.Lerp(speedLimit, runLimit, Time.deltaTime * 5f);
            maxInput = Mathf.Lerp(maxInput, 0.5f, Time.deltaTime * 5f);
            yield return new WaitForSeconds(0.03f);
        }
        if (PlayerResourcesManager.Dashes == 0)
            Invoke("ChargeUpDash", 0.7f);
        yield break;
    }

    private void ChargeUpDash()
    {
        ui.dashCharges[(int)PlayerResourcesManager.Dashes].enabled = true;
        PlayerResourcesManager.Dashes += 1;
    }


    private void ChargeUpArrow()
    {
        AimRotate();        
        arrowSpeed = Mathf.Lerp(arrowSpeed, maxArrowSpeed, Time.deltaTime*1.3f);
        Debug.Log(arrowSpeed);
        if (!isAiming) isAiming = !isAiming;
    }

    private void GatherSource()
    {
        RaycastHit[] hits;
        int layerMask = 1 << 10;
        hits = Physics.SphereCastAll(gameObject.transform.position, 3f, transform.forward, 1f, layerMask);
        foreach (RaycastHit rh in hits)
        {
            SourcePoint sP = rh.collider.gameObject.GetComponent<SourcePoint>();
            sP.objectToChase = gameObject;
            sP.movementSpeed = 20f;
        }
    }

    private bool IsGrounded()
    {
        //Ray ray = new Ray(this.transform.position + Vector3.up * 2 * colExtents.x, Vector3.down);     
        Debug.DrawRay(transform.position - transform.forward, Vector3.down, Color.red);
        //colExtents.x + 1.4f
        return Physics.Raycast(transform.position, Vector3.down, 1.2f);
    }
    private void ShootArrow()
    {
        PlayerResourcesManager.Arrows -= 1;
        GameObject Arrow = Instantiate(arrow, bow.transform.position, bow.transform.rotation);
        Arrow.GetComponent<Rigidbody>().AddForce(Arrow.transform.forward * arrowSpeed, ForceMode.Impulse);
        if (isAiming) isAiming = !isAiming;
        ResetArrowSpeed();
        shootingParticleSystem.Play(true);
        cameraMain.fieldOfView += 1.5f;
       

    }

    void OnCollisionEnter(Collision other)
    {

        switch (other.gameObject.tag)
        {
            case "Bow":
                GameObject.Destroy(other.gameObject);
                skillObtained[0] = true;
                bow.SetActive(true);
                bowMesh.SetActive(true);
                break;
            case "TheSource":
                GameObject.Destroy(other.gameObject);
                skillObtained[1] = true;
                UI.SetActive(true);
                dashesImages[0].SetActive(true);
                PlayerResourcesManager.playerResourcesCaps[3] = 1;
                PlayerResourcesManager.playerResourcesCaps[2] = 2;
                Debug.Log(PlayerResourcesManager.playerResourcesCaps[2]);
                PlayerResourcesManager.Dashes += 1;
                break;
        }
    }

    public Vector3 LastDirection
    {
        get { return lastDirection; }
        set { lastDirection = value; }
    }

}
