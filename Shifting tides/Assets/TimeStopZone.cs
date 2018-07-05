using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopZone : MonoBehaviour
{
    private List<GameObject> influncedGameObjects = new List<GameObject>();
    private bool isInfluencing;

    private void Start()
    {
        Invoke("StartInfluencing", 1.4f);
        Invoke("StopInfluencing", 6.7f);
    }

    void Update()
    {
        if (isInfluencing)
        {
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.position, 10f, transform.forward, 0.1f);
            foreach (RaycastHit rh in hits)
            {
                if (rh.collider.gameObject.GetComponent<TimeBoundGameObject>())
                {
                    TimeBoundGameObject tbGameObject = rh.collider.gameObject.GetComponent<TimeBoundGameObject>();
                    tbGameObject.isTimeStopped = true;
                    influncedGameObjects.Add(rh.collider.gameObject);
                }
                if (rh.collider.gameObject.name == "Player")
                { 
                    PlayerSkillModule playerSkillModule = rh.collider.gameObject.GetComponentInChildren<PlayerSkillModule>();
                    playerSkillModule.isTimeStopped = true;
                }
            }
        }
    }

    private void StartInfluencing() { isInfluencing = true; }

    private void StopInfluencing()
    {
        foreach (GameObject influencedGameObject in influncedGameObjects)
        {
            if (influencedGameObject.GetComponent<TimeBoundGameObject>())
            {
                TimeBoundGameObject tbGameObject = influencedGameObject.GetComponent<TimeBoundGameObject>();
                tbGameObject.isTimeStopped = false;
            }
            if (influencedGameObject.gameObject.name == "Player")
            {
                PlayerSkillModule playerSkillModule = influencedGameObject.GetComponent<PlayerSkillModule>();
                playerSkillModule.isTimeStopped = false;
            }
        }
        Destroy(gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject influencedGameObject in influncedGameObjects)
        {
            if (influencedGameObject == other.gameObject)
            {
                if (other.gameObject.GetComponent<TimeBoundGameObject>())
                {
                    TimeBoundGameObject tbGameObject = other.gameObject.GetComponent<TimeBoundGameObject>();
                    tbGameObject.isTimeStopped = false;
                }
                if (other.gameObject.gameObject.name == "Player")
                {
                    PlayerSkillModule playerSkillModule = other.gameObject.GetComponent<PlayerSkillModule>();
                    playerSkillModule.isTimeStopped = false;
                }
            }
        }

    }
}

