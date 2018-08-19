using UnityEngine;
using System.Collections;

public class PlayerPhysicsModule : PlayerModule
{
    public Rigidbody rigidbodyPlayer;
    public bool onGround;
    [HideInInspector]
    public float horizontalInput, verticalInput;
    public float defaultMoveForce, moveForce, speedLimit, runLimit, maxInput;

    private PlayerAnimatorManager playerAnimatorManager;


    protected override void Initialize()
    {
        playerAnimatorManager = GameObject.Find("Player").GetComponent<PlayerAnimatorManager>();
        InitializeModuleID();
    }

    public override void InitializeModuleID()
    {
        ModuleID = 0;
    }

    public override void ModuleUpdate()
    {
        onGround = IsGrounded();
      
        if (!onGround)
        {
            GetComponentInParent<CapsuleCollider>().material.dynamicFriction = 0f;
            GetComponentInParent<CapsuleCollider>().material.staticFriction = 0f;
            if (!BumpCheck())
            {
                playerAnimatorManager.OnGround = false;
            }
            else if (BumpCheck()) {
                playerAnimatorManager.OnGround = true;
            }
        }
        else if (onGround && !IsMoving())
        {
            GetComponentInParent<CapsuleCollider>().material.dynamicFriction = 3.34f;
            GetComponentInParent<CapsuleCollider>().material.staticFriction = 3.6f;

        }
        else if (onGround)
        {
            GetComponentInParent<CapsuleCollider>().material.dynamicFriction = 0.34f;
            GetComponentInParent<CapsuleCollider>().material.staticFriction = 0.6f;
            playerAnimatorManager.OnGround = true;
          
        }
    }

    private bool BumpCheck() {

        bool hitABump = true;
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity);
        if (hit.distance > 1.5f) { hitABump = false; }
        return hitABump;
    }

    private bool IsGrounded()
    {
        //Ray ray = new Ray(this.transform.position + Vector3.up * 2 * colExtents.x, Vector3.down);     
        //Debug.DrawRay(transform.position - transform.forward, Vector3.down, Color.red);
        //colExtents.x + 1.4f
        return Physics.Raycast(transform.position, Vector3.down, 1.2f);
    }

    private bool IsMoving()
    {
        return (horizontalInput != 0) || (verticalInput != 0);
    }
}
