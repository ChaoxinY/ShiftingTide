using UnityEngine;
using System.Collections;

public class PlayerSkillModule : PlayerModule
{
    private bool dashWarmUp;
    private PlayerTideComboManager playerTideComboManager;
    private PlayerPhysicsModule playerPhysicsModule;

    public GameObject[] dashesImages;
    public float jumpVel, dashForce, dashLimit;
    public bool isTimeStopped;

    void Start()
    {
        playerTideComboManager = GetComponent<PlayerTideComboManager>();
        playerPhysicsModule = GetComponent<PlayerPhysicsModule>();
    }

    public override void ModuleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F) && PlayerResourcesManager.IsThereEnoughResource(3, 0) && !dashWarmUp)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumping();
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
        if (playerPhysicsModule.onGround)
        {
            playerPhysicsModule.rigidbodyPlayer.AddForce((Vector3.up * jumpVel), ForceMode.Impulse);
        }
        else if (PlayerResourcesManager.IsThereEnoughResource(2, 0) && !playerPhysicsModule.onGround && isTimeStopped)
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
