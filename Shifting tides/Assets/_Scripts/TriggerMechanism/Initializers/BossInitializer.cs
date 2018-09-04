using UnityEngine;
using System.Collections.Generic;

public class BossInitializer : MonoBehaviour
{
    public List<Collider> colliders;
    private BossResourceManager bossResourceManager;


    private void Start()
    { 
        bossResourceManager = GetComponentInParent<BossResourceManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") {
            bossResourceManager.enabled = true;
            foreach (Collider collider in colliders) {
                collider.enabled = true;
            }
            Destroy(this);
        }
    }


}