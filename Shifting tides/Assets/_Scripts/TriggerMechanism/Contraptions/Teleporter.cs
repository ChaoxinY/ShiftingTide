using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    [SerializeField]
    private Transform teleportDestination;
    
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = teleportDestination.position;
    }
}
