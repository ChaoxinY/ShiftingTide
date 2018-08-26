using UnityEngine;
using System.Collections;

public class TreasureGuard : Spawner
{
    public Transform meshTransform;
    public float velocityProjectile;

    private AgentAnimatorManager agentAnimatorManager;
    private GameObject player;
    private Rigidbody playerRigidBody;
    private bool tracking;

    protected override void Initialize()
    {
        agentAnimatorManager = GetComponent<AgentAnimatorManager>();
        player = GameObject.Find("Player");
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    protected override IEnumerator LocalUpdate()
    {
        yield return base.LocalUpdate();
        if (tracking) { meshTransform.LookAt(player.transform); }
        if (Triggered == false) { agentAnimatorManager.Active = false; }
    }

    public override IEnumerator MechanismFunction()
    {
        Debug.Log("Fired");
        if (tracking == false) { tracking = true;}
        if (agentAnimatorManager.Active == false) { agentAnimatorManager.Active = true; }
        yield return new WaitUntil(() =>
        StaticToolMethods.GetAnimatorStateInfo(0, agentAnimatorManager.agentAnimator).IsName("Idle"));
        GameObject target = new GameObject();
        float predictionOffesetX = Random.Range(playerRigidBody.velocity.x * -0.2f, playerRigidBody.velocity.x * 0.2f);
        float predictionOffesetY = Random.Range(playerRigidBody.velocity.y * -0.2f, playerRigidBody.velocity.y * 0.2f);
        float predictionOffesetZ = Random.Range(playerRigidBody.velocity.z * -0.2f, playerRigidBody.velocity.z * 0.2f);

        Vector3 preditcionOffset = new Vector3(predictionOffesetX, predictionOffesetY, predictionOffesetZ);
        Vector3 midCenterOffset = new Vector3(0, 0.4f, 0);

        target.transform.position = player.transform.position + midCenterOffset + preditcionOffset;

        positionToSpawn.LookAt(target.transform);

        agentAnimatorManager.PlayAttackAnimation();
        yield return new WaitForSeconds(0.2f);
        GameObject spawnObject = Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.transform.rotation);
        spawnObject.GetComponent<Rigidbody>().AddForce(positionToSpawn.transform.forward * velocityProjectile, ForceMode.Impulse);
        yield return new WaitUntil(() => 
        !StaticToolMethods.GetAnimatorStateInfo(0, agentAnimatorManager.agentAnimator).IsName("Attacking"));
        yield break;
    }   
}
