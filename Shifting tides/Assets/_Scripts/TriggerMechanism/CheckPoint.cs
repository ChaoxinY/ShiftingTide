using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") {

            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.lastCheckPointPosition = transform.position;
            Destroy(this);
        }
    }

}
