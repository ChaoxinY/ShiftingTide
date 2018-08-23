using UnityEngine;
using System.Collections;

public class TreasureGuard : Spawner
{
    public float velocityProjectile;

    private GameObject player;
    private Rigidbody playerRigidBody;
    private bool tracking;

    protected override void Initialize()
    {
        base.Initialize();
        player = GameObject.Find("Player");
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    protected override IEnumerator LocalUpdate()
    {
        yield return base.LocalUpdate();
        if (tracking == true)
        {
            positionToSpawn.LookAt(player.transform);
        }
    }

    public override void MechanismFunction()
    {
        Debug.Log("Fired");
        if (tracking == false)
        {
            tracking = true;
        }
        GameObject target = new GameObject();
        float predictionOffesetX = Random.Range(playerRigidBody.velocity.x * -0.2f, playerRigidBody.velocity.x * 0.2f);
        float predictionOffesetY = Random.Range(playerRigidBody.velocity.y * -0.2f, playerRigidBody.velocity.y * 0.2f);
        float predictionOffesetZ = Random.Range(playerRigidBody.velocity.z * -0.2f, playerRigidBody.velocity.z * 0.2f);

        Vector3 preditcionOffset = new Vector3(predictionOffesetX, predictionOffesetY, predictionOffesetZ);
        Vector3 midCenterOffset = new Vector3(0, 0.4f, 0);

        target.transform.position = player.transform.position + midCenterOffset + preditcionOffset;

        positionToSpawn.LookAt(target.transform);
        GameObject spawnObject = Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.transform.rotation);
        spawnObject.GetComponent<Rigidbody>().AddForce(positionToSpawn.transform.forward * velocityProjectile, ForceMode.Impulse);
    }

}
