using UnityEngine;
using System.Collections;

public class AreaUIDisplayer : MonoBehaviour
{
    public Canvas displayUI;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            displayUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            displayUI.transform.LookAt(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            Invoke("CanvasFade", 1f);
        }
    }

    private bool IsPlayer(GameObject enteredGameObject)
    {
        bool playerEntered = false;
        if (enteredGameObject.tag == "Player")
        {
            playerEntered = true;
        }
        return playerEntered;
    }

    private void CanvasFade() {

        displayUI.gameObject.SetActive(false);
    }
}
