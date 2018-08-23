using UnityEngine;
using System.Collections;

//Used for projectile spawning(Arrows, fire, Cannonballs etc)
public class ProjectileSpawner : Spawner
{
    public float velocityProjectile;

    public override void MechanismFunction()
    {
        GameObject spawnObject = Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.transform.rotation);
        Component[] childRigidBodies = spawnObject.GetComponentsInChildren(typeof(Rigidbody));
        if (childRigidBodies.Length != 0)
        {
            foreach (Rigidbody rb in childRigidBodies)
            {
                rb.AddForce(positionToSpawn.transform.forward * velocityProjectile, ForceMode.Impulse);
            }
        }
    }
}
