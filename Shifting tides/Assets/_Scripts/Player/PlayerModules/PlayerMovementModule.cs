using UnityEngine;
using System.Collections;

public class PlayerMovementModule : PlayerModule
{
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerPhysicsModule playerPhysicsModule;
    private PlayerAimModule playerAimModule;
    private Camera cameraMain;
    private Vector3 currentMotion, colExtents, lastDirection;
    private float gravity;

    public GameObject gameManagerObject;
    public float moveLimit, jumpVel;

    protected override void Initialize()
    {  
        cameraMain = Camera.main;
        playerPhysicsModule = GameObject.Find("Player").GetComponentInChildren<PlayerPhysicsModule>();
        playerAnimatorManager = GameObject.Find("Player").GetComponent<PlayerAnimatorManager>();
        playerAimModule = GameObject.Find("Player").GetComponentInChildren<PlayerAimModule>();
        colExtents = GetComponentInParent<Collider>().bounds.extents;
        InitializeModuleID();
    }

    public override void InitializeModuleID()
    {
        ModuleID = 1;
    }

    public override void ModuleUpdate()
    {
        playerPhysicsModule.horizontalInput = Input.GetAxis("Horizontal");
        playerPhysicsModule.verticalInput = Input.GetAxis("Vertical");

        currentMotion = Input.GetAxisRaw("Vertical") * playerPhysicsModule.moveForce * gameManagerObject.transform.forward
            + Input.GetAxisRaw("Horizontal") * playerPhysicsModule.moveForce * gameManagerObject.transform.right;

        float inputSpeed = Vector2.ClampMagnitude(new Vector2(playerPhysicsModule.horizontalInput,
            playerPhysicsModule.verticalInput), playerPhysicsModule.maxInput).magnitude;

        playerAnimatorManager.InputSpeed = inputSpeed;
        playerAnimatorManager.Input = Input.anyKey;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ChangeMoveSpeedLimit(true, moveLimit);
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            ChangeMoveSpeedLimit(false, playerPhysicsModule.runLimit);
        }
        if (Input.GetKeyDown(KeyCode.Space) && playerPhysicsModule.onGround)
        {
            Jumping();
        }
       
        if (!playerAimModule.isAiming)
        {
            Rotate(playerPhysicsModule.horizontalInput, playerPhysicsModule.verticalInput);
        }
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
            Quaternion newRotation = Quaternion.Slerp(playerPhysicsModule.rigidbodyPlayer.rotation, targetRotation, 0.05f);
            playerPhysicsModule.rigidbodyPlayer.MoveRotation(newRotation);
            LastDirection = desiredDirection;
        }
        if (!IsMoving()) Repositioning();
    }
    public override void ModuleFixedUpdate()
    {
        gravity = playerPhysicsModule.rigidbodyPlayer.velocity.y >= -1 ? 0 : gravity -= 0.7f;
        playerPhysicsModule.rigidbodyPlayer.AddForce(currentMotion + Vector3.up * gravity, ForceMode.Acceleration);
        Vector2 horizontalVelocity = new Vector2(playerPhysicsModule.rigidbodyPlayer.velocity.x, playerPhysicsModule.rigidbodyPlayer.velocity.z);
        horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, playerPhysicsModule.speedLimit);
        playerPhysicsModule.rigidbodyPlayer.velocity = new Vector3(horizontalVelocity.x, playerPhysicsModule.rigidbodyPlayer.velocity.y, horizontalVelocity.y);
    }
    private void Repositioning()
    {
        if (lastDirection != Vector3.zero)
        {
            LastDirection = new Vector3(LastDirection.x, 0, LastDirection.z);
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            Quaternion newRotation = Quaternion.Slerp(playerPhysicsModule.rigidbodyPlayer.rotation, targetRotation, 0.05f);
            playerPhysicsModule.rigidbodyPlayer.MoveRotation(newRotation);
        }
    }
    private void Jumping()
    {
       playerAnimatorManager.PlayJumpAnimation();
       playerPhysicsModule.rigidbodyPlayer.AddForce((Vector3.up * jumpVel), ForceMode.Impulse);      
    }

    private void ChangeMoveSpeedLimit(bool isWalking, float targetLimit)
    {
        playerPhysicsModule.speedLimit = Mathf.Lerp(playerPhysicsModule.speedLimit, targetLimit, Time.deltaTime);
        playerPhysicsModule.maxInput = isWalking ? playerPhysicsModule.maxInput =
            Mathf.Lerp(playerPhysicsModule.maxInput, 0.2f, Time.deltaTime * 5f) :
       playerPhysicsModule.maxInput = Mathf.Lerp(playerPhysicsModule.maxInput, 0.7f, Time.deltaTime * 1.2f);
    }

    public Vector3 LastDirection
    {
        get { return lastDirection; }
        set { lastDirection = value; }
    }

    private bool IsMoving()
    {
        return (playerPhysicsModule.horizontalInput != 0) || (playerPhysicsModule.verticalInput != 0);
    }

 
}
