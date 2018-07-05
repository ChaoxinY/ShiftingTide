using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopZone : MonoBehaviour
{
    public List<GameObject> influencedGameObjects = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating("AreaDamage", 1.4f, 0.65f);
        StartCoroutine(StopInfluencing());
    }

    private void AreaDamage()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            if (influencedGameObject.GetComponentInParent<HostileResourceManager>())
            {
                HostileResourceManager hostileResource = influencedGameObject.GetComponentInParent<HostileResourceManager>();
                hostileResource.CurrentHealth -= 2.5f;
            }
        }
    }

    private IEnumerator StopInfluencing()
    {
        yield return new WaitForSeconds(6.7f);
        yield return StartCoroutine(ReturnGameObjectToNormalState());
        Destroy(transform.parent.gameObject);
        yield break;
    }

    private IEnumerator ReturnGameObjectToNormalState()
    {
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            if (influencedGameObject.GetComponent<TimeBoundGameObject>())
            {
                TimeBoundGameObject tbGameObject = influencedGameObject.GetComponent<TimeBoundGameObject>();
                tbGameObject.isTimeStopped = false;
            }
            if (influencedGameObject.gameObject.tag == "Enemy")
            {
                TimeBoundGameObject tbGameObject = influencedGameObject.GetComponentInParent<TimeBoundGameObject>();
                tbGameObject.isTimeStopped = false;
            }
            if (influencedGameObject.gameObject.name == "Player")
            {
                PlayerSkillModule playerSkillModule = influencedGameObject.GetComponentInChildren<PlayerSkillModule>();
                playerSkillModule.isTimeStopped = false;
            }
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForGameObjectToInfluence(other.gameObject);
    }

    private void CheckForGameObjectToInfluence(GameObject gameObjectCaught)
    {
        if (gameObjectCaught.GetComponent<TimeBoundGameObject>())
        {
            TimeBoundGameObject tbGameObject = gameObjectCaught.GetComponent<TimeBoundGameObject>();
            tbGameObject.isTimeStopped = true;
            influencedGameObjects.Add(gameObjectCaught.gameObject);
        }
        if (gameObjectCaught.gameObject.tag == "Enemy")
        {
            TimeBoundGameObject tbGameObject = gameObjectCaught.GetComponentInParent<TimeBoundGameObject>();
            tbGameObject.isTimeStopped = true;
            influencedGameObjects.Add(gameObjectCaught.gameObject);
        }
        if (gameObjectCaught.gameObject.name == "Player")
        {
            PlayerSkillModule playerSkillModule = gameObjectCaught.GetComponentInChildren<PlayerSkillModule>();
            playerSkillModule.isTimeStopped = true;
            influencedGameObjects.Add(gameObjectCaught.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            if (influencedGameObject == other.gameObject)
            {
                if (other.gameObject.gameObject.name == "Player")
                {
                    PlayerSkillModule playerSkillModule = other.gameObject.GetComponentInChildren<PlayerSkillModule>();
                    playerSkillModule.isTimeStopped = false;
                }
            }
        }

    }
}


